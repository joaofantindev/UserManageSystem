using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CompleteSystem.Models.DTOs;
using CompleteSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompleteSystem.Controllers;

[Route("[controller]")]
public class UsuarioController : Controller
{
    private readonly IUsuarioService _usuarioService;
    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioService.ListAsync();
        return View(usuarios);
    }

      public async Task<IActionResult> Details(int id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        if (usuario is null)
            return NotFound();

        return View(usuario);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            await _usuarioService.CriarAsync(dto);
            TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (RegraDeNegocioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        if (usuario is null)
            return NotFound();

        return View(usuario.ToUpdateDTO());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UsuarioUpdateDTO dto)
    {
        if (id != dto.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(dto);

        try
        {
            var atualizado = await _usuarioService.AtualizarAsync(dto);
            if (!atualizado)
                return NotFound();

            TempData["Sucesso"] = "Usuário atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (RegraDeNegocioException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _usuarioService.ObterPorIdAsync(id);
        if (usuario is null)
            return NotFound();

        return View(usuario);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var removido = await _usuarioService.RemoverAsync(id);
        if (!removido)
            return NotFound();

        TempData["Sucesso"] = "Usuário removido.";
        return RedirectToAction(nameof(Index));
    }
}
