import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SharedService } from '../../../../services/shared.service';

@Component({
  selector: 'app-configuration-menu-lg',
  templateUrl: './configuration-menu-lg.component.html',
  styleUrls: ['./configuration-menu-lg.component.css']
})
export class ConfigurationMenuLgComponent implements OnInit {

  production: boolean;
  permisos: string;

  constructor(private shared: SharedService) {
    this.production = environment.production;
    this.preparePermisos();
  }

  preparePermisos() {
    this.shared.permisosStream$.subscribe(response => {
      this.permisos = response;
    });

    if (!this.permisos) {
      this.permisos = localStorage.getItem('permits');
    }
  }

  ngOnInit(): void {
  }

}
