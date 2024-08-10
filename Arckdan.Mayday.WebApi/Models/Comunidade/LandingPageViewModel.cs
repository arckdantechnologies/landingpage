using System.Text.Json.Serialization;

namespace Arckdan.Mayday.WebApi.Models.Comunidade
{
    public class LandingPageViewModel
    {
        #region propriedades

        [JsonPropertyName("id")]
        public Guid? Id { get; set; }

        [JsonPropertyName("ip")]
        public string? IP { get; set; }

        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("uf")]
        public string? UF { get; set; }

        [JsonPropertyName("cidade")]
        public string? Cidade { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("whatsApp")]
        public long? WhatsApp { get; set; }

        [JsonPropertyName("tokenSeguranca")]
        public string? Token { get; set; }

        [JsonPropertyName("dataInclusao")]
        public DateTime Inclusao { get; set; }

        [JsonPropertyName("dataAlteracao")]
        public DateTime? Alteracao { get; set; }

        #endregion
    }
}