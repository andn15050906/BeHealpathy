using Microsoft.AspNetCore.Mvc;
using MLService;

namespace Calculation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MLController : ControllerBase
{
    private readonly string BertDataPath;
    private readonly string BertModelPath;

    private readonly string AngerPhoBertDataPath;
    private readonly string AngerPhoBertModelPath;
    private readonly string EnjoymentPhoBertDataPath;
    private readonly string EnjoymentPhoBertModelPath;
    private readonly string FearPhoBertDataPath;
    private readonly string FearPhoBertModelPath;
    private readonly string SadnessPhoBertDataPath;
    private readonly string SadnessPhoBertModelPath;

    public MLController()
    {
        BertDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "wikiDetoxAnnotated40kRows.tsv");
        BertModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SentimentModel.zip");

        AngerPhoBertDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "train_nor_811_Anger.tsv");
        AngerPhoBertModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "AngerSentimentModel.zip");
        EnjoymentPhoBertDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "train_nor_811_Enjoyment.tsv");
        EnjoymentPhoBertModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "EnjoymentSentimentModel.zip");
        FearPhoBertDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "train_nor_811_Fear.tsv");
        FearPhoBertModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "FearSentimentModel.zip");
        SadnessPhoBertDataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "train_nor_811_Sadness.tsv");
        SadnessPhoBertModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SadnessSentimentModel.zip");
    }

    [HttpGet]
    public IActionResult Predict([FromQuery] Dto dto)
    {
        if (string.IsNullOrEmpty(dto.MessageInput))
            return BadRequest();

        string emotion = string.Empty;
        OutputAnalysis response;

        var angerResult = new PhoBertExecutor(AngerPhoBertDataPath, AngerPhoBertModelPath, 1).Predict(dto.MessageInput);
        if (angerResult.Probability > 0.3)
        {
            response = new OutputAnalysis(true, angerResult.Probability, angerResult.Score, ["anger"]);
        }
        else
        {
            var enjoymentResult = new PhoBertExecutor(EnjoymentPhoBertDataPath, EnjoymentPhoBertModelPath, 2).Predict(dto.MessageInput);
            if (enjoymentResult.Probability > 0.3)
            {
                response = new OutputAnalysis(false, enjoymentResult.Probability, enjoymentResult.Score, ["enjoyment"]);
            }
            else
            {
                var sadnessResult = new PhoBertExecutor(SadnessPhoBertDataPath, SadnessPhoBertModelPath, 3).Predict(dto.MessageInput);
                if (sadnessResult.Probability > 0.3)
                {
                    response = new OutputAnalysis(true, sadnessResult.Probability, sadnessResult.Score, ["sadness"]);
                }
                else
                {
                    var fearResult = new PhoBertExecutor(FearPhoBertDataPath, FearPhoBertModelPath, 4).Predict(dto.MessageInput);
                    if (fearResult.Probability > 0.3)
                    {
                        response = new OutputAnalysis(true, fearResult.Probability, fearResult.Score, ["fear"]);
                    }
                    else
                    {
                        var bertResult = new BertExecutor(BertDataPath, BertModelPath).Predict(dto.MessageInput);
                        response = new OutputAnalysis(bertResult.Prediction, bertResult.Probability, bertResult.Score);
                    }
                }
            }
        }

        return Ok(response);
    }

    public class Dto
    {
        public string MessageInput { get; set; }
    }

    class OutputAnalysis
    {
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }

        public List<string> Emotions { get; set; } = [];
        public List<string> Keywords { get; set; } = [];
        public List<string> Topics { get; set; } = [];

        public OutputAnalysis()
        {

        }

        public OutputAnalysis(bool prediction, float probability, float score)
        {
            Prediction = prediction;
            Probability = probability;
            Score = score;
        }

        public OutputAnalysis(bool prediction, float probability, float score, List<string> emotions)
        {
            Prediction = prediction;
            Probability = probability;
            Score = score;
            Emotions = emotions;
        }
    }
}