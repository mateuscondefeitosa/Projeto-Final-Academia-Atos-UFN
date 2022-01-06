using System.ComponentModel.DataAnnotations;

namespace AutenticacaoMVCCookies.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "O login deve ser informado")]
        public string Login { get; set; }

        [Required(ErrorMessage = "É necessário inserir uma senha")]
        public string Senha { get; set; }
    }
}
