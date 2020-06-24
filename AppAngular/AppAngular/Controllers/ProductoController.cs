using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using AppAngular.Clases;
using AppAngular.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Producto/listarProductos")]
        public IEnumerable<ProductoCLS> listarProductos()
        {
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                List<ProductoCLS> lista = (from producto in bd.Producto
                                           join categoria in bd.Categoria
                                           on producto.Iidcategoria equals
                                           categoria.Iidcategoria
                                           select new ProductoCLS
                                           {
                                               idproducto = producto.Iidproducto,
                                               nombre = producto.Nombre,
                                               precio = (decimal)producto.Precio,
                                               stock = (int)producto.Stock,
                                               nombreCategoria = categoria.Nombre
                                           }).ToList();

                return lista;
            }
        }
    }
}