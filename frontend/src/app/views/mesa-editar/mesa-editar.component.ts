import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MesaService } from 'src/app/services/mesa/mesa.service';
import { OrdenPedidoService } from 'src/app/services/orden-pedido/orden-pedido.service';
import { OrdenService } from 'src/app/services/orden/orden.service';
import { ProductoEtiquetaService } from 'src/app/services/producto-etiqueta/producto-etiqueta.service';
import { ProductoService } from 'src/app/services/producto/producto.service';
import { ToastService } from 'src/app/services/toast/toast.service';

@Component({
  selector: 'app-mesa-editar',
  templateUrl: './mesa-editar.component.html',
  styleUrls: ['./mesa-editar.component.scss']
})
export class MesaEditarComponent implements OnInit {

  FiltroProducto = new FormGroup({
    Tipo: new FormControl(''),
    Categoria: new FormControl(''),
    Texto: new FormControl('')
  })
  MesaId: any = 0;
  Mesa: any = {};
  Orden: any = {};
  PedidoSeleccionado: any = {};
  ListaPedido: any[] = [];
  ListaProducto: any[] = [];
  ListaProductosFiltrados: any[] = [];
  ListaProductoEtiqueta: any[] = [];
  ListaEtiquetaTipo: any[] = [];
  ListaEtiquetaCategoria: any[] = [];

  constructor(private route: ActivatedRoute, private toastService: ToastService, private modalService: NgbModal, private mesaService: MesaService, private productoService: ProductoService, private productoEtiquetaService: ProductoEtiquetaService, private ordenService: OrdenService, private ordenPedidoService: OrdenPedidoService) { }

  ngOnInit(): void {
    this.MesaId = +this.route.snapshot.paramMap.get('id')!;

    this.ObtenerMesa();
    this.ListarProductos();
    this.ListarProductoEtiquetas();

    this.FiltroProducto.valueChanges.subscribe(x => {
      this.FiltrarProductos();
    });
  }
  ObtenerMesa() {
    this.mesaService.SeleccionarPorId(this.MesaId).subscribe(x => {
      this.Mesa = x;

      if (this.Mesa.Estado == 111) {
        this.Orden = this.Mesa.Orden;
        this.ListarPedidos();
      }
      else {
        this.ListaPedido = [];
      }
    });
  }
  ListarPedidos() {
    this.ordenPedidoService.Listar(this.Orden.OrdenId).subscribe(x => {
      this.ListaPedido = x;

      this.ListaPedido.forEach(x => {
        x.Producto = { Nombre: 'No encontrado' };
      });

      if (this.ListaProducto.length > 0) this.CargarPedidoProducto();
    });
  }
  ListarProductos() {
    this.productoService.Listar().subscribe(x => {
      this.ListaProducto = x;
      this.FiltrarProductos();
    })
  }
  FiltrarProductosBasico(filtro: any, x: any): boolean {
    var texto: any;

    if (filtro.Tipo && x.Etiquetas.filter((y: any) => y.Nombre == 'tipo' && y.Valor == filtro.Tipo).length == 0) return true;
    else if (filtro.Categoria && x.Etiquetas.filter((y: any) => y.Nombre == 'categoria' && y.Valor == filtro.Categoria).length == 0) return true;
    else if (filtro.Texto) {
      texto = '' + x.ProductoId + x.Nombre + x.Precio;

      if (texto.toLocaleLowerCase().indexOf(filtro.Texto) < 0) return true;
    }

    return false;
  }
  FiltrarProductosAvanzado(obj: any, x: any) {
    for (var key in obj)
      if (obj[key] != x[key]) return true;

    return false;
  }
  FiltrarProductos() {
    var filtro = this.FiltroProducto.value;
    var texto = '';
    var productos: any[] = [];
    var filtroFunction: any;
    var filtroObjeto: any;

    filtroFunction = this.FiltrarProductosBasico;

    if (filtro.Texto) {
      if (filtro.Texto.startsWith('$')) {
        try {
          filtroObjeto = JSON.parse(filtro.Texto.substring(1));
          filtroFunction = this.FiltrarProductosAvanzado;
          filtro = filtroObjeto;

          console.log(filtroObjeto);
        } catch (error) {
          filtro.Texto = filtro.Texto.toLocaleLowerCase();
        }
      }
      else {
        filtro.Texto = filtro.Texto.toLocaleLowerCase();
      }
    }

    this.ListaProducto.forEach(x => {
      x.Existe = false;
      x.EstadoClase = x.Disponible ? 'input-group mb-3 product-success' : 'input-group mb-3 product-danger';
      x.EstadoTexto = x.Disponible ? '' : 'No disponible';

      x.Cantidad = 1;

      if (filtroFunction(filtro, x)) return;

      if (x.Disponible) productos.unshift(x);
      else productos.push(x);
    });

    this.ListaProductosFiltrados = productos;
    this.CargarPedidoProducto();
  }
  CargarPedidoProducto() {
    var productos: any[] = [];
    var total: any = 0;
    var productoPedido: any;

    this.ListaPedido.forEach(x => {
      total += x.Cantidad * x.PrecioUnitario;
      productos = this.ListaProducto.filter(y => y.ProductoId == x.ProductoId);

      if (productos.length == 1) {
        x.Producto = productoPedido = productos[0];

        productoPedido.Existe = true;
        productoPedido.Disponible = true;
        productoPedido.EstadoClase = 'input-group mb-3 product-warning';
        productoPedido.Cantidad = x.Cantidad;
      }
    });

    this.Orden.Total = total;
  }
  ListarProductoEtiquetas() {
    this.productoEtiquetaService.Listar().subscribe(x => {
      this.ListaProductoEtiqueta = x;

      this.ListaEtiquetaTipo = this.ListaProductoEtiqueta.filter(x => x.Nombre == 'tipo');
      this.ListaEtiquetaCategoria = this.ListaProductoEtiqueta.filter(x => x.Nombre == 'categoria');
    })
  }

