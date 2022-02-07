import { Component, Input, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../../../services/spiderfleet.service';
import { Label } from 'ng2-charts';
import { ChartDataSets } from 'chart.js';
import { SharedService } from '../../../../../services/shared.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard-nav',
  templateUrl: './dashboard-nav.component.html',
  styleUrls: ['./dashboard-nav.component.css']
})
export class DashboardNavComponent implements OnInit {
  devices: any = [];
  subempresas: any = [];

  subempresa: string = "";
  vehiculo: string = "";

  analiticaGraficaService: any = [];

  // FECHA ACTUAL
  dia = '';
  mes = '';
  date = '';
  anio = '';

  yearStart: any;
  week: number;

  semana = ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'];
  meses = ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio','Agosto','Septiembre','Octube','Noviembre','Diciembre'];

  // ENDPOINT
  unidadesActivos: any;
  totalDistacia: any;
  totalTiempo: any;

  graficaActividadLabel: Label[] = [''];
  graficaActividadData: ChartDataSets[] = [
    { data: [0], label: 'Actividad' }
  ];

  graficaCombustibleLabel: Label[] = [''];
  graficaCombustibleData: ChartDataSets[] = [
    { data: [0], label: 'Combustible' }
  ];

  enableGrupo: boolean = false;
  enableDispositivo: boolean = false;
  loading = false;
  loadingCombus = false;

  activateForms(estatus: boolean) {

    this.enableGrupo = estatus;
    this.enableDispositivo = estatus;
  }


  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    if (shared.verifyLoggin()) {
      this.getDevices();
      this.getSubempresas();
      this.fecha();
      this.getWeek(new Date());

      this.shared.analiticasGeneralStream$.subscribe(data => {
        if (data.length > 0) {
          this.setDashboard(data[0].dashboard);
          this.setGraphCombustible(data[1].combustible);
        } else {
          this.getDashboard();
        }
      });
    } else {
      this.router.navigate(['/login']);
    }



   }
  getDevices() {
    this.spiderService.getDevicesGeneral("Flota", "")
      .subscribe(data => {
        this.devices = data;
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

  async getDashboard(){
    this.activateForms(true);
    this.loading = true;
    await this.spiderService.getDashboard(localStorage.getItem('idu'))
    .subscribe(data => {
      this.analiticaGraficaService.push({ dashboard: data });
      this.getGraphCombustible();
    })
  }

  setDashboard(data: any) {
      this.unidadesActivos = data['TotalActivas'];
      this.totalDistacia = data['TotalDistancia'];
      this.totalTiempo = data['TotalTiempo'];

      this.graficaActividadData = [
        {data: data['Graficas']['data'], label:"Actividad"}
      ];

      this.graficaActividadLabel = data['Graficas']['label'];
      this.activateForms(false);
      this.loading = false;
  }

  async getGraphCombustible(){

      this.loadingCombus = true;
      this.activateForms(true);

      await this.spiderService.getGraphCombustible(this.subempresa, this.vehiculo)
      .subscribe(data => {
        this.analiticaGraficaService.push({ combustible: data });
        this.shared.broadcastAnaliticasGeneralStream(this.analiticaGraficaService);
      })
  }

  setGraphCombustible(data: any) {
    this.graficaCombustibleData = [
      {data: data['data'], label:"Combustible"}
    ];
    this.graficaCombustibleLabel = data['label'];
    this.loadingCombus = false;
    this.activateForms(false);
    this.vehiculo = "";
  }

  getWeek(date: any){
      date = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
      date.setUTCDate(date.getUTCDate() + 4 - (date.getUTCDay()||7));
      this.yearStart = new Date(Date.UTC(date.getUTCFullYear(),0,1));
      let weekNo = Math.ceil(( ( (date - this.yearStart) / 86400000) + 1)/7);
      this.week = weekNo;
  }

  fecha(){
    let data = new Date();
    this.date = data.getDate().toString();
    this.dia = this.semana[data.getDay()];
    this.mes = this.meses[data.getMonth()];
    this.anio = data.getUTCFullYear().toString();
  }
}
