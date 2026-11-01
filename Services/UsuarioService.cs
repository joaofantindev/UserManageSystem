using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Data;
using CompleteSystem.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace CompleteSystem.Services
{
   
    public class UsuarioService : IUsuarioService
        {
            private readonly AppDbContext _context;
            private readonly IPasswordHasher<Usuario> _passwordHasher;

            public UsuarioService(
                AppDbContext context,
                IPasswordHasher<Usuario> passwordHasher)
            {
                _context = context;
                _passwordHasher = passwordHasher;
            }

            // LISTA OS USUARIOS ORDENANDO POR NOME, TUDO DE FORMA ASSINCRONA
            public async Task<IReadOnlyList<UsuarioResponseDTO>> ListarAsync()
            {
                var usuarios = await _context.Usuarios
                    .AsNoTracking() // ninguem vai alterar esses objetos - mais leve pra leitura
                    .OrderBy(u => u.Nome)
                    .ToListAsync();

                return usuarios.Select(u => u.ToResponseDTO()).ToList();
            }


            //PEGA POR ID
            public async Task<UsuarioResponseDTO?> ObterPorIdAsync(int id)
            {
                var usuario = await _context.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id);

                return usuario?.ToResponseDTO();
            }

            // CRIA PASSANDO O DTO DE CRIACAO PARA DADOS NECESSARIOS, REMOVE ESPAÇOSS E DEIXA FORMATADO

            public async Task<UsuarioResponseDTO> CriarAsync(UsuarioCreateDTO dto)
            {
                var email = dto.Email.Trim().ToLowerInvariant();

                var emailEmUso = await _context.Usuarios
                    .AnyAsync(u => u.Email == email);

                if (emailEmUso)
                    throw new RegraDeNegocioException(
                        "Já existe um usuário com este e-mail.");

                var usuario = new Usuario
                {
                    Nome = dto.Nome.Trim(),
                    Email = email,
                    Ativo = true,
                    CriadoEm = DateTime.UtcNow
                };

                usuario.SenhaHash = _passwordHasher.HashPassword(usuario, dto.Senha);

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                return usuario.ToResponseDTO();
            }

            //ATUALIZA ELES
            public async Task<bool> AtualizarAsync(UsuarioUpdateDTO dto)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == dto.Id);

                if (usuario is null)
                    return false;

                var email = dto.Email.Trim().ToLowerInvariant();

                var emailEmUso = await _context.Usuarios
                    .AnyAsync(u => u.Email == email && u.Id != dto.Id);

                if (emailEmUso)
                    throw new RegraDeNegocioException(
                        "Já existe outro usuário com este e-mail.");

                usuario.Nome = dto.Nome.Trim();
                usuario.Email = email;
                usuario.Ativo = dto.Ativo;

                await _context.SaveChangesAsync();
                return true;
            }


            // deleta por id
            public async Task<bool> RemoverAsync(int id)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario is null)
                    return false;

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return true;
            }
        }
}