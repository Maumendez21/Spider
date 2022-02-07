import { Component } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-movilidad',
  templateUrl: './movilidad.component.html',
  styleUrls: ['./movilidad.component.css']
})
export class MovilidadComponent{

  production: boolean;

  constructor(private shared: SharedService) {
    this.preparePermisos();
    this.production = environment.production;
  }

  permisos: string;

  preparePermisos() {
    this.shared.permisosStream$.subscribe(response => {
      this.permisos = response;
    });

    if (!this.permisos) {
      this.permisos = localStorage.getItem('permits');
    }
  }

}
