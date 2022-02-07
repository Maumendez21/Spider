import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-graph-rendimiento',
  templateUrl: './graph-rendimiento.component.html',
  styleUrls: ['./graph-rendimiento.component.css']
})
export class GraphRendimientoComponent implements OnInit {

  constructor() { }

  @Input() data: ChartDataSets[];
  @Input() labels: Label[];
  @Input() colors;
  @Input() options : ChartOptions;

  ngOnInit(): void {
  }

  barChartOptions: ChartOptions;
  barChartLabels: Label[] = [''];
  barChartType: ChartType = 'bar';
  public barChartLegend = true;

  barChartData: ChartDataSets[] = [
    { data: [0], label: '' }
  ];

  chartColors: any;

  ngOnChanges(): void {
    this.barChartLabels = this.labels;
    this.barChartData = this.data;
    this.chartColors = this.colors;
    this.barChartOptions = this.options;
  }

}
