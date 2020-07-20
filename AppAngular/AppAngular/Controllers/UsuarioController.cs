using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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

        [HttpGet]
        [Route("api/Usuario/validarUsuario/{idUsuario}/{nombre}")]
        public int validarUsuario(int idUsuario, string nombre)
        {
            int rpta = 0;
            try
            {
                using (BDRestauranteContext bd = new BDRestauranteContext())
                {
                    if (idUsuario==0)
                    {
                        rpta = bd.Usuario.Where(p => p.Nombreusuario.ToLower() == nombre.ToLower()).Count();
                    }
                    else
                    {
                        rpta = bd.Usuario.Where(p => p.Nombreusuario.ToLower() == nombre.ToLower() && p.Iidusuario!=idUsuario).Count();
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
                        if (oUsuarioCLS.iidusuario==0)
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
    }
}