using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteSystem.Models.DTOs
{
    public class UsuarioCreateDTO
    {
        [Required(ErrorMessage = "o nome é obrigatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "o nome deve ter entre 3 a 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;



        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 8,
            ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}
// FORMULARIO DE CRIACAO DE USUARIO | ACTION CREATE -> DADOS DO USUARIO