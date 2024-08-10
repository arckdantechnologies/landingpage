using System.Text.Json.Serialization;

namespace Arckdan.Mayday.WebApi.Models.Seguranca
{
    public class TokenViewModel
    {
        #region propriedades

        [JsonPropertyName("client_id")]
        public string? Client_Id { get; set; }

        [JsonPropertyName("client_secret")]
        public string? Client_Secret { get; set; }

        #endregion
    }
}
