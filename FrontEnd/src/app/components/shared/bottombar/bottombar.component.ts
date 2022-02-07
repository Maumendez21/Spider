import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import * as moment from 'moment-timezone';

@Component({
  selector: 'app-bottombar',
  templateUrl: './bottombar.component.html',
  styleUrls: ['./bottombar.component.css']
})
export class BottombarComponent implements OnInit {

  show: boolean = true;
  lastTravels: any = [];

  constructor(private shared: SharedService, private spider: SpiderfleetService, private router: Router) {
    this.verifyLogged();
  }

  ngOnInit(): void {
  }

  verifyLogged() {

    this.shared.loggedStream$.subscribe(data => {
      this.show = data;
      if (data) {
        this.showBottomBar();
      }
    });

    if (!this.show) {
      this.shared.broadcastLoggedStream((localStorage.getItem("token") ? true : false ));
    }

  }

  showBottomBar() {
    this.shared.zoomCoordsStream$.subscribe(data => {

      this.show = data['bottom'];

      if (data['bottom'] && !data['filterBottom']) {

        this.spider.getLastTravelsDevice(data['device'])
          .subscribe(data => {
            this.lastTravels = data;
          });

      } else if (data['bottom'] && data['filterBottom']) {

        this.spider.getAllRoutesPerDate(data['device'], data['startDate'], data['endDate'])
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

    this.spider.getRouteDevice(device, startDate, endDate)
      .subscribe(data => {
        console.log(data);

        data['device'] = device;
        data['startDate'] = startDate;
        data['endDate'] = endDate;
        this.shared.broadcastRouteDirectionStream(data);
      });
  }

  showDetailTravel(device: string, startDate: string, endDate: string, index: number): string {

    // if (index+1 <= this.lastTravels.length-1) {
    //   startDate = (this.diffTimeTravelStartEnd(startDate, this.lastTravels[index+1]['EndDate'], 30)) ? this.lastTravels[index+1]['EndDate'] : startDate;
    // }

    return `/trip/${device}/${startDate}/${endDate}`;
  }

  diffTimeTravelStartEnd(start: string, end: string, minutes: number): boolean {

    start = start.replace('T', ' ');
    end = end.replace('T', ' ');

    start = start.replace('Z', '');
    end = end.replace('Z', '');

    const startMoment = moment(start);
    const endMoment = moment(end);

    const durationMoment = moment.duration(startMoment.diff(endMoment));
    const minutesMoment = durationMoment.asMinutes();

    return (minutesMoment < minutes);
  }

  hideBottomBar() {
    this.show = false;
    this.lastTravels = [];
  }

}
