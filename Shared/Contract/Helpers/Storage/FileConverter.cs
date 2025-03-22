using Contract.Requests.Progress.McqRequests.Dtos;
using Contract.Requests.Progress.SurveyRequests.Dtos;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using SixLabors.ImageSharp;

namespace Contract.Helpers.Storage;

/// <summary>
/// Using SixLabors.ImageSharp.
/// System.Drawing.Common is only supported on Windows
/// </summary>
public static class FileConverter
{
    public const string EXTENSION_JPG = ".jpg";



    /// <summary>
    /// Does not close the returned stream
    /// </summary>
    public static async Task<Stream> ToJpg(IFormFile file)
    {
        if (file.ContentType == "image/jpeg")
            return file.OpenReadStream();

        using MemoryStream imgStream = new();
        await file.CopyToAsync(imgStream);
        imgStream.Seek(0, SeekOrigin.Begin);
        using var image = await Image.LoadAsync(imgStream);

        MemoryStream jpgStream = new();
        await image.SaveAsJpegAsync(jpgStream);
        jpgStream.Position = 0;
        return jpgStream;
    }



    /// <summary>
    /// Only 1 Survey per file
    /// If the cell is empty, consider it is not a new record of the type
    /// </summary>
    public static CreateSurveyDto ProcessSurveyFromExcelFile(IFormFile file)
    {
        CreateSurveyDto dto = new();

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            using var package = new ExcelPackage(stream);

            // worksheet 1 - Questions
            var worksheet = package.Workbook.Worksheets[0];
            int lastRow = worksheet.Dimension.End.Row;
            while (lastRow >= 1)
            {
                var range = worksheet.Cells[lastRow, 1, lastRow, 6];
                if (range.Any(c => c.Value != null)) {
                    break;
                }
                lastRow--;
            }
            int rowCount = lastRow;
            int startRow = 2;

            dto.Name = worksheet.Cells[startRow, 1].Text;
            dto.Description = worksheet.Cells[startRow, 2].Text;
            dto.Questions = [];
            var currentQuestion = new CreateMcqQuestionDto();
            for (int row = startRow; row < rowCount; row++)
            {
                var questionContent = worksheet.Cells[row, 3].Text;
                var questionExplanation = worksheet.Cells[row, 4].Text;
                if (questionContent != currentQuestion.Content && questionContent != string.Empty)
                {
                    currentQuestion = new CreateMcqQuestionDto
                    {
                        Content = questionContent,
                        Explanation = questionExplanation,
                        Answers = []
                    };
                    dto.Questions.Add(currentQuestion);
                }

                try
                {
                    dto.Questions[^1].Answers.Add(new CreateMcqAnswerDto
                    {
                        Content = worksheet.Cells[row, 5].Text,
                        Score = int.Parse(worksheet.Cells[row, 6].Text)
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    continue;
                }
            }

            // worksheet 2 - Bands
            worksheet = package.Workbook.Worksheets[1];
            rowCount = worksheet.Dimension.Rows;
            startRow = 2;

            dto.Bands = [];
            var currentBand = new CreateSurveyScoreBandDto();
            for (int row = startRow; row <= rowCount; row++)
            {
                var bandName = worksheet.Cells[row, 1].Text;
                if (bandName != currentBand.BandName && bandName != string.Empty)
                    currentBand.BandName = bandName;

                currentBand = new CreateSurveyScoreBandDto
                {
                    BandName = currentBand.BandName,
                    BandRating = worksheet.Cells[row, 2].Text,
                    MinScore = int.Parse(worksheet.Cells[row, 3].Text),
                    MaxScore = int.Parse(worksheet.Cells[row, 4].Text)
                };
                dto.Bands.Add(currentBand);
            }
        }

        return dto;
    }
}