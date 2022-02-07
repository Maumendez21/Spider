import { Component, OnInit } from '@angular/core';
import { MapService } from 'src/app/Pages/home/Services/map.service';
import { SharedService } from '../../Services/shared.service';

@Component({
  selector: 'app-right-menu',
  templateUrl: './right-menu.component.html',
  styleUrls: ['./right-menu.component.css']
})
export class RightMenuComponent implements OnInit {

  constructor(
    private shared: SharedService,
    private mapService: MapService
  ) {
    this.showBottomBar();
  }


  public show: boolean = true;
  public lastTravels: any = [];
  public title: string = "";
  public tipe: boolean = true;



  ngOnInit(): void {
  }




  showBottomBar() {

    this.shared.rightMenu$.subscribe(data => {

      this.title = data['title'];
      this.tipe = data['tipe'];

    })


    this.shared.zoomCoordsStream$.subscribe(data => {

      this.show = data['bottom'];

      if (data['bottom'] && !data['filterBottom']) {

        this.mapService.getLastTravelsDevice(data['device'])
          .subscribe(data => {
            this.lastTravels = data;
          });

      } else if (data['bottom'] && data['filterBottom']) {

        this.mapService.getAllRoutesPerDate(data['device'], data['startDate'], data['endDate'])
          .subscribe(data => {
            this.lastTravels = data;
          });

      } else {
        this.lastTravels = [];
      }


    });
  }

  showTravel(device: string, startDate: string, endDate: string, index: number) {

    // if (index+1 <= this.lastTravels.length-1) {
    //   startDate = (this.diffTimeTravelStartEnd(startDate, this.lastTravels[index+1]['EndDate'], 30)) ? this.lastTravels[index+1]['EndDate'] : startDate;
    // }

    // this.spider.getRouteDevice(device, startDate, endDate)
    //   .subscribe(data => {
    //     console.log(data);

    //     data['device'] = device;
    //     data['startDate'] = startDate;
    //     data['endDate'] = endDate;
    //     this.shared.broadcastRouteDirectionStream(data);
    //   });
  }

  hideBottomBar() {
    this.show = false;
    this.lastTravels = [];
  }

}