  AumentarProducto(producto: any) {
    let productos = this.ListaProducto.filter(x => x.ProductoId == producto.ProductoId);

    if (productos.length == 1) productos[0].Cantidad++;
  }
  DisminuirProducto(producto: any) {
    let productos = this.ListaProducto.filter(x => x.ProductoId == producto.ProductoId);

    if (productos.length == 1 && productos[0].Cantidad > 1) productos[0].Cantidad--;
  }
  AgregarProducto(producto: any) {
    if (this.Mesa.Estado == 101) {
      this.ordenService.Insertar({ MesaId: this.MesaId }).subscribe(x => {
        this.Mesa.Orden = this.Orden = x;
        this.Mesa.Estado = 111;

        this.AgregarProductoInterno(producto);
        this.toastService.showSuccess('Se creo la orden exitosamente');

        this.FiltroProducto.reset();
      })
    }
    else {
      this.AgregarProductoInterno(producto);

      this.FiltroProducto.reset();
    }
  }
  AgregarProductoInterno(producto: any) {
    let productos = this.ListaProducto.filter(x => x.ProductoId == producto.ProductoId);

    if (productos.length != 1) return;

    producto = productos[0];
    producto.OrdenId = this.Orden.OrdenId;

    if (producto.Existe) {
      this.ordenPedidoService.Actualizar(producto).subscribe(x => {
        this.ListaPedido = x;
        this.CargarPedidoProducto();
        this.toastService.showSuccess('Se actualizo el pedido exitosamente');
      });
    }
    else {
      this.ordenPedidoService.Insertar(producto).subscribe(x => {
        this.ListaPedido = x;
        this.CargarPedidoProducto();
        this.toastService.showSuccess('Se agrego el pedido exitosamente');
      });
    }
  }
  EditarPedido(pedido: any) {
    this.FiltroProducto.get('Texto')?.setValue('${"ProductoId":'+pedido.ProductoId+'}')
  }
  EliminarPedido(content: any, pedido: any) {
    this.PedidoSeleccionado = pedido;

    this.modalService.open(content).result.then((result) => {
      if (result == 'eliminar') {
        this.ordenPedidoService.Eliminar(pedido).subscribe(x => {
          this.ListaPedido = x;
          this.FiltroProducto.reset();
          this.ListarProductos();
          this.toastService.showSuccess('Se elimino el pedido exitosamente');
        });
      }
    }, (reason) => {

    });
  }
  FinalizarOrden(content: any) {
    var orden = JSON.parse(JSON.stringify(this.Orden));

    orden.Estado = 201;

    this.modalService.open(content).result.then((result) => {
      if (result == 'finalizar') {
        this.ordenService.Actualizar(orden).subscribe(x => {
          this.ObtenerMesa();
          this.toastService.showSuccess('Se finalizo la orden exitosamente');
        });
      }
    }, (reason) => {
      
    });
  }
  LimpiarFiltroTexto() {
    this.FiltroProducto.get('Texto')?.reset();
  }
}
