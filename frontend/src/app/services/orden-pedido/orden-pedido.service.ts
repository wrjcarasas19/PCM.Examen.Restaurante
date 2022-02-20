import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdenPedidoService {

  constructor(private http:HttpClient) { }

  Listar(ordenId: any): Observable<any[]>{
    let direccion = `${environment.urlApi}/orden/pedido?ordenId=${ordenId}`;

    return this.http.get<any[]>(direccion);
  }
  Insertar(pedido: any): Observable<any[]>{
    let direccion = `${environment.urlApi}/orden/pedido`;

    return this.http.post<any[]>(direccion, pedido);
  }
  Actualizar(pedido: any): Observable<any[]>{
    let direccion = `${environment.urlApi}/orden/pedido`;

    return this.http.put<any[]>(direccion, pedido);
  }
  Eliminar(pedido: any): Observable<any[]>{
    let direccion = `${environment.urlApi}/orden/pedido`;

    return this.http.delete<any[]>(direccion, { body: pedido });
  }
}