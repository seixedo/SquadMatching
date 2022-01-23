
using System;
using System.Linq;
using Microsoft.ML.Data;
using Microsoft.ML;
using Microsoft.ML.Recommender;
using Microsoft.ML.Trainers;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;

namespace SquadMatching.MLDOTNET
{
    public class DataBaseLoader
    {
        public void Main()
        {
            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
            EvaluateModel(mlContext, testDataView, model);
           var resultado= UseModelForSinglePrediction(mlContext, model);
        }
        public static (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
           
            string constr = "workstation id=squadmatch.mssql.somee.com;packet size=4096;user id=seixedo_SQLLogin_1;pwd=g6512iqcac;data source=squadmatch.mssql.somee.com;persist security info=False;initial catalog=squadmatch;";


            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<UserHabData>();
            string sqlCommand = "SPC_MEDIA";
            string sqlCommand2 = "select * from REC_TEST";

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, constr, sqlCommand);
            DatabaseSource dbSource2 = new DatabaseSource(SqlClientFactory.Instance, constr, sqlCommand2);
            var sourcer = dbSource.GetType();
            IDataView trainingdData = loader.Load(dbSource);
            IDataView testData = loader.Load(dbSource2);
            Single[] teste3 = new Single[100];
            teste3= testData.GetColumn<Single>("MEDIA").ToArray();

            IEnumerable<Single> test2 = testData.GetColumn<Single>("MEDIA");
            return (trainingdData, testData);
        }
        public static ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "CD_ALUNOEncoded", inputColumnName: "CD_ALUNO")
    .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "CD_HABILIDADEEncoded", inputColumnName: "CD_HABILIDADE"));

            var options = new MatrixFactorizationTrainer.Options
            {
                MatrixColumnIndexColumnName = "CD_ALUNOEncoded",
                MatrixRowIndexColumnName = "CD_HABILIDADEEncoded",
                LabelColumnName = "MEDIA",
                NumberOfIterations = 500,
                ApproximationRank = 10
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
            Debug.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;

        }

        public static void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Debug.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "MEDIA", scoreColumnName: "Score");
            Debug.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Debug.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }
        public static String UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
        {
            Debug.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<UserHabData, UserHabDataPrediction>(model);
            var testInput = new UserHabData { CD_ALUNO = 15, CD_HABILIDADE = 8};

            var movieRatingPrediction = predictionEngine.Predict(testInput);
            Debug.WriteLine("Teste" + Math.Round(movieRatingPrediction.Score, 1));
            if (Math.Round(movieRatingPrediction.Score, 1) > 2.5)
            {
                Debug.WriteLine("Movie " + testInput.CD_HABILIDADE + " is recommended for user " + testInput.CD_ALUNO);
                return ("Movie " + testInput.CD_HABILIDADE + " is recommended for user " + testInput.CD_ALUNO);
            }
            else
            {
                Debug.WriteLine("Movie " + testInput.CD_HABILIDADE + " is not recommended for user " + testInput.CD_ALUNO);
                return("Movie " + testInput.CD_HABILIDADE + " is not recommended for user " + testInput.CD_ALUNO);
            }
        }

    }
}
