import { Component, OnInit, Input } from '@angular/core';
import { ChartOptions, ChartDataSets, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-graph-fuel-consume',
  templateUrl: './graph-fuel-consume.component.html',
  styleUrls: ['./graph-fuel-consume.component.css']
})
export class GraphFuelConsumeComponent {

  constructor() { }

  @Input() data: number[];
  @Input() labels: any[];

  public barChartOptions: ChartOptions = {
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
  public barChartData: ChartDataSets[] = [
    { data: [0], label: '' },
  ];
  public barChartLabels: Label[] = [''];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public colors = [{ backgroundColor: '#727cf5' }];

  ngOnChanges(): void {
    
    
    this.barChartData = [{
      data: this.data, label: 'Litros'
    }]
    this.barChartLabels = this.labels;
    
  }

}
