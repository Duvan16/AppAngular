import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class UsuarioService {

  urlBase: string = "";
  constructor(private http: Http, @Inject("BASE_URL") url: string) {
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

}
