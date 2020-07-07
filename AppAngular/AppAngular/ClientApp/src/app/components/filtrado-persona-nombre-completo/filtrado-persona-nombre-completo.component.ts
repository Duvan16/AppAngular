import { Component, OnInit } from '@angular/core';
import { PersonaService } from '../../services/persona.service';

@Component({
  selector: 'filtrado-persona-nombre-completo',
  templateUrl: './filtrado-persona-nombre-completo.component.html',
  styleUrls: ['./filtrado-persona-nombre-completo.component.css']
})
export class FiltradoPersonaNombreCompletoComponent implements OnInit {

  personas: any;
  constructor(private personService: PersonaService) { }

  ngOnInit() {
  }

  buscar(nombreCompleto) {
    this.personService.getPersonaFiltro(nombreCompleto.value).
      subscribe(res => this.personas = res);
  }

}
