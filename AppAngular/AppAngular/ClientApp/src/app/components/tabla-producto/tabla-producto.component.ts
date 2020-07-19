import { Component, OnInit, Input } from '@angular/core';
import { ProductoService } from '../../services/Producto.service';

@Component({
  selector: 'tabla-producto',
  templateUrl: './tabla-producto.component.html',
  styleUrls: ['./tabla-producto.component.css']
})
export class TablaProductoComponent implements OnInit {

  @Input() productos: any;
  @Input() isMantenimiento = false;
  p: number = 1;
  cabeceras: string[] = ["Id Producto", "Nombre", "Precio", "Stock", "Nombre Categoría"];

  constructor(private producto: ProductoService) { }

  //Todo lo que pongamos en el ngOnInit se va a ejecutar cuando cargue la pagina
  ngOnInit() {
    this.producto.getProducto().subscribe(
      data => this.productos = data
    );
  }

  eliminarProducto(idProducto) {
    if (confirm("¿Desea eliminar el registro?")) {
      this.producto.eliminarProducto(idProducto).subscribe(p => {
        //
        this.producto.getProducto().subscribe(
          data => this.productos = data
        );
      });
    }
  }

}
