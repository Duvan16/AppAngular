import { Injectable, Inject } from '@angular/core';
import { Http } from '@angular/http';

import 'rxjs/add/operator/map';

@Injectable()
export class CategoriaService {

  urlBase: string;

  constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {
    this.urlBase = baseUrl;
  }

  public getCategoria() {
    return this.http.get("api/Categoria/listarCategorias")
      .map(res => res.json());
  }

}
