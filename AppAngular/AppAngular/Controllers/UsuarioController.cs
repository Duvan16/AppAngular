using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppAngular.Clases;
using AppAngular.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAngular.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/Usuario/listaTipoUsuario")]
        public IEnumerable<TipoUsuarioCLS> listaTipoUsuario()
        {
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                List<TipoUsuarioCLS> listaTipoUsuario = (from tipoUsuario in bd.TipoUsuario
                                                         where tipoUsuario.Bhabilitado == 1
                                                         select new TipoUsuarioCLS
                                                         {
                                                             iidtipousuario = tipoUsuario.Iidtipousuario,
                                                             nombre = tipoUsuario.Nombre
                                                         }).ToList();

                return listaTipoUsuario;
            }
        }

        [HttpGet]
        [Route("api/Usuario/listarUsuario")]
        public IEnumerable<UsuarioCLS> listarUsuario()
        {
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                List<UsuarioCLS> listaUsuario = (from usuario in bd.Usuario
                                                 join persona in bd.Persona
                                                 on usuario.Iidpersona equals persona.Iidpersona
                                                 join tipoUsuario in bd.TipoUsuario
                                                 on usuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                                 where usuario.Bhabilitado == 1
                                                 select new UsuarioCLS
                                                 {
                                                     iidusuario = usuario.Iidusuario,
                                                     nombreusuario = usuario.Nombreusuario,
                                                     nombrePersona = persona.Nombre + " " + persona.Appaterno + " " + persona.Appaterno,
                                                     nombreTipoUsuario = tipoUsuario.Nombre
                                                 }).ToList();

                return listaUsuario;

            }
        }

        [HttpGet]
        [Route("api/Usuario/filtrarUsuarioPorTipo/{idTipo?}")]
        public IEnumerable<UsuarioCLS> filtrarUsuarioPorTipo(int idTipo = 0)
        {
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                List<UsuarioCLS> listaUsuario = (from usuario in bd.Usuario
                                                 join persona in bd.Persona
                                                 on usuario.Iidpersona equals persona.Iidpersona
                                                 join tipoUsuario in bd.TipoUsuario
                                                 on usuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                                                 where usuario.Bhabilitado == 1
                                                 && usuario.Iidtipousuario == idTipo
                                                 select new UsuarioCLS
                                                 {
                                                     iidusuario = usuario.Iidusuario,
                                                     nombreusuario = usuario.Nombreusuario,
                                                     nombrePersona = persona.Nombre + " " + persona.Appaterno + " " + persona.Appaterno,
                                                     nombreTipoUsuario = tipoUsuario.Nombre
                                                 }).ToList();

                return listaUsuario;
            }
        }
    }
}