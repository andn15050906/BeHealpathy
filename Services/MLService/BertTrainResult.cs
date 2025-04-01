using Microsoft.ML;
using Microsoft.ML.Data;

namespace MLService;

public class BertTrainResult
{
    public CalibratedBinaryClassificationMetrics Metrics { get; set; }
    public ITransformer TrainedModel { get; set; }
}
