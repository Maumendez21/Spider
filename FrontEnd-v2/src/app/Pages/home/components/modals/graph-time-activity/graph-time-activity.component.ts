import { Component, OnInit, Input } from '@angular/core';
import { ChartOptions, ChartDataSets, ChartType } from 'chart.js';
import { Label } from 'ng2-charts';

@Component({
  selector: 'app-graph-time-activity',
  templateUrl: './graph-time-activity.component.html',
  styleUrls: ['./graph-time-activity.component.css']
})
export class GraphTimeActivityComponent  {

  constructor() { }

  @Input() data: number[];
  @Input() labels: any[];

  public barChartOptions: ChartOptions = {
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


  public barChartData: ChartDataSets[] = [
    { data: [0], label: '' },
  ];
  public barChartLabels: Label[] = [''];
  public barChartType: ChartType = 'bar';
  public barChartLegend = true;
  public colors = [{ backgroundColor: '#39afd1' }];

  ngOnChanges(): void {
    
    this.barChartData = [{
      data: this.data, label: 'hh:min:ss'
    }]
    this.barChartLabels = this.labels;
    
  }
}
