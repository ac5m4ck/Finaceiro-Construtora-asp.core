using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiro.Models;

namespace SistemaFinanceiro.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private AppDbContext _Context;

        public UsuarioController(AppDbContext context)
        {
            _Context = context;
        }
        
        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        //Modelo de grids
        public IActionResult IndexGrid(string search)
        {

            return PartialView(string.IsNullOrWhiteSpace(search) ? _Context.Usuario.AsQueryable() : _Context.Usuario.Where(c =>
                c.Nome.Contains(search) ||
                c.Email.Contains(search)));
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id > 0)
            {
                return View(_Context.Usuario.Find(id));
            }
            else
            {
                return View(new Usuario());
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Usuario usuario)
        {  
            try
            {
                if (ModelState.IsValid)
                {
                    if (usuario.Id > 0)
                    {
                        var persertir = _Context.Usuario.Find(usuario.Id);

                        persertir.Nome = usuario.Nome;
                        persertir.Email = usuario.Email;
                        persertir.Senha = usuario.Senha;

                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var validacao = _Context.Usuario.FirstOrDefault(c => c.Email == usuario.Email);

                        if (validacao != null)
                        {
                            ViewBag.Messagen = "Não foi possivel cadastrar Email, já é cadastrado na nossa base";                           
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            _Context.Usuario.Add(usuario);
                            _Context.SaveChanges();
                        }                     

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View(usuario);
                }
               
            }
            catch
            {
                return View(usuario);
            }
        }
      
        public ActionResult Delete(int id)
        {

            var persertir = _Context.Usuario.Find(id);
            _Context.Usuario.Remove(persertir);
            _Context.SaveChanges();

            return RedirectToAction(nameof(Index));
           
        }
    }
}