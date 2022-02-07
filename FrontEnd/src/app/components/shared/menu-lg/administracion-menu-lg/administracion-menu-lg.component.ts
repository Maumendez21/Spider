import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../../../services/shared.service';

@Component({
  selector: 'app-administracion-menu-lg',
  templateUrl: './administracion-menu-lg.component.html',
  styleUrls: ['./administracion-menu-lg.component.css']
})
export class AdministracionMenuLgComponent implements OnInit {


  permisos: string;

  constructor(private shared: SharedService) {
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
