using Microsoft.ML.Data;

namespace LumeServer.DTOs.LumeAI
{
    // Essa classe serve para agrupar os itens resultantes do treinamento da IA para gerar o relatório
    public class MovieClusterPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint ClusterId { get; set; }

        public float[] Features { get; set; }
    }
}
