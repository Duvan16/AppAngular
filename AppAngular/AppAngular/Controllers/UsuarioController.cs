using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AppAngular.Clases;
using AppAngular.Models;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Route("api/Usuario/validarUsuario/{idUsuario}/{nombre}")]
        public int validarUsuario(int idUsuario, string nombre)
        {
            int rpta = 0;
            try
            {
                using (BDRestauranteContext bd = new BDRestauranteContext())
                {
                    if (idUsuario == 0)
                    {
                        rpta = bd.Usuario.Where(p => p.Nombreusuario.ToLower() == nombre.ToLower()).Count();
                    }
                    else
                    {
                        rpta = bd.Usuario.Where(p => p.Nombreusuario.ToLower() == nombre.ToLower() && p.Iidusuario != idUsuario).Count();
                    }
                }
            }
            catch (Exception)
            {
                rpta = 0;
            }
            return rpta;
        }

        [HttpGet]
        [Route("api/Usuario/recuperarUsuario/{iidUsuario}")]
        public UsuarioCLS recuperarUsuario(int iidUsuario)
        {
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                UsuarioCLS oUsuarioCLS = new UsuarioCLS();
                Usuario oUsuario = bd.Usuario.Where(p => p.Iidusuario == iidUsuario).First();

                oUsuarioCLS.iidusuario = oUsuario.Iidusuario;
                oUsuarioCLS.nombreusuario = oUsuario.Nombreusuario;
                oUsuarioCLS.iidTipousuario = (int)oUsuario.Iidtipousuario;

                return oUsuarioCLS;
            }
        }

        [HttpPost]
        [Route("api/Usuario/guardarDatos")]
        public int guardarDatos([FromBody]UsuarioCLS oUsuarioCLS)
        {
            int rpta = 0;
            try
            {
                using (BDRestauranteContext bd = new BDRestauranteContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oUsuarioCLS.iidusuario == 0)
                        {
                            // Agregar usuario
                            Usuario oUsuario = new Usuario();
                            oUsuario.Nombreusuario = oUsuarioCLS.nombreusuario;
                            //Cifrar Contraseña
                            SHA256Managed sha = new SHA256Managed();
                            string clave = oUsuarioCLS.contra;
                            byte[] dataNoCifrada = Encoding.Default.GetBytes(clave);
                            byte[] dataCifrada = sha.ComputeHash(dataNoCifrada);
                            string claveCifrada = BitConverter.ToString(dataCifrada).Replace("-", "");
                            oUsuario.Contra = claveCifrada;
                            oUsuario.Iidpersona = oUsuarioCLS.iidPersona;
                            oUsuario.Iidtipousuario = oUsuarioCLS.iidTipousuario;
                            oUsuario.Bhabilitado = 1;
                            bd.Usuario.Add(oUsuario);

                            //Modificar Persona(btieneUsuario de 0 a 1)

                            Persona oPersona = bd.Persona.Where(p => p.Iidpersona == oUsuarioCLS.iidPersona).First();
                            oPersona.Btieneusuario = 1;
                            bd.SaveChanges();
                            transaccion.Complete();
                            rpta = 1;

                        }
                        else
                        {
                            //Editar
                            Usuario oUsuario = bd.Usuario.Where(p => p.Iidusuario == oUsuarioCLS.iidusuario).First();
                            oUsuario.Nombreusuario = oUsuarioCLS.nombreusuario;
                            oUsuario.Iidtipousuario = oUsuarioCLS.iidTipousuario;
                            bd.SaveChanges();
                            transaccion.Complete();
                            rpta = 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                rpta = 0;
            }
            return rpta;
        }

        [HttpGet]
        [Route("api/Usuario/eliminarUsuario/{idUsuario}")]
        public int eliminarUsuario(int idUsuario)
        {
            int rpta = 0;
            try
            {
                using (BDRestauranteContext bd = new BDRestauranteContext())
                {
                    Usuario oUsuario = bd.Usuario.Where(p => p.Iidpersona == idUsuario).First();
                    oUsuario.Bhabilitado = 0;
                    bd.SaveChanges();
                    rpta = 1;
                }
            }
            catch (Exception)
            {
                rpta = 0;
            }
            return rpta;
        }

        [HttpPost]
        [Route("api/Usuario/login/")]
        public UsuarioCLS login([FromBody]UsuarioCLS oUsuarioCLS)
        {
            int rpta = 0;
            UsuarioCLS oUsuario = new UsuarioCLS();

            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                SHA256Managed sha = new SHA256Managed();
                byte[] dataNoCifrada = Encoding.Default.GetBytes(oUsuarioCLS.contra);
                byte[] dataCifrada = sha.ComputeHash(dataNoCifrada);
                string claveCifrada = BitConverter.ToString(dataCifrada).Replace("-", "");
                rpta = bd.Usuario.Where(p => p.Nombreusuario.ToUpper() == oUsuarioCLS.nombreusuario.ToUpper() && p.Contra == claveCifrada).Count();
                if (rpta == 1)
                {
                    Usuario oUsuarioRecuperar = bd.Usuario.Where(p => p.Nombreusuario.ToUpper() == oUsuarioCLS.nombreusuario.ToUpper() && p.Contra == claveCifrada).First();
                    HttpContext.Session.SetString("usuario", oUsuarioRecuperar.Iidusuario.ToString());
                    HttpContext.Session.SetString("tipoUsuario", oUsuarioRecuperar.Iidtipousuario.ToString());
                    oUsuario.iidusuario = oUsuarioRecuperar.Iidusuario;
                    oUsuario.nombreusuario = oUsuarioRecuperar.Nombreusuario;
                }
                else
                {
                    oUsuario.iidusuario = 0;
                    oUsuario.nombreusuario = "";
                }
            }
            return oUsuario;
        }

        [HttpGet]
        [Route("api/Usuario/obtenerVariableSession")]
        public SeguridadCLS obtenerVariableSession()
        {
            SeguridadCLS oSeguridadCLS = new SeguridadCLS();
            string variableSession = HttpContext.Session.GetString("usuario");
            if (variableSession is null)
            {
                oSeguridadCLS.valor = "";
            }
            else
            {
                oSeguridadCLS.valor = variableSession;
                List<PaginaCLS> listaPaginas = new List<PaginaCLS>();
                int idUsuario = int.Parse(HttpContext.Session.GetString("usuario"));
                int idTipoUsuario = int.Parse(HttpContext.Session.GetString("tipoUsuario"));
                using (BDRestauranteContext bd = new BDRestauranteContext())
                {
                    listaPaginas = (from usuario in bd.Usuario
                                    join tipoUsuario in bd.TipoUsuario
                                    on usuario.Iidtipousuario equals
                                    tipoUsuario.Iidtipousuario
                                    join paginaTipo in bd.PaginaTipoUsuario
                                    on usuario.Iidtipousuario equals paginaTipo.Iidtipousuario
                                    join pagina in bd.Pagina
                                    on paginaTipo.Iidpagina equals pagina.Iidpagina
                                    where usuario.Iidusuario == idUsuario
                                    && usuario.Iidtipousuario == idTipoUsuario
                                    && paginaTipo.Bhabilitado == 1
                                    select new PaginaCLS
                                    {
                                        accion = pagina.Accion.Substring(1)
                                    }).ToList();

                    oSeguridadCLS.lista = listaPaginas;
                }
            }
            return oSeguridadCLS;
        }

        [HttpGet]
        [Route("api/Usuario/cerrarSesion")]
        public SeguridadCLS cerrarSesion()
        {
            SeguridadCLS oSeguridadCls = new SeguridadCLS();
            try
            {
                HttpContext.Session.Remove("usuario");
                HttpContext.Session.Remove("tipoUsuario");
                oSeguridadCls.valor = "OK";
            }
            catch (Exception)
            {
                oSeguridadCls.valor = "";
            }
            return oSeguridadCls;
        }

        [HttpGet]
        [Route("api/Usuario/listarPaginas")]
        public List<PaginaCLS> listarPaginas()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();
            int idTipoUsuario = int.Parse(HttpContext.Session.GetString("tipoUsuario"));
            using (BDRestauranteContext bd = new BDRestauranteContext())
            {
                listaPagina = (from paginaTipo in bd.PaginaTipoUsuario
                               join pagina in bd.Pagina
                               on paginaTipo.Iidpagina equals pagina.Iidpagina
                               where paginaTipo.Bhabilitado == 1
                               && paginaTipo.Iidtipousuario == idTipoUsuario
                               select new PaginaCLS
                               {
                                   iidpagina = pagina.Iidpagina,
                                   accion = pagina.Accion,
                                   mensaje = pagina.Mensaje,
                                   bhabilitado = (int)pagina.Bhabilitado
                               }).ToList();
                return listaPagina;
            }
        }
    }
}