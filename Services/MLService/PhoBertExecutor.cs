using Microsoft.ML;
using MLService.DataStructures;
using static Microsoft.ML.DataOperationsCatalog;

namespace MLService;

public sealed class PhoBertExecutor
{
    private static Dictionary<int, PredictionEngine<PhoBertSentimentIssue, SentimentPrediction>> _engines = [];

    private readonly string _dataPath;
    private readonly string _modelPath;
    private readonly int _seed;

    public PhoBertExecutor(string dataPath, string modelPath, int seed)
    {
        _dataPath = dataPath;
        _modelPath = modelPath;

        // Create prediction engine related to the loaded trained model
        if (!_engines.TryGetValue(seed, out _))
        {
            var result = PrepareContext();
            var mlContext = result.Item1;
            var trainResult = result.Item2;
            lock (_engines)
            {
                if (!_engines.TryGetValue(seed, out _))
                    _engines.Add(seed, mlContext.Model.CreatePredictionEngine<PhoBertSentimentIssue, SentimentPrediction>(trainResult.TrainedModel));
            }
        }
        _seed = seed;
    }

    public SentimentPrediction Predict(string inputText)
    {
        // Make a single test prediction, loading the model from .ZIP file
        return _engines[_seed].Predict(new PhoBertSentimentIssue { Text = inputText });
    }

    private (MLContext, BertTrainResult) PrepareContext()
    {
        // Create MLContext to be shared across the model creation workflow objects 
        // Set a random seed for repeatable/deterministic results across multiple trainings.
        var mlContext = new MLContext(_seed);

        // STEP 1: Common data loading configuration
        IDataView dataView = mlContext.Data.LoadFromTextFile<PhoBertSentimentIssue>(_dataPath, hasHeader: true);

        TrainTestData trainTestSplit = mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        IDataView trainingData = trainTestSplit.TrainSet;
        IDataView testData = trainTestSplit.TestSet;

        // STEP 2: Common data process configuration with pipeline data transformations          
        var dataProcessPipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(PhoBertSentimentIssue.Text));

        // STEP 3: Set the training algorithm, then create and config the modelBuilder                            
        var trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");
        var trainingPipeline = dataProcessPipeline.Append(trainer);

        // STEP 4: Train the model fitting to the DataSet
        var trainedModel = trainingPipeline.Fit(trainingData);

        // STEP 5: Evaluate the model and show accuracy stats
        var predictions = trainedModel.Transform(testData);
        var metrics = mlContext.BinaryClassification.Evaluate(data: predictions, labelColumnName: "Label", scoreColumnName: "Score");

        // STEP 6: Save/persist the trained model to a .ZIP file
        //mlContext.Model.Save(trainedModel, trainingData.Schema, _modelPath);

        //Console.WriteLine("The model is saved to {0}", ModelPath);

        return (mlContext, new BertTrainResult { Metrics = metrics, TrainedModel = trainedModel });
    }
}