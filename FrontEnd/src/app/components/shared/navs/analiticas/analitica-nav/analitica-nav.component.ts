import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Label } from 'ng2-charts';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import moment from 'moment';

@Component({
  selector: 'app-analitica-nav',
  templateUrl: './analitica-nav.component.html',
  styleUrls: ['./analitica-nav.component.css']
})
export class AnaliticaNavComponent implements OnInit {

  spiderMarkers: any = [];
  subempresas: any = [];

  dateWeek: string = "";
  fechaInicio: string = "";
  fechaInicioTemp: boolean = false;
  fechaFin: string = "";
  fechaFinTemp: boolean = false;
  subempresa: string = "";
  vehiculo: string = "";





  totalHoras: string;
  totalKm: string;
  totalLitros: string;
  totalRendimiento: string;

  enableFecha: boolean = false;
  btnPDFEXCEL: boolean = false;
  enableGrupo: boolean = false;
  enableDispositivo: boolean = true;
  enableLoading = false;

  graficaHorasLabel: Label[] = [''];
  graficaKmLabel: Label[] = [''];
  graficaLitrosLabel: Label[] = [''];
  graficaRendimientoLabel: Label[] = [''];

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

  colorGraficaHoras = [{ backgroundColor: '#d32f2f' }];
  colorGraficaKm = [{ backgroundColor: '#ffa726' }];
  colorGraficaLitros = [{ backgroundColor: '#8d6e63' }];
  colorGraficaRendimiento = [{ backgroundColor: '#0097a7' }];

  barChartOptionsHoras: ChartOptions = {
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

  barChartOptionsKm: ChartOptions = {
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

  barChartOptionsLitros: ChartOptions = {
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

  barChartOptionsRendimiento: ChartOptions = {
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

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService) {
    this.limpiarFiltrosMapa();


    if (shared.verifyLoggin()) {
      this.getDevices();
      this.getSubempresas();
    } else {
      this.router.navigate(['/login']);
    }
  }


  rankingsBest: any[];
  rankingsLow: any[];

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
  idu: string = localStorage.getItem('idu')
  generarReporte() {

      const idu = localStorage.getItem("idu");
      // console.log(this.fechaInicio);

      return `http://spiderfleetapi.azurewebsites.net/api/dashboard/report/analitica?param=${idu}&grupo=${this.subempresa}&device=${this.vehiculo}&fechainicio=${this.fechaInicio}&fechafin=${this.fechaFin}`


  }

  ngOnInit(): void {
  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  updateGrupo() {
    this.getAnaliticas();
  }

  updateVehiculo() {
    this.getAnaliticas();
  }

  updateFechas() {
    const weak = +this.dateWeek.substring(6); // - 1
    const year = this.dateWeek.substring(0, this.dateWeek.length - 4);

    //add(1, 'd')

    this.fechaInicio = moment(year).add(weak, 'weeks').startOf('week').locale("es").format('YYYY-MM-DD');
    this.fechaFin = moment(year).add(weak, 'weeks').endOf('week').locale("es").format('YYYY-MM-DD');
  }

  activateForms(estatus: boolean) {
    this.enableFecha = estatus;
    this.enableGrupo = estatus;
    this.enableDispositivo = estatus;
  }

  fechaInicioInvalid:  boolean = false;

  validateDate(): boolean{
    this.fechaInicioTemp =  moment(this.fechaInicio).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaFinTemp =  moment(this.fechaFin).isAfter(moment().format('YYYY-MM-DD'));
    this.fechaInicioInvalid = moment(this.fechaInicio).isAfter(moment(this.fechaFin));
    return ((!this.fechaInicioTemp && !this.fechaFinTemp) && (!this.fechaInicioInvalid)) ? true : false
  }


  async getAnaliticas() {
    if ((this.fechaInicio != "" &&  this.fechaFin != "" && this.subempresa != "") ){
      if (this.validateDate()) {
        this.enableLoading = true;
        this.activateForms(true);
        await this.spiderService.getAnalitcs(this.fechaInicio, this.fechaFin, this.subempresa, this.vehiculo)
        .subscribe(data => {
          this.rankingsBest = data['ListRankingBest'];
          this.rankingsLow = data['ListRankingLower'];

          this.totalHoras = data['TotalTiempo'];
          this.totalKm = data['TotalDistancia'];
          this.totalLitros = data['TotalLitros'];
          this.totalRendimiento = data['TotalRendimiento'];

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
        this.toastr.warning("Fechas invalidas.");
        this.btnPDFEXCEL = false;
      }
    } else {
      this.toastr.warning("Es necesario seleccionar rango de Fechas y el grupo.");
    }
  }
}
