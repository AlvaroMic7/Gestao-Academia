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
    public class TreinosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treinos
        // (Continua igual)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Treinos.ToListAsync());
        }

        // GET: Treinos/Details/5
        // ESTA É A TELA "MONTADOR DE TREINO"
    // GET: Treinos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var treino = await _context.Treinos
            .Include(t => t.ItensTreino)
                .ThenInclude(it => it.Exercicio)
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (treino == null) return NotFound();

        // --- A MÁGICA DO AGRUPAMENTO ACONTECE AQUI ---
        // 1. Buscamos os exercícios ordenados primeiro por Grupo, depois por Nome
        var exercicios = await _context.Exercicios
            .OrderBy(e => e.GrupoMuscular)
            .ThenBy(e => e.Nome)
            .ToListAsync();

        // 2. Criamos o SelectList indicando que o campo "GrupoMuscular" é o agrupador (o 4º parâmetro)
        ViewBag.ExercicioId = new SelectList(exercicios, "Id", "Nome", "GrupoMuscular", null);

        return View(treino);
    }

        // GET: Treinos/Create
        // (Continua igual)
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treinos/Create
        // (Modificado para redirecionar para "Details" após criar)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Observacoes")] Treino treino)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treino);
                await _context.SaveChangesAsync();
                
                // Redireciona para o "Montador" (Details)
                return RedirectToAction("Details", new { id = treino.Id });
            }
            return View(treino);
        }

        // --- NOVA AÇÃO ---
        // POST: Treinos/AdicionarItem
        // Esta ação é chamada pelo formulário "Adicionar Exercício" na tela Details
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> AdicionarItem(int TreinoId, int ExercicioId, int Series, string Repeticoes, string TempoDescanso, string? Observacoes)
        {
            var novoItem = new ItemTreino
            {
                TreinoId = TreinoId,
                ExercicioId = ExercicioId,
                Series = Series, // Salvando o novo campo
                Repeticoes = Repeticoes,
                TempoDescanso = TempoDescanso,
                Observacoes = Observacoes
            };

            _context.ItensTreino.Add(novoItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = TreinoId });
        }

        // --- NOVA AÇÃO ---
        // GET: Treinos/RemoverItem/17
        // Chamado quando o usuário clica em "Remover" em um exercício da lista
        public async Task<IActionResult> RemoverItem(int? id) // id aqui é do ItemTreino
        {
            if (id == null)
            {
                return NotFound();
            }

            // 1. Encontra o "ItemTreino" (a linha) que o usuário quer remover
            var itemTreino = await _context.ItensTreino.FindAsync(id);
            
            if (itemTreino == null)
            {
                return NotFound();
            }

            // Guarda o Id do Treino para o qual vamos redirecionar
            int treinoId = itemTreino.TreinoId;

            // 2. Remove o item e salva
            _context.ItensTreino.Remove(itemTreino);
            await _context.SaveChangesAsync();

            // 3. Redireciona de volta para a página "Details"
            return RedirectToAction("Details", new { id = treinoId });
        }


        // GET: Treinos/Delete/5
        // (Continua igual)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treino = await _context.Treinos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treino == null)
            {
                return NotFound();
            }

            return View(treino);
        }

        // POST: Treinos/Delete/5
        // (MODIFICADO para excluir os ItensTreino primeiro!)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 1. Encontra o treino E todos os seus itens
            var treino = await _context.Treinos
                .Include(t => t.ItensTreino)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (treino != null)
            {
                // 2. Remove todos os itens primeiro
                _context.ItensTreino.RemoveRange(treino.ItensTreino);
                
                // 3. Remove o treino
                _context.Treinos.Remove(treino);
                
                // 4. Salva tudo
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}