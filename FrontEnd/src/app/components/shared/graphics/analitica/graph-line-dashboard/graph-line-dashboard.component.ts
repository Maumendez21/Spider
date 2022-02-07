import { Component, Input, OnInit } from '@angular/core';
import { Label } from 'ng2-charts';
import { ChartDataSets, ChartType, ChartOptions } from 'chart.js';
import { Color } from 'chartjs-plugin-datalabels/types/options';
import { data } from 'jquery';

@Component({
  selector: 'app-graph-line-dashboard',
  templateUrl: './graph-line-dashboard.component.html',
  styleUrls: ['./graph-line-dashboard.component.css']
})
export class GraphLineDashboardComponent implements OnInit {
  
  @Input() data: ChartDataSets[];
  @Input() labels: Label[];
  constructor() { }

  ngOnInit(): void {
  }

  public lineChartData: ChartDataSets[] = [
    { data: [0], label: ''}
  ];
  public lineChartLabels: Label[] = [''];
  public lineChartLegend = true;
  public lineChartType: ChartType = 'line';

  public barChartOptions: ChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{}] },
    legend: { position: 'bottom' },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
      }
    }
  };
  
  public lineChartColors = [{ backgroundColor: 'rgba(255,0,0,0.3)',
  borderColor: 'red',
  pointBackgroundColor: 'rgba(148,159,177,1)',
  pointBorderColor: '#fff',
  pointHoverBackgroundColor: '#fff',
  pointHoverBorderColor: 'rgba(148,159,177,0.8)' }];

  ngOnChanges(): void {
    this.lineChartLabels = this.labels;
    this.lineChartData = this.data;
    
  }

  
}
