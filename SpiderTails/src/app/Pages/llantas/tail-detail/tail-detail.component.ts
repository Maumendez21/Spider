import { Component, OnInit, ViewChild } from '@angular/core';
import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexTitleSubtitle,
  ApexStroke,
  ApexDataLabels,
  ApexMarkers,
  ApexYAxis,
  ApexGrid,
  ApexLegend,
  ApexTooltip,
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexFill
} from "ng-apexcharts";


export type ChartOptions = {
  series2: ApexNonAxisChartSeries;
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  stroke: ApexStroke;
  dataLabels: ApexDataLabels;
  markers: ApexMarkers;
  colors: string[];
  yaxis: ApexYAxis;
  grid: any;
  legend: ApexLegend;
  title: ApexTitleSubtitle;
  toolbar: any;
  tooltip: ApexTooltip;
  fill: ApexFill;
  plotOptions: ApexPlotOptions;
  labels: string[];
};
@Component({
  selector: 'app-tail-detail',
  templateUrl: './tail-detail.component.html',
  styleUrls: ['./tail-detail.component.css']
})
export class TailDetailComponent implements OnInit {

  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  public chartOptions2: Partial<ChartOptions>;
  public chartOptions3: Partial<ChartOptions>;
  public chartOptions4: Partial<ChartOptions>;

  public commonOptions: Partial<ChartOptions> = {
    dataLabels: {
      enabled: false
    },
    stroke: {
      curve: "straight"
    },
    toolbar: {
      tools: {
        selection: false
      }
    },
    markers: {
      size: 6,
      hover: {
        size: 10
      }
    },
    tooltip: {
      followCursor: false,
      theme: "dark",
      x: {
        show: false
      },
      marker: {
        show: false
      },
      y: {
        title: {
          formatter: function() {
            return "";
          }
        }
      }
    },
    grid: {
      clipMarkers: false
    },
    xaxis: {
      type: "datetime"
    }
  };

