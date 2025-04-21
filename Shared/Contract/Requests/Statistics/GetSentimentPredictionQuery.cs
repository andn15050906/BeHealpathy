using Contract.Responses.Statistics;

namespace Contract.Requests.Statistics;

public record GetSentimentPredictionQuery(List<InputByDay> inputs);

public record InputByDay(DateTime date, List<string> text);

public record AnalysisOutputByDay(DateTime date, Output.Analysis analysis);