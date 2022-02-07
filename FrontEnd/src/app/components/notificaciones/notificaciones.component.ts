import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { Subject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

declare const google: any;

@Component({
  selector: 'app-notificaciones',
  templateUrl: './notificaciones.component.html',
  styleUrls: ['./notificaciones.component.css']
})
export class NotificacionesComponent implements OnInit {

  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};
  notifications: any[] = [];



  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private route: Router, private toastr: ToastrService) {

    this.limpiarFiltrosMapa();

    if (this.shared.verifyLoggin()) {
      this.getNotifications();
    } else {
      this.route.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }



  async getNotifications() {

    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      ordering: true,
      responsive: true,
      language: {
        'search': "Buscar",
        'paginate': {
          'first': 'Primero',
          'previous': 'Anterior',
          'next': 'Siguiente',
          'last': 'Ultimo'
        },
        'lengthMenu': 'Mostrar _MENU_ documentos',
        'info': 'Mostrando _PAGE_ de _PAGES_'
      },
    };

    await this.spiderService.getNotifications()
      .subscribe(data => {

        this.notifications = data['listNotifications'];
        this.dtTrigger.next();
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.route.navigate(['/login']);
      });
  }

}
