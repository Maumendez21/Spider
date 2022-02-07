import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import moment from 'moment';
import { SharedService } from 'src/app/Services/shared.service';
import { SpiderService } from 'src/app/Services/spider.service';
import Swal from 'sweetalert2';
import { AnaliticService } from '../services/analitic.service';
declare var google: any;

@Component({
  selector: 'app-heath-map',
  templateUrl: './heath-map.component.html',
  styleUrls: ['./heath-map.component.css']
})
export class HeathMapComponent implements OnInit {

  constructor(
    private spiderService: SpiderService,
    private analiticService: AnaliticService,
    private shared: SharedService,
    private router: Router,
  ) {
    this.getDevices();
    this.getSubempresas();
  }

  enableLoading: boolean = true;

  public isMarkerVisible: boolean = false;

  public spiderMarkers: any = [];
  public subempresas: any = [];

  public dateWeek: string = "";
  public fechaInicio: string = "";
  public fechaFin: string = "";
  public subempresa: string = "";
  public vehiculo: string = "";

  public map: any;
  public heatmap: any;
  public lat: number = 19.4525976;
  public long: number = -99.1182164;
  public zoom: number = 11;
  public loading = false;
  public // * FitBounds
  public fitBounds: boolean = false;
  public agmFitBounds: boolean = false;

  public coordsHeatMap: any[] = [];
  public coordsRaw: any[] = [];

  ngOnInit(): void {
  }

  onMapReady(map: any) {
    this.map = map;
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

  fechaInicioTemp: boolean = false;
  fechaFinTemp: boolean = false;
  fechaInicioInvalid:  boolean = false;


  validateDate(): boolean{
    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaInicioInvalid = moment(this.fechaInicio).isAfter(moment(this.fechaFin));
    return ((!this.fechaInicioTemp && !this.fechaFinTemp) && (!this.fechaInicioInvalid)) ? true : false
  }

  async searchHeatMap() {

    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));

    if (this.fechaInicio != "" && this.fechaFin != "" && (this.vehiculo)) {


      if (this.validateDate()) {

        this.clearHeatMap();
        this.loading = true;

        await this.analiticService.getHeatMap(this.fechaInicio, this.fechaFin, this.subempresa, this.vehiculo)
          .subscribe(response => {

            if (response.length > 0) {
              this.coordsRaw = response;

              response.map(x => {
                this.coordsHeatMap.push(new google.maps.LatLng(x.Latitude, x.Longitude));
              });

              this.reloadPointsHeatMap();
              this.loading = false;

              this.fitBounds = true;
              this.agmFitBounds = true;
            } else {
              this.loading = false;
              Swal.fire('Ups! :/', 'El vehiculo no contiene información para analizar', 'warning')
              
            }
          });
        }else {
          Swal.fire('Ups! :/', 'Fechas invalidas', 'error')
        }
        
        
      } else {
        Swal.fire('Atención!', 'Selecciona la fecha inicio y fin y un vehiculo', 'warning')
    }
  }

  reloadPointsHeatMap() {

    this.heatmap = new google.maps.visualization.HeatmapLayer({
      map: this.map,
      data: this.coordsHeatMap
    });

    this.heatmap.setMap(this.map);
  }

  clearHeatMap() {
    if (this.coordsHeatMap.length > 0) {
      this.coordsHeatMap = [];
      this.heatmap.setMap(null);
    }
  }



}
