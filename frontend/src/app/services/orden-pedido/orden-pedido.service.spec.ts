import { TestBed } from '@angular/core/testing';

import { OrdenPedidoService } from './orden-pedido.service';

describe('OrdenPedidoService', () => {
  let service: OrdenPedidoService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrdenPedidoService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
