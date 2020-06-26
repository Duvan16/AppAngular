using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAngular.Clases;
using AppAngular.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    public class CategoriaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Categoria/listarCategorias")]
        public IEnumerable<CategoriaCLS> listarCategorias()
        {
            using (var bd = new BDRestauranteContext())
            {
                IEnumerable<CategoriaCLS> listaCategoria = (from categoria in bd.Categoria
                                                            where categoria.Bhabilitado == 1
                                                            select new CategoriaCLS
                                                            {
                                                                idcategoria = categoria.Iidcategoria,
                                                                nombre = categoria.Nombre
                                                            }).ToList();

                return listaCategoria;
            }
        }
    }
}