using System.Text.Json.Serialization;

namespace Arckdan.Mayday.Domain.Seguranca
{
    public class TokenBearerModel
    {
        #region propridades

        [JsonPropertyName("access_token")]
        public string AccessToken { get; private set; }

        [JsonPropertyName("token_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? TokenType { get; private set; }

        [JsonPropertyName("expires_in")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ExpiresIn { get; private set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; private set; }

        #endregion

        #region factory

        public static class TokenModelFactory
        {
                /// <summary>
                /// obtém o model com os dados completos
                /// </summary>
                /// <param name="accessToken">token de acesso</param>
                /// <param name="tokenType">tipo do token</param>
                /// <param name="expiresIn">timestamp de expiração do token</param>
                /// <param name="refreshToken">token regerado</param>
                /// <returns>retorna a model com os dados completos</returns>
                public static TokenBearerModel ObterModel(string accessToken, string? tokenType, int? expiresIn, string refreshToken)
                    => new TokenBearerModel
                    {
                        AccessToken = accessToken,
                        TokenType = tokenType,
                        ExpiresIn = expiresIn,
                        RefreshToken = refreshToken
                    };
        }

        #endregion
    }
}
