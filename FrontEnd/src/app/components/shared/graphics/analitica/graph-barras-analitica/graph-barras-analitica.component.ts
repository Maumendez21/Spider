import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';

import * as pluginDataLabels from 'chartjs-plugin-datalabels';

@Component({
  selector: 'app-graph-barras-analitica',
  templateUrl: './graph-barras-analitica.component.html',
  styleUrls: ['./graph-barras-analitica.component.css']
})
export class GraphBarrasAnaliticaComponent implements OnInit {

  @Input() data: ChartDataSets[];
  @Input() labels: Label[];
  @Input() colors;
  @Input() options : ChartOptions;

  barChartOptions: ChartOptions;
  barChartLabels: Label[] = [''];
  barChartType: ChartType = 'bar';
  barChartLegend = true;

  barChartData: ChartDataSets[] = [
    { data: [0], label: '' }
  ];

  chartColors: any;

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.barChartLabels = this.labels;
    this.barChartData = this.data;
    this.chartColors = this.colors;
    this.barChartOptions = this.options;
  }

}
