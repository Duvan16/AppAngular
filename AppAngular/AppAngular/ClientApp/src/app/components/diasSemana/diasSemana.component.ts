import { Component } from "@angular/core";


@Component({
  selector: "diasSemana",
  templateUrl: "./diasSemana.component.html"
})

export class DiasSemana {
  nombre: string = "Licito";
  cursos: string[] = ["LinQ", "Ado.net", "Asp.net MVC", "Angular"];
  persona: Object = {
    nombre: "Pepe",
    apellido: "Perez"
  }
  enlace: string = "https://www.google.com.co/";
}
