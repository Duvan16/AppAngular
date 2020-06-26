import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';

//Registrar el componente creado

import { ButtonAgregar } from './components/button/Button.component';
import { DiasSemana } from './components/diasSemana/diasSemana.component';
import { TablaProductoComponent } from './components/tabla-producto/tabla-producto.component';
import { ProductoService } from './services/Producto.service';
import { HttpModule } from '@angular/http';
import { BuscadorProductoNombreComponent } from './components/buscador-producto-nombre/buscador-producto-nombre.component';
import { FiltradoProductoNombreComponent } from './components/filtrado-producto-nombre/filtrado-producto-nombre.component';
import { FiltradoProductoCategoriaComponent } from './components/filtrado-producto-categoria/filtrado-producto-categoria.component';
import { BuscadorProductoCategoriaComponent } from './components/buscador-producto-categoria/buscador-producto-categoria.component';
import { CategoriaService } from './services/categoria.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    ButtonAgregar,
    DiasSemana,
    TablaProductoComponent,
    BuscadorProductoNombreComponent,
    FiltradoProductoNombreComponent,
    FiltradoProductoCategoriaComponent,
    BuscadorProductoCategoriaComponent
  ],
  imports: [
    HttpModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: 'filtradoProductoCategoria', component: FiltradoProductoCategoriaComponent, pathMatch: 'full' },
      { path: 'filtradoProductoNombre', component: FiltradoProductoNombreComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'diasSemana', component: DiasSemana },
    ])
  ],
  providers: [ProductoService, CategoriaService],
  bootstrap: [AppComponent]
})
export class AppModule { }
