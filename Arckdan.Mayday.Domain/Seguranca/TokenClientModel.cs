using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arckdan.Mayday.Domain.Seguranca
{
    public class TokenClientModel
    {
        #region propriedades

        [JsonPropertyName("client_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientId { get; private set; }

        [JsonPropertyName("client_secret")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientSecret { get; private set; }

        [JsonPropertyName("client_role")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ClientRole { get; private set; }

        #endregion

        #region factory

        public static class TokenClientFactory
        {
            /// <summary>
            /// obtém o model com os dados completos
            /// </summary>
            /// <param name="clientId">id do usuário do token</param>
            /// <param name="clientSecret">senha do usuário do token</param>
            /// <param name="clientRole">papel do usuário</param>
            /// <returns>retorna a model com os dados completos</returns>
            public static TokenClientModel ObterModel(string? clientId, string? clientSecret, string? clientRole)
                => new TokenClientModel
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    ClientRole = clientRole,
                };
        }

        #endregion
    }
}
