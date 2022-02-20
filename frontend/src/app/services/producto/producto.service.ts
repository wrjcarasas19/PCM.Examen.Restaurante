import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {

  constructor(private http:HttpClient) { }

  Listar(): Observable<any[]>{
    let direccion = `${environment.urlApi}/producto`;

    return this.http.get<any[]>(direccion);
  }
}
