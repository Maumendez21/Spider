import { Component, OnInit } from '@angular/core';
import { AnaliticService } from '../services/analitic.service';
import { MapService } from 'src/app/Pages/home/Services/map.service';
import { SharedService } from 'src/app/Services/shared.service';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-day-activity',
  templateUrl: './day-activity.component.html',
  styleUrls: ['./day-activity.component.css']
})
export class DayActivityComponent implements OnInit {

  constructor(
    private analiticService: AnaliticService,
    private mapService: MapService,
    private shared: SharedService,
    private router: Router,

  ) {
    this.getActivityDat(this.deviceSearch);
    this.shared.notificationsStream$.subscribe(data => {
      this.notifications = data;
    })
    
    this.getNotifications();
    this.shared.broadcastPermisosStream('ANA200');
  }
  
  ngAfterViewInit(): void {
    //Called after ngAfterContentInit when the component's view has been initialized. Applies to components only.
    //Add 'implements AfterViewInit' to the class.
    
    
  }

  public deviceSearch: string = "";
  public cargando: boolean = true;
  public data: any[];
  public unidades: string;

  public notifications: string;
  public notificationes: any[] = [];

  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};
  ngOnInit(): void {
  }

  getActivityDat(device: string){
    this.cargando = true;
    this.analiticService.getActivityDay(device).subscribe(data => {
      this.unidades = data['Actives'] + "/" + data['Inactives'];
      this.data = data['ListDay'];
      this.cargando = false;
    })
  }
  notify(){
    this.router.navigate(["/home/notification"]);
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
        this.notificationes = data['listNotifications'];
        this.dtTrigger.next();
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.router.navigate(['/login']);
      });
  }




}
