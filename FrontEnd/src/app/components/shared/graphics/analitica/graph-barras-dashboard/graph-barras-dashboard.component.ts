import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartType, ChartOptions } from 'chart.js';
import { Label } from 'ng2-charts';
import { data } from 'jquery';

@Component({
  selector: 'app-graph-barras-dashboard',
  templateUrl: './graph-barras-dashboard.component.html',
  styleUrls: ['./graph-barras-dashboard.component.css']
})
export class GraphBarrasDashboardComponent implements OnInit {
  
  @Input() data: ChartDataSets[];
  @Input() labels: Label[];

  constructor() { }

  ngOnInit(): void {
  }
  public barChartData: ChartDataSets[] = [
    
    { data: [0], label: '' }
  ];
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
  public barChartLabels: Label[] = [''];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  
  chartColors = [{backgroundColor: '#1AB394'}]

  ngOnChanges(): void {
    this.barChartData = this.data;
    this.barChartLabels = this.labels;
    
  }


}
