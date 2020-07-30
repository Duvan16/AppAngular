import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';
import { Router } from '@angular/router';

@Injectable()
export class UsuarioService {

  urlBase: string = "";
  constructor(private http: Http, @Inject("BASE_URL") url: string, private router: Router) {
    this.urlBase = url;
  }

  public getUsuario() {
    return this.http.get(this.urlBase + "api/Usuario/listarUsuario")
      .map(res => res.json());
  }

  public getTipoUsuario() {
    return this.http.get(this.urlBase + "api/Usuario/listaTipoUsuario")
      .map(res => res.json());
  }
  public getFiltrarUsuarioPorTipo(idTipo) {
    return this.http.get(this.urlBase + "api/Usuario/filtrarUsuarioPorTipo/" + idTipo)
      .map(res => res.json());
  }
  public validarUsuario(idUsuario, nombre) {
    return this.http.get(this.urlBase + "api/Usuario/validarUsuario/" + idUsuario + "/" + nombre)
      .map(res => res.json());
  }
  public recuperarUsuario(idUsuario) {
    return this.http.get(this.urlBase + "api/Usuario/recuperarUsuario/" + idUsuario)
      .map(res => res.json());
  }

  public guardarDatos(usuarioClS) {
    return this.http.post(this.urlBase + "api/Usuario/guardarDatos", usuarioClS).map(res => res.json())
  }

  public eliminarUsuario(idUsuario) {
    return this.http.get(this.urlBase + "api/Usuario/eliminarUsuario/" + idUsuario).map(res => res.json())
  }

  public login(usuario) {
    return this.http.post(this.urlBase + "api/Usuario/login/", usuario).map(res => res.json());
  }

  public obtenerVariableSession(next) {
    return this.http.get("api/Usuario/obtenerVariableSession").map(res => {
      var data = res.json();
      var inf = data.valor;
      if (inf == "") {
        this.router.navigate(["/pagina-error"])
        return false;
      } else {
        var pagina = next["url"][0].path;
        if (data.lista != null) {
          var paginas = data.lista.map(pagina => pagina.accion);
          if (paginas.indexOf(pagina) > -1 && pagina != "Login") {
            return true;
          } else {
            this.router.navigate(["/pagina-error-permiso"]);
            return false;
          }
        }
        return true;
      }
    });
  }

  public obtenerSession() {
    return this.http.get("api/Usuario/obtenerVariableSession").map(res => {
      var data = res.json();
      var inf = data.valor;
      if (inf == "") {
        return false;
      } else {
        return true;
      }
    });
  }

  public cerrarSesion() {
    return this.http.get("api/Usuario/cerrarSesion").map(res => res.json());
  }

  public listarPaginas() {
    return this.http.get("api/Usuario/listarPaginas").map(res => res.json());
  }

  public listarTipoUsuarios() {
    return this.http.get(this.urlBase + "api/TipoUsuario/listarTipoUsuarios").map(res => res.json());
  }

  public listarPaginasTiposUsuario() {
    return this.http.get(this.urlBase + "api/TipoUsuario/listarPaginasTiposUsuario").map(res => res.json());
  }

  public listarPaginasRecuperar(iidTipoUsuario) {
    return this.http.get(this.urlBase + "api/TipoUsuario/listarPaginasRecuperar/" + iidTipoUsuario).map(res => res.json());
  }

  public guardarDatosTipoUsuario(tipoUsuarioCLS) {
    return this.http.post(this.urlBase + "api/TipoUsuario/guardarDatosTipoUsuario", tipoUsuarioCLS).map(res => res.json());
  }

  public eliminarTipoUsuario(iidTipoUsuario) {
    return this.http.get(this.urlBase + "api/TipoUsuario/eliminarTipoUsuario/" + iidTipoUsuario).map(res => res.json());
  }


  public listarPaginasBD() {
    return this.http.get("api/Pagina/listarPaginasBD").map(res => res.json());
  }

  public guardarPagina(paginaCLS) {
    return this.http.post(this.urlBase + "api/Pagina/guardarPagina", paginaCLS).map(res => res.json());
  }

  public recuperarPagina(idPagina) {
    return this.http.get("api/Pagina/recuperarPagina/" + idPagina).map(res => res.json());
  }

  public eliminarPagina(idPagina) {
    return this.http.get("api/Pagina/eliminarPagina/" + idPagina).map(res => res.json());
  }

}
