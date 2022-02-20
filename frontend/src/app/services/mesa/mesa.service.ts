import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MesaService {

  constructor(private http:HttpClient) { }

  Listar(): Observable<any[]>{
    let direccion = `${environment.urlApi}/mesa`;

    return this.http.get<any[]>(direccion);
  }
  SeleccionarPorId(id:string): Observable<any>{

    let direccion = `${environment.urlApi}/mesa?mesaId=${id}`;

    return this.http.get<any>(direccion);
  }
}
