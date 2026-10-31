using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Models.DTOs;

namespace CompleteSystem.Services
{
    public interface IUsuarioService
    {
        Task<IReadOnlyList<UsuarioResponseDTO>> ListAsync(); // I Read Only List = eu só leio listas, não modifico
        Task<UsuarioResponseDTO?> ObterPorIdAsync(int id); //
        Task<UsuarioResponseDTO> CriarAsync(UsuarioCreateDTO dto); //
        Task<bool> AtualizarAsync(UsuarioUpdateDTO dto); //
        Task<bool> RemoverAsync(int id); //
    }
}
// fazer a abstração para o controller não depender da classe completa
/*
TASK<> = lida com o tempo, QUANDO a tarefa(task) vai chegar, por isso é sempre assincrona (async), usando AWAIT tambem para 
operações de busca e escrita no banco de dados.

LIST<> = lida com armazenamento, QUANTOS dados existem
*/
