import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../services/usuario.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  isExpanded = false;
  login: boolean = false;

  constructor(private usuarioService: UsuarioService, private router: Router) {

  }

  ngOnInit() {
    this.usuarioService.obtenerSession().subscribe(data => {
      if (data) {
        this.login = true;
      } else {
        this.login = false;
        this.router.navigate(["/login"]);
      }
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  cerrarSesion() {
    this.usuarioService.cerrarSesion().subscribe(res => {
      if (res.valor == "OK") {
        this.login = false;
      }
    })
  }
}
