using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Data;
using Microsoft.AspNetCore.Identity;

namespace CompleteSystem.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _contexto;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        // 
    }
}