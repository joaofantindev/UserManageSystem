using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserManageSystem.Models.DTOs
{
    public class ProdutoCreateDTO
    {
        [Required(ErrorMessage = "o nome é obrigatorio! ")]
        [StringLength(100, MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;

       
        [Required(ErrorMessage = "marca é obrigatoria!!!")]
        [StringLength(100, MinimumLength = 1)]
        public string Marca { get; set; } = string.Empty;
       

        [Range(typeof(decimal), "0.01", "999999999.99", ErrorMessage = "informa um preço maior que 0")]
        public decimal Preco { get; set; }
    }
}