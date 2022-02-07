import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-alarmas',
  templateUrl: './alarmas.component.html',
  styleUrls: ['./alarmas.component.css']
})
export class AlarmasComponent  {
  alarmas: any[];

  constructor(private router: Router, private shared: SharedService) {
    this.limpiarFiltrosMapa();
    if (shared.verifyLoggin()) {

    } else {
      this.router.navigate(['/login']);
    }
  }

  limpiarFiltrosMapa() {
    this.shared.limpiarFiltros();
    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }


}
