using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManageSystem.Models.DTOs;

namespace UserManageSystem.Services
{
    public interface IProdutoService
    {
        Task<IReadOnlyList<ProdutoRespostaDTO>> ListarAsync();
        Task<ProdutoRespostaDTO?> ObterPorIdAsync();
        Task<bool> AtualizarAsync(ProdutoUpdateDTO dto);
        Task<bool> RemoverAsyn(int id);
        // bool pra saber se o produto existe ou foi encontrado
    }
}
// interface é o contrato que o controller vai ver, apenas mostra as operações, mas não como funcionam