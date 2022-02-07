import { Component, Input, OnInit } from '@angular/core';
import { MapService } from '../../../Services/map.service';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-info-mdevice-modal',
  templateUrl: './info-mdevice-modal.component.html',
  styleUrls: ['./info-mdevice-modal.component.css']
})
export class InfoMDeviceModalComponent {

  // Gráficas 
  // km
  public graficaKmData: any[];
  public graficaKmLabel: any[];
  // time
  public graficaTimeData: any[];
  public graficaTimeLabel: any[];
  // fuel
  public graficaFuelData: any[];
  public graficaFuelLabel: any[];

  @Input() public device: any;
  @Input() public tipo: number;
  @Input() public infoDevice: any;
  @Input() public alarmas: any;
  @Input() public setDevice: any;
  @Input() public getEstatusName: any;
  @Input() public getTipoDispositivo: any;
  @Input() public logotipo: any;
  @Input() public paroMotor: any;
  @Input() public infoDevice2: any = {};

  public latitud: number = 19.4525976;
  public longitud: number = -99.1182164;
  public nombre: string;
  public zoom: number = 11;
  public map: any;
  public fitBounds: boolean = true;
  public agmFitBounds: boolean = true;

  public role: string = localStorage.getItem('role');

  constructor(private mapService: MapService) { }

  ngOnChanges(): void {
    this.getGraphics(this.device);
  }
  
  
  getGraphics(device: string){
    
    this.mapService.getGraphicsDetails(device)
    .subscribe(data => {
      // Grafica Kilometros
      this.graficaKmData = data.Odo.data
      this.graficaKmLabel = data.Odo.label
      // Gráfica Tiempo
      this.graficaTimeData = data.Time.data
      this.graficaTimeLabel = data.Time.label
      // Gráfica Combustible
      this.graficaFuelData = data.Fuel.data
      this.graficaFuelLabel = data.Fuel.label
    })
  }


  viewParodeMotor(): boolean{
    if (this.infoDevice.EngineStop === 1 && this.infoDevice.TypeDevice === 8 && this.role == "Administrador") {
      return true;
    }else {
      return false;
    }
  }

}
