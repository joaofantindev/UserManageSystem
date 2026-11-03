using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManageSystem.Models.DTOs
{
    public class ProdutoRespostaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public Decimal Preco { get; set; }
        public DateTime CompradoEm { get; set; }
    }
}