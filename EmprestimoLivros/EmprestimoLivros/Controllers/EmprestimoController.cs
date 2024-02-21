using ClosedXML.Excel;
using EmprestimoLivros.Data;
using EmprestimoLivros.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso!";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult EditarEmprestimo(EmprestimosModel request)
        {
            if (ModelState.IsValid)
            {
                var emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == request.Id);

                if (emprestimo is null) return NotFound(request.Id);

                emprestimo.Recebedor = request.Recebedor;
                emprestimo.Fornecedor = request.Fornecedor;
                emprestimo.LivroEmprestado = request.LivroEmprestado;

                _db.Emprestimos.Update(emprestimo);
                _db.SaveChanges();

                TempData["MensagemSucesso"] = "Edição realizada com sucesso!";

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

                TempData["MensagemSucesso"] = "Exclusão realizada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(request);
        }

        public IActionResult Exportar()
        {
            var tabela = CadastrarDadosNaTabela();

            using (XLWorkbook workBook = new XLWorkbook())
            {
                workBook.AddWorksheet(tabela, "Dados Emprestimo");

                using (MemoryStream ms = new MemoryStream())
                {
                    workBook.SaveAs(ms);

                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spredsheetml.sheet", "Emprestimo.xls");
                }
            }
        }




        [HttpGet]
        public IActionResult Excluir(int? id)
        {
            if (id is null) return NotFound();

            var emprestimo = _db.Emprestimos.FirstOrDefault(x => x.Id == id);

            if (emprestimo is null) return NotFound();

            return View(emprestimo);
        }


        private DataTable CadastrarDadosNaTabela()
        {
            var tabela = new DataTable();

            tabela.TableName = "Dados do Emprestimo";

            tabela.Columns.Add("Recebedor", typeof(string));
            tabela.Columns.Add("Fornecedor", typeof(string));
            tabela.Columns.Add("Livro", typeof(string));
            tabela.Columns.Add("DataEmprestimo", typeof(DateTime));

            var dados = _db.Emprestimos.ToList();

            if (dados.Count > 0)
            {
                dados.ForEach(emprestimo =>
                {
                    tabela.Rows.Add(
                        emprestimo.Recebedor,
                        emprestimo.Fornecedor,
                        emprestimo.LivroEmprestado,
                        emprestimo.DataUltimaAtualizacao);
                });
            }


            return tabela;
        }
    }
}
