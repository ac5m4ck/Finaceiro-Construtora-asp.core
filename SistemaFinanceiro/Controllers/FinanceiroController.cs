using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaFinanceiro.Models;

namespace SistemaFinanceiro.Controllers
{
    [Authorize]
    public class FinanceiroController : Controller
    {
        private AppDbContext _Context;

        public FinanceiroController(AppDbContext context)
        {
            _Context = context;
        }
        
        // GET: Financeiro
        public ActionResult Index()
        {
            return View(_Context.Financeriros.ToList());
        }

        //Modelo de grids
        public IActionResult IndexGrid(string search)
        {
            return PartialView(string.IsNullOrWhiteSpace(search) ? _Context.Financeriros.AsQueryable() : _Context.Financeriros.Where(c =>
                c.Descricao.Contains(search) || c.Terceiros.Id.ToString().Contains(search) || c.DataCadastro.ToString().Contains(search) || c.Valor.ToString().Contains(search)));

        }


        // GET: Financeiro/Edit/5
        public ActionResult Edit(int? id)
        {
            AjusteContextoEditar();

            if (id > 0)
            {
                return View(_Context.Financeriros.Find(id));
            }
            else
            {
                return View(new Financeriros());
            }
        }

        // POST: Financeiro/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Financeriros terceiros)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (terceiros.Id > 0)
                    {
                        _Context.Entry(terceiros).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _Context.Entry(terceiros).Property(x => x.DataCadastro).IsModified = false;
                        _Context.SaveChanges();

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        terceiros.DataCadastro = DateTime.Now;
                        _Context.Financeriros.Add(terceiros);
                        _Context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    AjusteContextoEditar();
                    return View(terceiros);
                }
               
            }
            catch (Exception ex)
            {
                AjusteContextoEditar();
                return View(terceiros);
            }
        }

      
        public ActionResult Delete(int id)
        {

            var persertir = _Context.Financeriros.Find(id);
            _Context.Financeriros.Remove(persertir);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
           
        }

        private void AjusteContextoEditar()
        {
            var terceiros = _Context.Terceiros.ToList();
            ViewBag.Terceiros = new SelectList(terceiros, "Id", "Nome");
        }
    }
}