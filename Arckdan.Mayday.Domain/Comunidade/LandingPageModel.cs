using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Arckdan.Mayday.Domain.Comunidade
{
    public class LandingPageModel
    {
        #region construtores
        
        /// <summary>
        /// construtor da classe LandingPageModel
        /// </summary>
        public LandingPageModel() { }

        #endregion

        #region propriedades

        [Required]
        [Key]
        [Column(TypeName = "char")]
        [StringLength(36)]
        [JsonPropertyName("id")]
        public Guid? Id { get; private set; }

        [Required]
        [Column(TypeName = "Varchar")]
        [StringLength(15)]
        [JsonPropertyName("ip")]
        public string? IP { get; private set; }

        [Required(ErrorMessage = "nome")]
        [Column(TypeName = "Varchar")]
        [StringLength(100)]
        [MaxLength(100, ErrorMessage = "nomeTamanho")]
        [JsonPropertyName("nome")]
        public string? Nome { get; private set; }

        [Required(ErrorMessage = "uf")]
        [Column(TypeName = "Char")]
        [StringLength(2)]
        [JsonPropertyName("uf")]
        public string? UF { get; private set; }

        [Required(ErrorMessage = "cidae")]
        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        [JsonPropertyName("cidade")]
        public string? Cidade { get; private set; }

        [Required(ErrorMessage = "email")]
        [Column(TypeName = "Varchar")]
        [StringLength(150)]
        [JsonPropertyName("email")]
        public string? Email { get; private set; }

        [Required(ErrorMessage = "whatsapp")]
        [Column(TypeName = "bigint")]
        [JsonPropertyName("whatsApp")]
        public long? WhatsApp { get; private set; }

        [Required(ErrorMessage = "tokenSeguranca")]
        [Column(TypeName = "varchar")]
        [StringLength(200)]
        [JsonPropertyName("tokenSeguranca")]
        public string Token { get; private set; }

        [Required(ErrorMessage = "inclusao")]
        [Column(TypeName = "Datetime")]
        [JsonPropertyName("dataInclusao")]
        public DateTime Inclusao { get; private set; }

        [Column(TypeName = "Datetime")]
        [JsonPropertyName("dataAlteracao")]
        public DateTime? Alteracao { get; private set; }

        #endregion

        #region factory

        public static class LandingPageFactory
        {
            /// <summary>
            /// obtém o model com os dados completos
            /// </summary>
            /// <param name="id">id do registro</param>
            /// <param name="ip">ip do registro da page view</param>
            /// <param name="nome">nome do early adopter</param>
            /// <param name="uf">estado aonde reside o early adopter</param>
            /// <param name="cidade">cidadd aond resite o early adopter</param>
            /// <param name="email">e-mail de comunicação com o early adopter</param>
            /// <param name="whatsApp">whatsapp do early adopter</param>
            /// <param name="token">token de acesso aos dados do early adopter</param>
            /// <param name="inclusao">data de inclusão do registro</param>
            /// <param name="alteracao">data de alteração do registro</param>
            /// <returns>retorna a model com os dados completos</returns>
            public static LandingPageModel ObterModel(
                Guid? id,
                string? ip,
                string? nome,
                string? uf,
                string? cidade,
                string? email,
                long? whatsApp,
                string token,
                DateTime inclusao,
                DateTime? alteracao
                ) => new LandingPageModel
                {
                    Id = id,
                    IP = ip,
                    Nome = nome,
                    UF = uf,
                    Cidade = cidade,
                    Email = email,
                    WhatsApp = whatsApp,
                    Token = token,
                    Inclusao = inclusao,
                    Alteracao = alteracao
                };

            /// <summary>
            /// obtém o model com os dados completos
            /// </summary>
            /// <param name="id">id do registro</param>
            /// <returns>retorna a model com os dados completos</returns>
            public static LandingPageModel ObterModel(Guid? id) => new LandingPageModel { Id = id };

            /// <summary>
            /// obtém o model com os dados completos
            /// </summary>
            /// <param name="token">id do registro</param>
            /// <returns>retorna a model com os dados completos</returns>
            public static void ObterModel(string token, LandingPageModel p)
            {
                p.Token = token;
            }
        }

        #endregion
    }
}
