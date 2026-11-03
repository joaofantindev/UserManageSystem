using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManageSystem.Models.DTOs
{
    public static class ProdutoMapperDTO //metodos comuns, criam e devolvem um objeto
    {
        public static ProdutoRespostaDTO ToResponseDTO(this Produto produto) => new() //recebe um Produto e retorna um DTO: filtro do filtro
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Marca = produto.Marca,
            Preco = produto.Preco,
            CompradoEm = produto.CompradoEm
        };
        /*
            AQUI A GENTE PREPARA O PRODUTO PRA SER EXIBIDO
        */

        public static ProdutoUpdateDTO ToUpdateDTO(this ProdutoRespostaDTO produto) => new()
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Marca = produto.Marca,
            Preco = produto.Preco
        };
        /*
            prepara os dados que já existem para preencher o formulário de edição
        */
    }
}