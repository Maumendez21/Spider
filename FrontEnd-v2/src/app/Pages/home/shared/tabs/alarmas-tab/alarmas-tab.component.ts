import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alarmas-tab',
  templateUrl: './alarmas-tab.component.html',
  styleUrls: ['./alarmas-tab.component.css']
})
export class AlarmasTabComponent implements OnInit {

  @Input() alarmas: any;

  public latitud: number = 19.4525976;
  public longitud: number = -99.1182164;
  public nombre: string;
  public zoom: number = 11;
  public map: any;
  public fitBounds: boolean = true;
  public agmFitBounds: boolean = true;

  constructor() { }
  ngOnInit(): void {
  }


  showMap(latitud: number, longitud: number, alarma: string ){
    this.latitud = latitud;
    this.longitud = longitud;
    this.nombre = alarma;
  }

  onMapReady(map: any) {
    this.map = map;

  }

}
