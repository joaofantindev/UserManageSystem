using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManageSystem.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public Decimal Preco { get; set; } //decimal pra dinheiro 
        public DateTime CompradoEm { get; set; } = DateTime.UtcNow;   
    }
}