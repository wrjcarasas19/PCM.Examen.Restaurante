import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MesaService } from 'src/app/services/mesa/mesa.service';
import { ToastService } from 'src/app/services/toast/toast.service';

@Component({
  selector: 'app-mesa-listar',
  templateUrl: './mesa-listar.component.html',
  styleUrls: ['./mesa-listar.component.scss']
})
export class MesaListarComponent implements OnInit {

  FiltroMesa = new FormGroup({
    Estado: new FormControl('0', [Validators.required])
  });
  ListaMesas: any[] = [];
  ListaMesasFiltradas: any[] = [];

  constructor(private router: Router, private mesaService: MesaService, private toastService: ToastService) { }

  ngOnInit(): void {
    this.ListarMesas();

    this.FiltroMesa.get('Estado')?.valueChanges.subscribe(x => {
      this.FiltrarMesas();
    })
  }

  ListarMesas() {
    this.mesaService.Listar().subscribe(x => {
      this.ListaMesas = x;
      this.FiltroMesa.get('Estado')?.setValue('0');
    })
  }
  FiltrarMesas() {
    var listaPadre: any[] = [];
    var listaHija: any[] = [];
    var estado = this.FiltroMesa.get('Estado')?.value;

    this.ListaMesas.forEach(x => {
      switch (x.Estado) {
        case 101: x.EstadoClase = 'card bg-success text-white'; break;
        case 111: x.EstadoClase = 'card bg-danger text-white'; break;
        case 901: x.EstadoClase = 'card bg-secondary text-white'; break;
        default: x.EstadoClase = 'card'; break;
      }

      if (listaHija.length == 6) {
        listaPadre.push(listaHija);
        listaHija = [];
      }

      if (estado == 0 || estado == x.Estado) listaHija.push(x);
    })

    if (listaHija.length > 0) {
      listaPadre.push(listaHija);
    }

    this.ListaMesasFiltradas = listaPadre;
  }
  SeleccionarMesa(mesa: any) {
    if (mesa.Estado != 901) this.router.navigate(['/mesa/' + mesa.MesaId]);
    else this.toastService.showWarning('No se puede gestionar una mesa inactiva');
  }
}
