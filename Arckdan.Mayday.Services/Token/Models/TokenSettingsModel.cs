using Microsoft.Extensions.Configuration;

namespace Arckdan.Mayday.Services.Token.Models
{
    public class TokenSettingsModel
    {
        #region atributos

        readonly IConfiguration _configuration;

        #endregion

        #region construtores

        /// <summary>
        /// construtor da classe TokenSettingsModel
        /// </summary>
        /// <param name="configuration">configurações para acesso aos dados de geração do token JWT</param>
        public TokenSettingsModel(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        #endregion

        #region propriedades

        public string? Issuer => _configuration.GetSection("Jwt:Issuer").Value;

        public string? Audience => _configuration.GetSection("Jwt:Audience").Value;

        public string? Key => _configuration.GetSection("Jwt:Key").Value;

        public string? Client_Id => _configuration.GetSection("Jwt:Client_Id").Value;

        public string? Client_Secret => _configuration.GetSection("Jwt:Client_Secret").Value;

        public string? Client_Role => _configuration.GetSection("Jwt:Client_Role").Value;

        #endregion
    }
}