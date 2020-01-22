using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaFinanceiro.Models;

namespace SistemaFinanceiro.Controllers
{
    public class AcessoController : Controller
    {
        private AppDbContext _Context;

        public AcessoController(AppDbContext context)
        {
            _Context = context;
        }

       #region Index, Logging, Logout

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Entrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Entrar(AcessoModel model, string returnUrl)
        {

            var usuario = _Context.Usuario.FirstOrDefault(c => c.Email == model.Email && c.Senha == model.Senha);

            if (usuario == null)
            {
                ViewBag.Messagen = "Usúario ou senha invalido.";
                ViewBag.returUrl = returnUrl;
                return View(model);
            }

            var claims = new List<Claim>
                {
                    new Claim("sub", usuario.Id.ToString()),
                    new Claim("user", usuario.Email),
                    new Claim("name", usuario.Nome),
                    new Claim("email", usuario.Email)
                };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")),
                new AuthenticationProperties
                {
                    IsPersistent = true
                });


            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return Redirect("Index");
            }

        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Entrar");
        }


        #endregion

        #region Usuario

        public ActionResult Usuario()
        {
            return View();
        }

        //Modelo de grids
        public IActionResult UsuarioGrid(string search)
        {

            return PartialView(string.IsNullOrWhiteSpace(search) ? _Context.Usuario.AsQueryable() : _Context.Usuario.Where(c =>
                c.Nome.Contains(search) ||
                c.Email.Contains(search)));
        }

        // GET: Usuario/Edit/5
        public ActionResult UsuarioEdit(int? id)
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
        public ActionResult UsuarioEdit(Usuario usuario)
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

        public ActionResult UsuarioDelete(int id)
        {

            var persertir = _Context.Usuario.Find(id);
            _Context.Usuario.Remove(persertir);
            _Context.SaveChanges();

            return RedirectToAction("Usuario");

        }

        #endregion

        #region Terceiros

        public ActionResult Terceiros()
        {
            return View(_Context.Terceiros.ToList());
        }

        //Modelo de grids
        public IActionResult TerceirosGrid(string search)
        {
            return PartialView(string.IsNullOrWhiteSpace(search) ? _Context.Terceiros.AsQueryable() : _Context.Terceiros.Where(c =>
                c.Nome.Contains(search) || c.Cpf.Contains(search) || c.Endereco.Contains(search) || c.Cpf.Contains(search)));

        }

        // GET: Terceiros/Edit/5
        public ActionResult TerceirosEdit(int? id)
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
        public ActionResult TerceirosEdit(Terceiros terceiros)
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

                        return RedirectToAction("Terceiros");
                    }
                    else
                    {
                        terceiros.DataCadastro = DateTime.Now;
                        _Context.Terceiros.Add(terceiros);
                        _Context.SaveChanges();

                        return RedirectToAction("Terceiros");
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


        public ActionResult TerceirosDelete(int id)
        {

            var persertir = _Context.Terceiros.Find(id);
            _Context.Terceiros.Remove(persertir);
            _Context.SaveChanges();

            return RedirectToAction("Terceiros");

        }

        #endregion

        #region Financeiro

        // GET: Financeiro
        public ActionResult Financeiro()
        {
            return View(_Context.Financeriros.ToList());
        }

        //Modelo de grids
        public IActionResult FinanceiroGrid(string search)
        {
            return PartialView(string.IsNullOrWhiteSpace(search) ? _Context.Financeriros.AsQueryable() : _Context.Financeriros.Where(c =>
                c.Descricao.Contains(search) || c.Terceiros.Id.ToString().Contains(search) || c.DataCadastro.ToString().Contains(search) || c.Valor.ToString().Contains(search)));

        }


        // GET: Financeiro/Edit/5
        public ActionResult FinanceiroEdit(int? id)
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
        public ActionResult FinanceiroEdit(Financeriros terceiros)
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

                        return RedirectToAction("Financeiro");
                    }
                    else
                    {
                        terceiros.DataCadastro = DateTime.Now;
                        _Context.Financeriros.Add(terceiros);
                        _Context.SaveChanges();

                        return RedirectToAction("Financeiro");
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


        public ActionResult FinanceiroDelete(int id)
        {

            var persertir = _Context.Financeriros.Find(id);
            _Context.Financeriros.Remove(persertir);
            _Context.SaveChanges();

            return RedirectToAction("Financeiro");

        }

        private void AjusteContextoEditar()
        {
            var terceiros = _Context.Terceiros.ToList();
            ViewBag.Terceiros = new SelectList(terceiros, "Id", "Nome");
        }

        #endregion
    }
}