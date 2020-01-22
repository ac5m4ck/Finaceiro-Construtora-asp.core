using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiro.Models;

namespace SistemaFinanceiro.Controllers
{
    [Authorize]
    public class TerceirosController : Controller
    {
        private AppDbContext _Context;

        public TerceirosController(AppDbContext context)
        {
            _Context = context;
        }
        
        // GET: Terceiros
        public ActionResult Index()
        {
            return View(_Context.Terceiros.ToList());
        }

        //Modelo de grids
        public IActionResult IndexGrid(string search)
        {
            return PartialView(string.IsNullOrWhiteSpace(search) ? _Context.Terceiros.AsQueryable() : _Context.Terceiros.Where(c =>
                c.Nome.Contains(search)  || c.Endereco.Contains(search) || c.Telefone.ToString().Contains(search)));

        }

        // GET: Terceiros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id > 0)
            {
                var terceiros = _Context.Terceiros.Include(c => c.ListaDeFinanceriros).FirstOrDefault(c => c.Id == id);
                return View(terceiros);
            }
            else
            {
                return View(new Terceiros());
            }
        }

        // POST: Terceiros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Terceiros terceiros)
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
                        _Context.Terceiros.Add(terceiros);
                        _Context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(terceiros);
                }
               
            }
            catch (Exception ex)
            {
                return View(terceiros);
            }
        }

      
        public ActionResult Delete(int id)
        {

            var persertir = _Context.Terceiros.Find(id);
            _Context.Terceiros.Remove(persertir);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
           
        }
    }
}