using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompleteSystem.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty; // o service transforma em hash
        public bool Ativo { get; set; } = true;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}