import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AdminService } from 'src/app/Pages/admin/services/admin.service';
import { SpiderService } from 'src/app/Services/spider.service';
import Swal from 'sweetalert2';
import { GeofencesService } from '../../Pages/geofences/services/geofences.service';
import { MobilityService } from '../../Pages/mobility/services/mobility.service';

@Component({
  selector: 'app-filters-form',
  templateUrl: './filters-form.component.html',
  styleUrls: ['./filters-form.component.css']
})
export class FiltersFormComponent implements OnInit {

  subempresas: any = [];
  puntosInteres: any = [];
  spiderMarkers: any = [];

  fechaInicio: string = "";
  fechaFin: string = "";
  compani: string = "";
  vehiculo: string = "";
  puntoInteres: string ="";
  geofences: any;
  geofence: string = "";

  @Input() section: string;
  @Output() data: EventEmitter<any[]> = new EventEmitter();

  constructor(
    private router: Router,
    private spiderService: SpiderService,
    private geofencesService: GeofencesService,
    private mobilityService: MobilityService,
    private adminService : AdminService
  ) {
    this.getDevices("");
    this.getSubempresas();
    this.getListGeocercas();
    this.getPuntosInteres();
  }

  getPuntosInteres(){
    this.mobilityService.getPuntosInteresMovilidad()
    .subscribe(data => {
      this.puntosInteres = data;
    });
  }

  getListGeocercas() {
    this.geofencesService.getListGeocercasMonitoreo()
        .subscribe(data => {
          this.geofences = data;
        })
  }

  getDevicesGeo(geo: any){
    this.geofencesService.getDevicesGeofenceMonitoreo(this.geofences[geo].Id)
        .subscribe(data => {
          this.spiderMarkers = data;
        });
  }


  getDevices(compani: string) {
    this.spiderService.getDevicesAdmin(compani)
    .subscribe(data => {
      this.spiderMarkers = data;
    }, error => {
      this.router.navigate(['/login']);
    });
  }

  getSubempresas() {
    this.spiderService.getSubempresas()
    .subscribe(data => {

      this.subempresas = data;
    });
  }

  ngOnInit(): void {
  }

  filtrar(){
    const data = {
      punto: this.puntoInteres,
      compania: this.compani,
      device: this.vehiculo,
      fechaIni: this.fechaInicio + ' 00:00:00',
      fechaEnd: this.fechaFin  + ' 23:59:00'
    }

    if (this.fechaInicio != "" && this.fechaFin != "") {
      if (this.section == '5'){
        //  return this.spiderService.getNotificatonsAnaliticas(data.fechaIni, data.fechaEnd)
        // .subscribe((data: any[]) => {
        //   this.data.emit(data);
        // })
      }
    }else{
     

      return Swal.fire({
        icon: 'error',
        title: 'Es necesario seleccionar el dispositivo, la fecha inicio y fecha fin'
      })

    }

    if (this.vehiculo != "" && this.fechaInicio != "" && this.fechaFin != "") {
      if (this.section == '1') {
        this.mobilityService.getResponsablesMovilidad(data.device, data.fechaIni, data.fechaEnd)
        .subscribe((res: any) => {

          const datos = [{
            responsables: res,
            datos: data
          }]

          this.data.emit(datos);
        })

      }else if (this.section == '2') {

        this.adminService.getAlarmasAdmin(data.device, data.fechaIni, data.fechaEnd)
        .subscribe(data => {
          
          
          this.data.emit(data);
        })

      }else if (this.section == '3') {

        this.adminService.getRutasAdmin( data.compania ,data.device, data.fechaIni, data.fechaEnd)
        .subscribe(data => {
          this.data.emit(data);
        })

      }else if (this.section == '4') {

        // Instrcucción para punto de interés
        this.mobilityService.getAnalisisPuntosInteresMovilidad( data.punto, data.fechaIni, data.fechaEnd, data.device)
        .subscribe((res: any[]) => {
          const datos = [{
            puntos: res,
            datos: data
          }]

          


          this.data.emit(datos);
        })
      }else if (this.section == '6'){

        this.idGeofence = this.geofences[this.geofence]["Id"];

        this.geofencesService.getListGeocercasHistorico( data.device, this.idGeofence, data.fechaIni, data.fechaEnd)
        .subscribe((data: any[]) => {
          const datos = [{
            devices: data,
            idGeo: this.idGeofence,
            geocercas: this.geofences,
            geocerca: this.geofence
          }]
          this.data.emit(datos);
        })

      }

    }else{


      Swal.fire({
        icon: 'error',
        title: 'Es necesario seleccionar el dispositivo, la fecha inicio y fecha fin'
      })
    }
  }

  idGeofence: string;

}
