import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MesaListarComponent } from './mesa-listar.component';

describe('MesaListarComponent', () => {
  let component: MesaListarComponent;
  let fixture: ComponentFixture<MesaListarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MesaListarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MesaListarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
