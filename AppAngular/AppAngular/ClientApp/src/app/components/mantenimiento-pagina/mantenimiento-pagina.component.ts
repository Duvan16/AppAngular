import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'mantenimiento-pagina',
  templateUrl: './mantenimiento-pagina.component.html',
  styleUrls: ['./mantenimiento-pagina.component.css']
})
export class MantenimientoPaginaComponent implements OnInit {

  pagina: FormGroup;
  constructor() {
    this.pagina = new FormGroup({
      "iidpagina": new FormControl("0"),
      "mensaje": new FormControl("", [Validators.required, Validators.maxLength(100)]),
      "accion": new FormControl("", [Validators.required, Validators.maxLength(100)])
    });
  }

  ngOnInit() {
  }

}
