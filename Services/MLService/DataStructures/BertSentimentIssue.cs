using Microsoft.ML.Data;

namespace MLService.DataStructures;

public sealed class BertSentimentIssue
{
    [LoadColumn(0)]
    public bool Label { get; set; }

    [LoadColumn(2)]
    public string Text { get; set; } = string.Empty;
}