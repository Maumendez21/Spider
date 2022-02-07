import { Component, Input, OnInit } from '@angular/core';
import { SharedService } from '../../../Services/shared.service';

@Component({
  selector: 'app-filtro-fechas-travels',
  templateUrl: './filtro-fechas-travels.component.html',
  styleUrls: ['./filtro-fechas-travels.component.css']
})
export class FiltroFechasTravelsComponent implements OnInit {

  constructor(
    private shared: SharedService,
  ) {
    this.getDevice();
  }

  @Input() prepareDates: any;
  public fechaInicio: string;
  public fechaFin: string;
  public device: string;
  public lat: number;
  public lng: number;
  public zoom: number;

  ngOnInit(): void {
  }


  getDevice(){
    this.shared.zoomCoordsStream$.subscribe(data => {
      this.device = data.device
      this.lat = data.latitud
      this.lng = data.longitud
      this.zoom = data.zoom
    })
  }


  searchAllTrips() {
    if (this.fechaInicio && this.fechaFin) {
      this.shared.broadcastZoomCoordsStream({
        device: this.device,
        latitud: this.lat,
        longitud: this.lng,
        zoom: this.zoom,
        bottom: true,
        filterBottom: true,
        startDate: this.fechaInicio,
        endDate: this.fechaFin
      });
    }
  }




}
