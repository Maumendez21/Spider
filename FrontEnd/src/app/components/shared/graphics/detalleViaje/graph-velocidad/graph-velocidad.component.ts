import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartPointOptions, ChartType, plugins } from 'chart.js';
import { Color, BaseChartDirective, Label } from 'ng2-charts';

import * as pluginAnnotations from 'chartjs-plugin-annotations';

@Component({
  selector: 'app-graph-velocidad',
  templateUrl: './graph-velocidad.component.html',
  styleUrls: ['./graph-velocidad.component.css']
})
export class GraphVelocidadComponent implements OnInit {

  constructor() { }

  @Input() public grafic: any = {};
  @Input() public lineChartLabels: Label[] = [];

  public data: Array<number> = [];
  // public limit: number = 0;

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.grafic.data.forEach((element) => {
        this.data.push( parseFloat(element))
    });

    // this.limit = this.grafic.MaximumSpeed
  }



  public lineChartData: ChartDataSets[] = [
    { data: this.data, label: 'Velocidad'}
  ];

  // @Input() public lineChartOptions: (ChartOptions & { annotation: any });
  public lineChartOptions: (ChartOptions & { annotation: any }) = {
    responsive: true,
    annotation:
    {
      annotations: [
        // [this.pluggins]
        {
          type: 'line',
          mode: 'horizontal',
          scaleID: 'y-axis-0',
          value: 100,
          borderColor: 'red',
          borderWidth: 3,
          label: {
            enabled: true,
            fontColor: 'white',
            content: 'Limite de velocidad'
          }
        }
      ],
    },

  };
  public lineChartColors: Color[] = [
    { // red
      backgroundColor: 'rgba(0,95,158,0.3)',
      borderColor: '#42A5F5',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    }
  ];
  public lineChartLegend = true;

  public lineChartPlugins = [pluginAnnotations];
  public lineChartType: ChartType = 'line';

}
