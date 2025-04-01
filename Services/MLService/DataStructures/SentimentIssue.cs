using Microsoft.ML.Data;

namespace MLService.DataStructures;

public sealed class SentimentIssue
{
    [LoadColumn(0)]
    public bool Label { get; set; }

    [LoadColumn(2)]
    public string Text { get; set; } = string.Empty;
}