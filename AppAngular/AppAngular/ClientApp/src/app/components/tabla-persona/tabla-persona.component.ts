import { Component, OnInit, Input } from '@angular/core';
import { PersonaService } from '../../services/persona.service';

@Component({
  selector: 'tabla-persona',
  templateUrl: './tabla-persona.component.html',
  styleUrls: ['./tabla-persona.component.css']
})
export class TablaPersonaComponent implements OnInit {

  @Input() personas: any;
  cabeceras: string[] = ["Id Persona", "Nombre Completo", "Telefono","Correo"];
  constructor(private personaService: PersonaService) { }

  ngOnInit() {
    this.personaService.getPersona().subscribe(
      data => this.personas = data
    );
  }

}
