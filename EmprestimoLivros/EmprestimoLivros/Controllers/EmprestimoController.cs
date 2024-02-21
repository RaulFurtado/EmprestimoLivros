using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimoLivros.Controllers
{
    public class EmprestimoController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EmprestimoController(ApplicationDbContext db)
        {
              _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<EmprestimosModel> emprestimos = _db.Emprestimos;

            return View(emprestimos);
        }

        public IActionResult Editar(int? id)
        {
            if (id is null) return NotFound();

            var emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo is null) return NotFound();

            return View(emprestimo);
        }

        public IActionResult Cadastrar(EmprestimosModel request)
        {
            if (ModelState.IsValid)
            {
                _db.Emprestimos.Add(request);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult EditarEmprestimo(EmprestimosModel request)
        {
            if (ModelState.IsValid)
            {
                _db.Emprestimos.Update(request);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpPost]
        public IActionResult Excluir(EmprestimosModel request)
        {
            if (ModelState.IsValid)
            {
                var emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == request.Id);

                if (emprestimo is null) return NotFound(emprestimo);

                _db.Emprestimos.Remove(emprestimo);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(request);
        }


        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id is null) return NotFound();

            var emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo is null) return NotFound();

            return View(emprestimo);
        }
    }
}
