import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdenService {

  constructor(private http: HttpClient) { }

  Insertar(orden: any): Observable<any> {
    let direccion = `${environment.urlApi}/orden`;

    return this.http.post<any>(direccion, orden);
  }
  Actualizar(orden: any): Observable<any> {
    let direccion = `${environment.urlApi}/orden`;

    return this.http.put<any>(direccion, orden);
  }
}
