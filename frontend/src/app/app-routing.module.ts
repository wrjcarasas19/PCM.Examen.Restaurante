import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MesaEditarComponent } from './views/mesa-editar/mesa-editar.component';
import { MesaListarComponent } from './views/mesa-listar/mesa-listar.component';

const routes: Routes = [
  {
    path: '',
    redirectTo: 'mesa',
    pathMatch: 'full'
  },
  {
    path: 'mesa',
    component: MesaListarComponent
  },
  {
    path: 'mesa/:id',
    component: MesaEditarComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
