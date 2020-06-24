import { Injectable, Inject } from "@angular/core";
import { Http } from '@angular/http';

@Injectable()

export class ProductoService {

  urlBase: string = "";

  constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {

    //urlBase tiene el nombre del dominio
    this.urlBase = baseUrl;
  }

  public getProducto() {
    return this.http.get(this.urlBase +"api/Producto/listarProductos")
  }


}
