using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Data;
using Microsoft.EntityFrameworkCore;
using UserManageSystem.Models;
using UserManageSystem.Models.DTOs;

namespace UserManageSystem.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly AppDbContext _banco;
         public ProdutoService(AppDbContext banco)
        {
            _banco = banco;
        }

        //łista por nome
        public async Task<IReadOnlyList<ProdutoRespostaDTO>> ListarAsync()
        {
            var produtos = await _banco.Produtos.AsNoTracking().OrderBy(p => p.Nome).ToListAsync();
            return produtos.Select(p => p.ToResponseDTO()).ToList();
        }

        //pega pelo id
        public async Task<ProdutoRespostaDTO?> ObterPorIdAsync(int id)
        {
            var produto = await _banco.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return produto?.ToResponseDTO();
        }

        //cria
        public async Task<ProdutoRespostaDTO> CriarAsync(ProdutoCreateDTO dto)
        {
            var produto = new Produto
            {
             Nome = dto.Nome.Trim(),
             Marca = dto.Marca.Trim(),
             Preco = dto.Preco   
            };

            _banco.Produtos.Add(produto);
            await _banco.SaveChangesAsync();

            return produto.ToResponseDTO();
        }

        //atualiza
        public async Task<bool> AtualizarAsync(ProdutoUpdateDTO dto)
        {
            var produto = await _banco.Produtos.FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (produto == null)
                return false;
            
            produto.Nome = dto.Nome.Trim();
            produto.Marca = dto.Marca.Trim();
            produto.Preco = dto.Preco;

            await _banco.SaveChangesAsync();
            return true;
        }

        //remove
        public async Task<bool> RemoverAsync(int id)
        {
            var produto = await _banco.Produtos.FirstOrDefaultAsync(p => p.Id == id);

            if (produto == null)
                return false;

            _banco.Produtos.Remove(produto);
            await _banco.SaveChangesAsync();
            return true;
        }
    }
}