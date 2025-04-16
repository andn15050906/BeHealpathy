using Microsoft.ML.Data;

namespace MLService.DataStructures;

public sealed class PhoBertSentimentIssue
{
    [LoadColumn(1)]
    public bool Label { get; set; }

    [LoadColumn(2)]
    public string Text { get; set; } = string.Empty;
}