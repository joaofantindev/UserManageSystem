using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManageSystem.Models.DTOs
{
    public class ProdutoUpdateDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "nome do produto obrigatorio na edicao")]
        [StringLength(100, MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "marca obrigatoria na edicao")]
        [StringLength(100, MinimumLength = 2)]
        public string Marca { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "digite um numero maior que 0")]
        public decimal Preco { get; set; }
    }
}