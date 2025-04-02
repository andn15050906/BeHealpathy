using Microsoft.AspNetCore.Mvc;
using MLService;

namespace Calculation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MLController : ControllerBase
{
    private readonly string DataPath;
    private readonly string ModelPath;

    public MLController()
    {
        DataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "wikiDetoxAnnotated40kRows.tsv");
        ModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "SentimentModel.zip");
    }

    [HttpGet]
    public IActionResult Predict([FromQuery] Dto dto)
    {
        if (string.IsNullOrEmpty(dto.MessageInput))
            return BadRequest();

        var result = new BertExecutor(DataPath, ModelPath).Predict(dto.MessageInput);

        var response = new OutputAnalysis
        {
            Prediction = result.Prediction,
            Probability = result.Probability,
            Score = result.Score
        };
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
    }
}