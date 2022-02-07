import { Component, OnInit, Input } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-graph-km-recorridos',
  templateUrl: './graph-km-recorridos.component.html',
  styleUrls: ['./graph-km-recorridos.component.css']
})
export class GraphKmRecorridosComponent {

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
  public colors = [{ backgroundColor: '#0acf97' }];

  ngOnChanges(): void {
    this.barChartData = [{
      data: this.data, label: 'Kilometros'
    }]
    this.barChartLabels = this.labels;
    
  }

  

  
}
