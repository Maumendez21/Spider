import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import moment from 'moment';
import { ToastrService } from 'ngx-toastr';

declare var google: any;

@Component({
  selector: 'app-heatmap-nav',
  templateUrl: './heatmap-nav.component.html',
  styleUrls: ['./heatmap-nav.component.css']
})
export class HeatmapNavComponent implements OnInit {

  isMarkerVisible: boolean = false;

  spiderMarkers: any = [];
  subempresas: any = [];

  dateWeek: string = "";
  fechaInicio: string = "";
  fechaFin: string = "";
  subempresa: string = "";
  vehiculo: string = "";

  map: any;
  heatmap: any;
  lat: number = 19.4525976;
  long: number = -99.1182164;
  zoom: number = 11;
  loading = false;
  // * FitBounds
  fitBounds: boolean = true;
  agmFitBounds: boolean = true;

  coordsHeatMap: any[] = [];
  coordsRaw: any[] = [];

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) {

    if (shared.verifyLoggin()) {
      this.getDevices();
      this.getSubempresas();
    } else {
      this.router.navigate(['/login']);
    }
  }

  getDevices() {
    this.spiderService.getDevicesGeneralNew("")
      .subscribe(data => {
        this.spiderMarkers = data['ListLastPosition'];
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

  ngOnInit(): void {
  }

  onMapReady(map: any) {
    this.map = map;
  }

  reloadPointsHeatMap() {

    this.heatmap = new google.maps.visualization.HeatmapLayer({
      map: this.map,
      data: this.coordsHeatMap
    });

    this.heatmap.setMap(this.map);
  }

  fechaInicioTemp: boolean = false;
  fechaFinTemp: boolean = false;
  fechaInicioInvalid:  boolean = false;


  validateDate(): boolean{
    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaInicioInvalid = moment(this.fechaInicio).isAfter(moment(this.fechaFin));
    return ((!this.fechaInicioTemp && !this.fechaFinTemp) && (!this.fechaInicioInvalid)) ? true : false
  }

  searchHeatMap() {

    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));

    if (this.fechaInicio != "" && this.fechaFin != "" && (this.subempresa || this.vehiculo)) {


      if (this.validateDate()) {

        this.clearHeatMap();
        this.loading = true;

        this.spiderService.getHeatMap(this.fechaInicio, this.fechaFin, this.subempresa, this.vehiculo)
          .subscribe(response => {

            if (response.length > 0) {
              this.coordsRaw = response;

              response.map(x => {
                this.coordsHeatMap.push(new google.maps.LatLng(x.Latitude, x.Longitude));
              });

              this.reloadPointsHeatMap();
              this.loading = false;
            } else {
              this.loading = false;
              this.toastr.warning("El vehiculo no contiene información para analizar", "Vehiculo sin viajes");
            }
          });
      }else {
        this.toastr.warning("Fechas invalidas.");
      }


    } else {
      this.toastr.warning("Es necesario seleccionar la semana y algun Grupo o Vehiculo", "Campos faltantes");
    }
  }

  clearHeatMap() {
    if (this.coordsHeatMap.length > 0) {
      this.coordsHeatMap = [];
      this.heatmap.setMap(null);
    }
  }

}
