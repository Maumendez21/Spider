import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions, ChartType } from 'chart.js';
import { Label, Color } from 'ng2-charts';

import * as pluginAnnotations from 'chartjs-plugin-annotations';

@Component({
  selector: 'app-velocity-graphic',
  templateUrl: './velocity-graphic.component.html',
  styleUrls: ['./velocity-graphic.component.css']
})
export class VelocityGraphicComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    //Called before any other lifecycle hook. Use it to inject dependencies, but avoid any serious work here.
    //Add '${implements OnChanges}' to the class.

    this.grafic.data.forEach((element) => {
      this.data.push( parseFloat(element))
  });

  }

  @Input() public grafic: any = {};
  @Input() public lineChartLabels: Label[] = [];

  public data: Array<number> = [];

  public lineChartData: ChartDataSets[] = [
    { data: this.data, label: 'Velocidad'}
  ];

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
      borderColor: '#727cf5',
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
