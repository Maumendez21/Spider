import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { SharedService } from 'src/app/Services/shared.service';
import { MapService } from '../Services/map.service';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {

  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};
  notifications: any[] = [];


  constructor(
    private mapService: MapService,
    private shared: SharedService,
    private router: Router,
  ) {
    this.getNotifications();
  }

  ngOnInit(): void {
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

    await this.mapService.getNotifications()
      .subscribe(data => {
        this.notifications = data['listNotifications'];
        this.dtTrigger.next();
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.router.navigate(['/login']);
      });
  }

}
