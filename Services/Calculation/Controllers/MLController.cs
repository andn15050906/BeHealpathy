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

    private static AnalysisCache _cache;

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

        _cache ??= new AnalysisCache();
    }

    [HttpPost]
    public IActionResult Predict([FromBody] GetSentimentPredictionQuery query)
    {
        if (query.inputs.Count == 0)
            return BadRequest();

        List<AnalysisOutputByDay> response = [];

        foreach (var input in query.inputs)
        {
            if (input != null)
            {
                foreach (var textInput in input.text)
                    try
                    {
                        var analysis = _cache.TryGet(textInput);
                        if (analysis is null)
                        {
                            analysis = Analyze(textInput);
                            _cache.Set(textInput, analysis);
                        }

                        response.Add(new AnalysisOutputByDay(input.date, analysis));
                    }
                    catch (Exception)
                    {
                        continue;
                    }
            }
        }

        return Ok(response);
    }

    private OutputAnalysis Analyze(string message)
    {
        try
        {
            var angerResult = new PhoBertExecutor(AngerPhoBertDataPath, AngerPhoBertModelPath, 1).Predict(message);
            if (angerResult.Probability > 0.3)
                return new OutputAnalysis(true, angerResult.Probability, angerResult.Score, ["anger"]);

            var enjoymentResult = new PhoBertExecutor(EnjoymentPhoBertDataPath, EnjoymentPhoBertModelPath, 2).Predict(message);
            if (enjoymentResult.Probability > 0.3)
                return new OutputAnalysis(false, enjoymentResult.Probability, enjoymentResult.Score, ["enjoyment"]);

            var sadnessResult = new PhoBertExecutor(SadnessPhoBertDataPath, SadnessPhoBertModelPath, 3).Predict(message);
            if (sadnessResult.Probability > 0.3)
                return new OutputAnalysis(true, sadnessResult.Probability, sadnessResult.Score, ["sadness"]);

            var fearResult = new PhoBertExecutor(FearPhoBertDataPath, FearPhoBertModelPath, 4).Predict(message);
            if (fearResult.Probability > 0.3)
                return new OutputAnalysis(true, fearResult.Probability, fearResult.Score, ["fear"]);

            var bertResult = new BertExecutor(BertDataPath, BertModelPath).Predict(message);
            return new OutputAnalysis(bertResult.Prediction, bertResult.Probability, bertResult.Score);
        }
        catch (Exception)
        {
            return new OutputAnalysis(false, 0.2f, 0.2f);
        }
    }

    public record InputByDay(DateTime date, List<string> text);
    public record AnalysisOutputByDay(DateTime date, OutputAnalysis analysis);
    public record GetSentimentPredictionQuery(List<InputByDay> inputs);

    public class OutputAnalysis
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

    public class AnalysisCache
    {
        private Dictionary<string, OutputAnalysis> _cache = [];

        public void Set(string input, OutputAnalysis output)
        {
            try
            {
                _cache[input] = output;
            }
            catch (Exception) { }
        }

        public OutputAnalysis? TryGet(string input)
        {
            if (_cache.TryGetValue(input, out var result))
                return result;
            return null;
        }
    }
}