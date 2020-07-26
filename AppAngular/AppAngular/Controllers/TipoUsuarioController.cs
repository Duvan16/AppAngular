using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAngular.Clases;
using AppAngular.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    public class TipoUsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/TipoUsuario/listarTipoUsuarios")]
        public List<TipoUsuarioCLS> listarTipoUsuarios()
        {
            List<TipoUsuarioCLS> listaTipoUsuario = new List<TipoUsuarioCLS>();
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                listaTipoUsuario = (from tipoUsuario in bd.TipoUsuario
                                    where tipoUsuario.Bhabilitado == 1
                                    select new TipoUsuarioCLS
                                    {
                                        iidtipousuario = tipoUsuario.Iidtipousuario,
                                        nombre = tipoUsuario.Nombre,
                                        descripcion = tipoUsuario.Descripcion,
                                        bhabilitado = (int)tipoUsuario.Bhabilitado
                                    }).ToList();
                return listaTipoUsuario;
            }
        }

        [HttpGet]
        [Route("api/TipoUsuario/listarPaginasTiposUsuario")]
        public List<PaginaCLS> listarPaginasTiposUsuario()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.Bhabilitado == 1
                               select new PaginaCLS
                               {
                                   iidpagina = pagina.Iidpagina,
                                   mensaje = pagina.Mensaje
                               }).ToList();
            }
            return listaPagina;
        }

        [HttpGet]
        [Route("api/TipoUsuario/listarPaginasRecuperar/{iidTipoUsuario}")]
        public TipoUsuarioCLS listarPaginasRecuperar(int iidTipoUsuario)
        {
            TipoUsuarioCLS oTipoUsuarioCLS = new TipoUsuarioCLS();
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                List<PaginaCLS> listaPaginas = (from tipoUsuario in bd.TipoUsuario
                                                join paginaTipoUsu in bd.PaginaTipoUsuario
                                                on tipoUsuario.Iidtipousuario equals
                                                paginaTipoUsu.Iidtipousuario
                                                join pagina in bd.Pagina
                                                on paginaTipoUsu.Iidpagina equals
                                                pagina.Iidpagina
                                                where paginaTipoUsu.Iidtipousuario == iidTipoUsuario
                                                && paginaTipoUsu.Bhabilitado == 1
                                                select new PaginaCLS
                                                {
                                                    iidpagina = pagina.Iidpagina
                                                }).ToList();

                TipoUsuario oTipoUsuario = bd.TipoUsuario.Where(p => p.Iidtipousuario == iidTipoUsuario).First();

                oTipoUsuarioCLS.iidtipousuario = oTipoUsuario.Iidtipousuario;
                oTipoUsuarioCLS.nombre = oTipoUsuario.Nombre;
                oTipoUsuarioCLS.descripcion = oTipoUsuario.Descripcion;
                oTipoUsuarioCLS.listaPagina = listaPaginas;

                return oTipoUsuarioCLS;
            }
        }
    }
}