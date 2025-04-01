using Microsoft.ML;
using MLService.DataStructures;
using static Microsoft.ML.DataOperationsCatalog;

namespace MLService;

public sealed class BertExecutor
{
    private readonly string _dataPath;
    private readonly string _modelPath;
    private static PredictionEngine<SentimentIssue, SentimentPrediction>? _engine = null;

    public BertExecutor(string dataPath, string modelPath)
    {
        _dataPath = dataPath;
        _modelPath = modelPath;

        // Create prediction engine related to the loaded trained model
        if (_engine == null)
        {
            var result = PrepareContext();
            var mlContext = result.Item1;
            var trainResult = result.Item2;
            _engine = mlContext.Model.CreatePredictionEngine<SentimentIssue, SentimentPrediction>(trainResult.TrainedModel);
        }
    }

    public SentimentPrediction Predict(string inputText)
    {
        // Make a single test prediction, loading the model from .ZIP file
        return _engine!.Predict(new SentimentIssue { Text = inputText });
    }

    private (MLContext, BertTrainResult) PrepareContext()
    {
        // Create MLContext to be shared across the model creation workflow objects 
        // Set a random seed for repeatable/deterministic results across multiple trainings.
        var mlContext = new MLContext(seed: 1);

        // STEP 1: Common data loading configuration
        IDataView dataView = mlContext.Data.LoadFromTextFile<SentimentIssue>(_dataPath, hasHeader: true);

        TrainTestData trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        IDataView trainingData = trainTestSplit.TrainSet;
        IDataView testData = trainTestSplit.TestSet;

        // STEP 2: Common data process configuration with pipeline data transformations          
        var dataProcessPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(SentimentIssue.Text));

        // STEP 3: Set the training algorithm, then create and config the modelBuilder                            
        var trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");
        var trainingPipeline = dataProcessPipeline.Append(trainer);

        // STEP 4: Train the model fitting to the DataSet
        var trainedModel = trainingPipeline.Fit(trainingData);

        // STEP 5: Evaluate the model and show accuracy stats
        var predictions = trainedModel.Transform(testData);
        var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label", scoreColumnName: "Score");

        // STEP 6: Save/persist the trained model to a .ZIP file
        mlContext.Model.Save(trainedModel, trainingData.Schema, _modelPath);

        //Console.WriteLine("The model is saved to {0}", ModelPath);

        return (mlContext, new BertTrainResult { Metrics = metrics, TrainedModel = trainedModel });
    }
}