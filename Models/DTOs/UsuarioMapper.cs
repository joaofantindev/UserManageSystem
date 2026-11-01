using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Data;

namespace CompleteSystem.Models.DTOs;

    public static class UsuarioMapper
    {
        // entidade <-> DTO
        public static UsuarioResponseDTO ToResponseDTO(this Usuario usuario) => new()
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Ativo = usuario.Ativo,
            CriadoEm = usuario.CriadoEm
        };

        public static UsuarioUpdateDTO ToUpdateDTO(this UsuarioResponseDTO usuario) => new()
        {
          Id = usuario.Id,
          Nome = usuario.Nome,
          Email = usuario.Email,
          Ativo = usuario.Ativo
        };
    }
