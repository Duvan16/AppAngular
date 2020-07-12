import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class PersonaService {

  urlBase: string;
  constructor(private http: Http, @Inject("BASE_URL") url: string) {
    this.urlBase = url;
  }

  public getPersona() {
    return this.http.get(this.urlBase + "api/Persona/listarPersonas")
      .map(res => res.json());
  }

  public getPersonaFiltro(nombreCompleto) {
    return this.http.get(this.urlBase + "api/Persona/filtrarPersona/" + nombreCompleto)
      .map(res => res.json());
  }

  public agregarPersona(persona) {
    var url = this.urlBase + "api/Persona/guardarPersona";
    return this.http.post(url, persona).map(res => res.json());
  }

  public recuperarPersona(idPersona) {
    return this.http.get(this.urlBase + "api/Persona/recuperarPersona/" + idPersona)
      .map(res => res.json());
  }

}
