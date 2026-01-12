using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAcademia.Models;

namespace SistemaAcademia.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Registros (Histórico de Treinos)
        public async Task<IActionResult> Index()
        {
            var historico = await _context.RegistrosTreino
                .Include(r => r.Treino) // Inclui o nome da ficha (Ex: Treino A)
                .OrderByDescending(r => r.DataTreino) // Mostra os mais recentes primeiro
                .ToListAsync();
            return View(historico);
        }

        // GET: Registros/Create (Tela para escolher qual treino iniciar)
        public IActionResult Create()
        {
            // Carrega a lista de fichas para o dropdown
            ViewBag.TreinoId = new SelectList(_context.Treinos, "Id", "Nome");
            return View();
        }

        // POST: Registros/Create (A MÁGICA ACONTECE AQUI)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TreinoId,DataTreino,Observacoes")] RegistroTreino registro)
        {
            if (ModelState.IsValid)
            {
                // 1. Salva o cabeçalho do diário (Data e qual Treino foi)
                _context.Add(registro);
                await _context.SaveChangesAsync();

                // 2. A MÁGICA: Copia os exercícios da Ficha para o Diário
                
                // Pega os itens da ficha original
                var itensDaFicha = await _context.ItensTreino
                    .Where(i => i.TreinoId == registro.TreinoId)
                    .ToListAsync();

                // Para cada exercício na ficha, cria uma linha no diário
                foreach (var item in itensDaFicha)
                {
                    var novoItemDiario = new RegistroItem
                    {
                        RegistroTreinoId = registro.Id,
                        ExercicioId = item.ExercicioId,
                        Carga = "0", // Começa com zero para o usuário preencher
                        SeriesRealizadas = item.Repeticoes, // Sugere fazer o que está na ficha
                        RepeticoesRealizadas = item.Repeticoes
                    };
                    _context.Add(novoItemDiario);
                }
                
                // Salva todos os itens copiados
                await _context.SaveChangesAsync();

                // 3. Redireciona para a tela de preencher as cargas
                return RedirectToAction(nameof(Details), new { id = registro.Id });
            }
            
            ViewBag.TreinoId = new SelectList(_context.Treinos, "Id", "Nome", registro.TreinoId);
            return View(registro);
        }

        // GET: Registros/Details/5 (A TELA DE PREENCHER CARGAS)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var registro = await _context.RegistrosTreino
                .Include(r => r.Treino)
                .Include(r => r.ItensRegistrados)
                    .ThenInclude(ir => ir.Exercicio) // Precisamos do nome do exercício
                .FirstOrDefaultAsync(m => m.Id == id);

            if (registro == null) return NotFound();

            return View(registro);
        }

        // POST: Registros/AtualizarCarga (Salva o peso de um exercício específico)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AtualizarItem(int ItemId, string Carga, string Series, string Repeticoes)
        {
            var item = await _context.RegistrosItens.FindAsync(ItemId);
            if (item == null) return NotFound();

            // Atualiza os dados
            item.Carga = Carga;
            item.SeriesRealizadas = Series;
            item.RepeticoesRealizadas = Repeticoes;

            _context.Update(item);
            await _context.SaveChangesAsync();

            // Volta para a tela do diário
            return RedirectToAction(nameof(Details), new { id = item.RegistroTreinoId });
        }
        
        // Ações de Delete (Padrão)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var registro = await _context.RegistrosTreino
                .Include(r => r.Treino)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (registro == null) return NotFound();

            return View(registro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registro = await _context.RegistrosTreino
                .Include(r => r.ItensRegistrados) // Inclui os itens para apagar junto
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registro != null)
            {
                // Remove os itens do diário primeiro
                _context.RegistrosItens.RemoveRange(registro.ItensRegistrados);
                // Remove o cabeçalho
                _context.RegistrosTreino.Remove(registro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}