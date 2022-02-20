import { TestBed } from '@angular/core/testing';

import { ProductoEtiquetaService } from './producto-etiqueta.service';

describe('ProductoEtiquetaService', () => {
  let service: ProductoEtiquetaService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProductoEtiquetaService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
