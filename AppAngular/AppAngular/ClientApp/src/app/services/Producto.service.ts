import { Injectable, Inject } from "@angular/core";
import { Http } from '@angular/http';

import 'rxjs/add/operator/map';

@Injectable()

export class ProductoService {

  urlBase: string = "";

  constructor(private http: Http, @Inject('BASE_URL') baseUrl: string) {

    //urlBase tiene el nombre del dominio
    this.urlBase = baseUrl;
  }

  public getProducto() {
    return this.http.get(this.urlBase + "api/Producto/listarProductos")
      .map(res => res.json());
  }

  public getFiltroProductoPorNombre(nombre) {
    return this.http.get(this.urlBase + "api/Producto/filtrarProductosPorNombre/" + nombre)
      .map(res => res.json());
  }

  public getFiltroProductoPorCategoria(idcategoria) {
    return this.http.get(this.urlBase + "api/Producto/filtrarProductosPorCategoria/" + idcategoria)
      .map(res => res.json());
  }

  public obtenerProductoPorId(idProducto) {
    return this.http.get("api/Producto/obtenerProductoPorId/" + idProducto)
      .map(res => res.json());
  }

  public listarMarcas() {
    return this.http.get("api/Producto/listarMarcas").map(res => res.json());
  }


}
