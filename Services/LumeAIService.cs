using LumeServer.Data;
using LumeServer.DTOs.LumeAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace LumeServer.Services
{

    // Essa é a classe LumeAIService, ela é responsável por lidar com a lógica de negócio relacionada ao Lume AI.
    // O que envolve importar o modelo zipado, vetorizar as alternativas e filmes que o usuário escolheu, comparar os vetores, retornar os filmes com base nos parâmetros, etc.
    public class LumeAIService
    {
        private readonly LumeDataContext _context;
        private static readonly string _modelPath = "C:\\dev\\ASPNET Core\\Lume\\LumeServer\\Resources\\modeloTreinado.zip";
        private static ITransformer _trainedModel;
        private static MLContext _mlContext;
        private static PredictionEngine<MovieData, MovieClusterPrediction> _predictionEngine;

        public LumeAIService(LumeDataContext context)
        {
            _context = context;
        }

        // Método auxiliar que inicializa o modelo
        public static void LoadModel()
        {
            _mlContext = new MLContext();
            using var fileStream = new FileStream(_modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            _trainedModel = _mlContext.Model.Load(fileStream, out _);
        }

        public async Task<List<ClusterDistanceDTO>> GetClosestClusters(MovieData input, int topN = 5)
        {
            if (_trainedModel == null)
                LoadModel();

            var clusters = await _context.Clusters.ToListAsync();

            var inputData = _mlContext.Data.LoadFromEnumerable(new[] { input });

            var transformed = _trainedModel.Transform(inputData);

            var prediction = _mlContext.Data
                .CreateEnumerable<MovieClusterPrediction>(transformed, reuseRowObject: false)
                .First();

            var inputFeatures = prediction.Features;

            var nearestClusters = clusters
                .Select(c => new ClusterDistanceDTO
                {
                    Id = c.Id,
                    Distance = EuclideanDistance(inputFeatures, c.CentroidVector)
                })
                .OrderBy(x => x.Distance)
                .Take(topN)
                .ToList();

            return nearestClusters;
        }


        private float EuclideanDistance(float[] a, float[] b)
        {
            if (a.Length != b.Length)
                throw new InvalidOperationException("Vetores de tamanhos diferentes.");

            float sum = 0f;
            for (int i = 0; i < a.Length; i++)
                sum += (a[i] - b[i]) * (a[i] - b[i]);

            return MathF.Sqrt(sum);
        }
    }
}
