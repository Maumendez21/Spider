import { Component, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ConfigurationService } from '../../Pages/configuration/services/configuration.service';
import { SpiderService } from '../../Services/spider.service';

@Component({
  selector: 'app-modal-report',
  templateUrl: './modal-report.component.html',
  styleUrls: ['./modal-report.component.css']
})
export class ModalReportComponent implements OnInit {

  usuario: number;
  devices: any = [];
  subempresas: any = [];

  grupo: string = "";
  unidad: string = "";
  fechaInicio: string;
  fechaFin: string;
  horaInicio: string = "00:00";
  horaFin: string = "23:59";
  itinerarios: string = "1";

  constructor(
    private datePipe: DatePipe,
    private configurationService: ConfigurationService,
    private spiderService: SpiderService
  ) { 
    
  }

  ngOnInit(): void {
    this.initReportes();
  }

  initReportes() {

    this.getListDevices();
    this.getSubempresas();
    this.grupo = "";
    this.unidad = "";

    this.fechaInicio = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
    this.fechaFin = this.datePipe.transform(new Date(), 'yyyy-MM-dd');
  }

  ngOnChanges(): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.
    
    
    this.initReportes();
  }


  getListDevices() {
    this.configurationService.getListDevicesConfiguration()
      .subscribe(data => {
        this.devices = data;
      });
  }

  getSubempresas() {
    this.spiderService.getSubempresas()
      .subscribe(data => {
        this.subempresas = data;
      });
  }

  report: string;

  generarReporte() {
    if (this.grupo != "" && this.grupo != null) {
      const idu = localStorage.getItem("idu");
      const company = localStorage.getItem("company");
      return `https://spiderfleetapi.azurewebsites.net/api/reporte/viajes/mongo/zip?empresa=${company}&param=${idu}&type=${this.itinerarios}&grupo=${this.grupo}&device=${this.unidad}&fechainicio=${this.fechaInicio + " " + this.horaInicio}&fechafin=${this.fechaFin + " " + this.horaFin}`;
      
      
    }
  }

  updateSubempresa() {
    this.unidad = "";
  }

}
