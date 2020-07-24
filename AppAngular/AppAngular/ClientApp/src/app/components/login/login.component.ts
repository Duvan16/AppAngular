import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UsuarioService } from '../../services/usuario.service';
import { Router } from '@angular/router';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  usuario: FormGroup;
  error: boolean = false;
  constructor(private usuarioService: UsuarioService, private router: Router) {
    this.usuario = new FormGroup({
      'nombreusuario': new FormControl("", Validators.required),
      'contra': new FormControl("", Validators.required)
    });
  }

  ngOnInit() {
  }

  login() {
    if (this.usuario.valid) {
      this.usuarioService.login(this.usuario.value).subscribe(res => {
        if (res.iidusuario == 0) {
          //error
          this.error = true;
        } else {
          //esta bien
          this.router.navigate(["/componente-bienvenida"]);
          this.error = false;
        }
      })
    }
  }

}
