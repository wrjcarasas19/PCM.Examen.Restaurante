import { Injectable, TemplateRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  toasts: any[] = [];

  showError(textOrTpl: string | TemplateRef<any>) {
    this.show(textOrTpl, {
      classname: 'bg-danger text-light',
      delay: 3000,
      autohide: true
    })    
  }
  showWarning(textOrTpl: string | TemplateRef<any>) {
    this.show(textOrTpl, {
      classname: 'bg-warning text-light',
      delay: 3000,
      autohide: true
    })    
  }
  showSuccess(textOrTpl: string | TemplateRef<any>) {
    this.show(textOrTpl, {
      classname: 'bg-success text-light',
      delay: 3000,
      autohide: true
    })    
  }
  show(textOrTpl: string | TemplateRef<any>, options: any = {}) {
    this.toasts.push({ textOrTpl, ...options });
  }

  remove(toast: any) {
    this.toasts = this.toasts.filter(t => t !== toast);
  }
}