  constructor() {
    this.chartOptions = {
      series: [
        {
          name: "My-series",
          data: [10, 41, 35, 51, 49, 62, 69, 91, 148]
        }
      ],
      chart: {
        height: 350,
        type: "line"
      },
      title: {
        text: "Comportamiento"
      },
      xaxis: {
        categories: ["Jan", "Feb",  "Mar",  "Apr",  "May",  "Jun",  "Jul",  "Aug", "Sep"]
      }
      // series: [
      //   {
      //     name: "High - 2013",
      //     data: [28, 29, 33, 36, 32, 32, 33]
      //   },
      // ],
      // chart: {
      //   height: 350,
      //   type: "line",
      //   dropShadow: {
      //     enabled: true,
      //     color: "#000",
      //     top: 18,
      //     left: 7,
      //     blur: 10,
      //     opacity: 0.2
      //   },
      //   toolbar: {
      //     show: false
      //   }
      // },
      // colors: ["#77B6EA"],
      // dataLabels: {
      //   enabled: true
      // },
      // stroke: {
      //   curve: "smooth"
      // },
      // title: {
      //   text: "Comportamiento",
      //   align: "left"
      // },
      // grid: {
      //   borderColor: "#e7e7e7",
      //   row: {
      //     colors: ["#f3f3f3", "transparent"], // takes an array which will be repeated on columns
      //     opacity: 0.5
      //   }
      // },
      // markers: {
      //   size: 1
      // },
      // xaxis: {
      //   categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul"],
      //   title: {
      //     text: "Month"
      //   }
      // },
      // yaxis: {
      //   title: {
      //     text: "Temperature"
      //   },
      //   min: 5,
      //   max: 40
      // },
      // legend: {
      //   position: "top",
      //   horizontalAlign: "right",
      //   floating: true,
      //   offsetY: -25,
      //   offsetX: -5
      // }
    };

    // this.chartOptions2 = {
    //   series: [
    //     {
    //       name: "Comportamiento",
    //       data: this.generateDayWiseTimeSeries(
    //         new Date("11 Feb 2017").getTime(),
    //         20,
    //         {
    //           min: 10,
    //           max: 60
    //         }
    //       )
    //     }
    //   ],
    //   title: {
    //     text: "Comportamiento",
    //     align: "left"
    //   },
    //   chart: {
    //     id: "yt",
    //     group: "social",
    //     type: "area",
    //     height: 160
    //   },
    //   colors: ["#00E396"],
    //   yaxis: {
    //     tickAmount: 2,
    //     labels: {
    //       minWidth: 40
    //     }
    //   }
    // };

    this.chartOptions3 = {
      series2: [75],
      chart: {
        height: 350,
        type: "radialBar",
        toolbar: {
          show: true
        }
      },
      plotOptions: {
        radialBar: {
          startAngle: -135,
          endAngle: 225,
          hollow: {
            margin: 0,
            size: "85%",
            background: "#fff",
            image: undefined,
            position: "front",
            dropShadow: {
              enabled: true,
              top: 3,
              left: 0,
              blur: 4,
              opacity: 0.24
            }
          },
          track: {
            background: "#fff",
            strokeWidth: "67%",
            margin: 0, // margin is in pixels
            dropShadow: {
              enabled: true,
              top: -3,
              left: 0,
              blur: 4,
              opacity: 0.35
            }
          },

          dataLabels: {
            show: true,
            name: {
              offsetY: -10,
              show: true,
              color: "#888",
              fontSize: "17px"
            },
            value: {
              formatter: function(val) {
                return parseInt(val.toString(), 10).toString();
              },
              color: "#111",
              fontSize: "36px",
              show: true
            }
          }
        }
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "dark",
          type: "horizontal",
          shadeIntensity: 0.5,
          gradientToColors: ["#ABE5A1"],
          inverseColors: true,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 100]
        }
      },
      stroke: {
        lineCap: "round"
      },
      labels: ["Presión Actual"]
    };
    this.chartOptions4 = {
      series2: [40],
      chart: {
        height: 350,
        type: "radialBar",
        toolbar: {
          show: true
        }
      },
      plotOptions: {
        radialBar: {
          startAngle: -135,
          endAngle: 225,
          hollow: {
            margin: 0,
            size: "85%",
            background: "#fff",
            image: undefined,
            position: "front",
            dropShadow: {
              enabled: true,
              top: 3,
              left: 0,
              blur: 4,
              opacity: 0.24
            }
          },
          track: {
            background: "#fff",
            strokeWidth: "67%",
            margin: 0, // margin is in pixels
            dropShadow: {
              enabled: true,
              top: -3,
              left: 0,
              blur: 4,
              opacity: 0.35
            }
          },

          dataLabels: {
            show: true,
            name: {
              offsetY: -10,
              show: true,
              color: "#888",
              fontSize: "17px"
            },
            value: {
              formatter: function(val) {
                return parseInt(val.toString(), 10).toString();
              },
              color: "#111",
              fontSize: "36px",
              show: true
            }
          }
        }
      },
      fill: {
        type: "gradient",
        gradient: {
          shade: "dark",
          type: "horizontal",
          shadeIntensity: 0.5,
          gradientToColors: ["#fd1d1d"],
          inverseColors: false,
          opacityFrom: 1,
          opacityTo: 1,
          stops: [0, 100]
        }
      },
      stroke: {
        lineCap: "round"
      },
      labels: ["Temperatura en %"]
    };







  }

  public generateDayWiseTimeSeries(baseval, count, yrange): any[] {
    let i = 0;
    let series = [];
    while (i < count) {
      var x = baseval;
      var y =
        Math.floor(Math.random() * (yrange.max - yrange.min + 1)) + yrange.min;

      series.push([x, y]);
      baseval += 86400000;
      i++;
    }
    return series;
  }

  ngOnInit(): void {
  }

 

}
