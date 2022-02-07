import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChartDataSets, ChartOptions } from 'chart.js';
import moment from 'moment';
import { Label } from 'ng2-charts';
import { SharedService } from 'src/app/Services/shared.service';
import { SpiderService } from 'src/app/Services/spider.service';
import Swal from 'sweetalert2';
import { AnaliticService } from '../services/analitic.service';

@Component({
  selector: 'app-perfomance',
  templateUrl: './perfomance.component.html',
  styleUrls: ['./perfomance.component.css']
})
export class PerfomanceComponent implements OnInit {

  constructor(
    private spiderService: SpiderService,
    private shared: SharedService,
    private router: Router,
    private analiticService: AnaliticService,
  ) {
    this.getDevices();
    this.getSubempresas();
  }

  idu: string = localStorage.getItem('idu')

  public fechaInicio: string = "";
  public fechaFin: string = "";

  public fechaInicioInvalid:  boolean = false;
  public fechaInicioTemp: boolean = false;
  public fechaFinTemp: boolean = false;

  public subempresas: any = [];
  public spiderMarkers: any = [];

  public subempresa: string = "";
  public vehiculo: string = "";
  public enableGrupo: boolean = false;


  public rankingsBest: any[];
  public rankingsLow: any[];

  public totalLitros: string;
  public totalRendimiento: string;

  public enableFecha: boolean = false;
  public enableDispositivo: boolean = true;
  public enableLoading = false;
  public btnPDFEXCEL: boolean = false;

  public totalHoras: string;
  public totalKm: string;

  /* GRAPHICS */

  graficaHorasData: ChartDataSets[] = [
    { data: [0], label: 'Horas' }
  ];
  graficaKmData: ChartDataSets[] = [
    { data: [0], label: 'Kilometros' }
  ];
  graficaLitrosData: ChartDataSets[] = [
    { data: [0], label: 'Litros' }
  ];
  graficaRendimientoData: ChartDataSets[] = [
    { data: [0], label: 'Rendimiento' }
  ];

  public graficaHorasLabel: Label[] = [''];
  public graficaKmLabel: Label[] = [''];
  public graficaLitrosLabel: Label[] = [''];
  public graficaRendimientoLabel: Label[] = [''];

  public colorGraficaHoras = [{ backgroundColor: '#727cf5' }];
  public colorGraficaKm = [{ backgroundColor: '#39afd1' }];
  public colorGraficaLitros = [{ backgroundColor: '#fa5c7c' }];
  public colorGraficaRendimiento = [{ backgroundColor: '#0acf97' }];

  public barChartOptionsHoras: ChartOptions = {
    responsive: true,
    legend: { position: 'bottom' },
    scales: { xAxes: [{}], yAxes: [{
      ticks: {
        callback: function(value, index, values) {
          return value.toString().replace(/\B(?=(\d{2})+(?!\d))/g, ":");
        }
      }
    }] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    },
    tooltips: {
      callbacks: {
        label: function(t, d) {
          var xLabel = d.datasets[t.datasetIndex].label;
          var yLabel = t.yLabel.toString().replace(/\B(?=(\d{2})+(?!\d))/g, ":");
          return xLabel + ': ' + yLabel;
        }
      }
    },
  };

  public barChartOptionsKm: ChartOptions = {
    responsive: true,
    legend: { position: 'bottom' },
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    },
  };

  public barChartOptionsLitros: ChartOptions = {
    responsive: true,
    legend: { position: 'bottom' },
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    },
  };

  public barChartOptionsRendimiento: ChartOptions = {
    responsive: true,
    legend: { position: 'bottom' },
    scales: { xAxes: [{}], yAxes: [{}] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    },
  };

  activateForms(estatus: boolean) {
    this.enableFecha = estatus;
    this.enableGrupo = estatus;
    this.enableDispositivo = estatus;
  }

  validateDate(): boolean{
    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaInicioInvalid = moment(this.fechaInicio).isAfter(moment(this.fechaFin));
    return ((!this.fechaInicioTemp && !this.fechaFinTemp) && (!this.fechaInicioInvalid)) ? true : false
  }


  ngOnInit(): void {
  }

  getDevices() {
    this.enableDispositivo = false;
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

  generarReporte() {
    const idu = localStorage.getItem("idu");
    return `http://spiderfleetapi.azurewebsites.net/api/dashboard/report/analitica?param=${idu}&grupo=${this.subempresa}&device=${this.vehiculo}&fechainicio=${this.fechaInicio}&fechafin=${this.fechaFin}`
  }


  async getAnaliticas() {
    if ((this.fechaInicio != "" &&  this.fechaFin != "" && this.subempresa != "") ){
      if (this.validateDate()) {
        this.enableLoading = true;
        this.activateForms(true);
        await this.analiticService.getAnalitcs(this.fechaInicio, this.fechaFin, this.subempresa, this.vehiculo)
        .subscribe(data => {
          this.rankingsBest = data['ListRankingBest'];
          this.rankingsLow = data['ListRankingLower'];

          this.totalHoras = data['TotalTiempo'];
          this.totalKm = data['TotalDistancia'];
          this.totalLitros = data['TotalLitros'];
          this.totalRendimiento = data['TotalRendimiento'];
          console.log(data['graficas']['graficaTiempo']['data']);
          

          this.graficaHorasData = [
            { data: data['graficas']['graficaTiempo']['data'], label: "Horas" }
          ];

          this.graficaKmData = [
            { data: data['graficas']['graficaDistancia']['data'], label: "Kilometros" }
          ];

          this.graficaLitrosData = [
            { data: data['graficas']['graficaLitros']['data'], label: "Litros" }
          ];

          this.graficaRendimientoData = [
            { data: data['graficas']['graficaRendimiento']['data'], label: "Rendimiento" }
          ];

          this.graficaHorasLabel = data['graficas']['graficaTiempo']['label']
          this.graficaKmLabel = data['graficas']['graficaDistancia']['label']
          this.graficaLitrosLabel = data['graficas']['graficaLitros']['label'];
          this.graficaRendimientoLabel = data['graficas']['graficaRendimiento']['label'];
          this.activateForms(false);
          this.enableLoading = false;
          this.btnPDFEXCEL = true;
        });
      }else {
        Swal.fire('Ups! :/', 'Fechas invalidas', 'warning')
        this.btnPDFEXCEL = false;
      }
    } else {
      Swal.fire('Cuidado!', 'Es necesario seleccionar rango de Fechas y el grupo.', 'warning')
    }
  }

}
