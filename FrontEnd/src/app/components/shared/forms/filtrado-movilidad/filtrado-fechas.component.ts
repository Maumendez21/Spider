import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-filtrado-fechas',
  templateUrl: './filtrado-fechas.component.html',
  styleUrls: ['./filtrado-fechas.component.css']
})
export class FiltradoFechasComponent {



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

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    if (shared.verifyLoggin()) {
      this.getDevices("");
      this.getSubempresas();
      this.getPuntosInteres();
      this.getListGeocercas();
    } else {
      this.router.navigate(['/login']);
    }
  }

  getPuntosInteres(){
    this.spiderService.getPuntosInteresMovilidad()
    .subscribe(data => {
      this.puntosInteres = data;
    });
  }

  getListGeocercas() {
    this.spiderService.getListGeocercasMonitoreo()
        .subscribe(data => {
          this.geofences = data;
        })
  }

  getDevicesGeo(geo: any){
    this.spiderService.getDevicesGeofenceMonitoreo(this.geofences[geo].Id)
        .subscribe(data => {
          this.spiderMarkers = data;
        });
  }


  getDevices(compani: string) {
    this.spiderService.getDevicesAdmin(compani)
    .subscribe(data => {
      this.spiderMarkers = data;
    }, error => {
      this.shared.broadcastLoggedStream(false);
      this.shared.clearSharedSession();
      this.router.navigate(['/login']);
    });
  }

  getSubempresas() {
    this.spiderService.getSubempresas()
    .subscribe(data => {

      this.subempresas = data;
    });
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
         return this.spiderService.getNotificatonsAnaliticas(data.fechaIni, data.fechaEnd)
        .subscribe((data: any[]) => {
          this.data.emit(data);
        })
      }
    }else{
      return this.toastr.warning("Es necesario seleccionar el dispositivo, la fecha inicio y fecha fin");
    }

    if (this.vehiculo != "" && this.fechaInicio != "" && this.fechaFin != "") {
      if (this.section == '1') {
        this.spiderService.getResponsablesMovilidad(data.device, data.fechaIni, data.fechaEnd)
        .subscribe((res: any) => {

          const datos = [{
            responsables: res,
            datos: data
          }]

          this.data.emit(datos);
        })

      }else if (this.section == '2') {

        this.spiderService.getAlarmasAdmin(data.device, data.fechaIni, data.fechaEnd)
        .subscribe(data => {
          this.data.emit(data);
        })

      }else if (this.section == '3') {

        this.spiderService.getRutasAdmin( data.compania ,data.device, data.fechaIni, data.fechaEnd)
        .subscribe(data => {
          this.data.emit(data);
        })

      }else if (this.section == '4') {

        // Instrcucción para punto de interés
        this.spiderService.getAnalisisPuntosInteresMovilidad( data.punto, data.fechaIni, data.fechaEnd, data.device)
        .subscribe((res: any[]) => {
          const datos = [{
            puntos: res,
            datos: data
          }]


          this.data.emit(datos);
        })
      }else if (this.section == '6'){
        // console.log('geoceercas historico');

        this.idGeofence = this.geofences[this.geofence]["Id"];
        // console.log(this.idGeofence);

        this.spiderService.getListGeocercasHistorico( data.device, this.idGeofence, data.fechaIni, data.fechaEnd)
        .subscribe((data: any[]) => {
          const datos = [{
            devices: data,
            idGeo: this.idGeofence,
            geocercas: this.geofences,
            geocerca: this.geofence
          }]
          this.data.emit(datos);
          // console.log(data);
          // console.log();


        })










      }

    }else{
      this.toastr.warning("Es necesario seleccionar el dispositivo, la fecha inicio y fecha fin");
    }
  }

  idGeofence: string;

}
