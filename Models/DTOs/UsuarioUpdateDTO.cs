using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteSystem.Models.DTOs
{
    public class UsuarioUpdateDTO
    {
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
    public string Email { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;
    }
}
// FORMULARIO DED ATUALIZACAO DE DADOS | ACTION EDIT -> DADOS NECESSARIOS