import { Component, Input, OnInit } from '@angular/core';
import { SharedService } from '../../../Services/shared.service';
import { MapService } from '../../../Pages/home/Services/map.service';

@Component({
  selector: 'app-travels',
  templateUrl: './travels.component.html',
  styleUrls: ['./travels.component.css']
})
export class TravelsComponent implements OnInit {

  constructor(
    private shared: SharedService,
    private mapServices: MapService,
  ) { }
  @Input() lastTravels: any = [];

  ngOnInit(): void {
  }


  showTravel(device: string, startDate: string, endDate: string, index: number) {
    // if (index+1 <= this.lastTravels.length-1) {
    //   startDate = (this.diffTimeTravelStartEnd(startDate, this.lastTravels[index+1]['EndDate'], 30)) ? this.lastTravels[index+1]['EndDate'] : startDate;
    // }
    this.mapServices.getRouteDevice(device, startDate, endDate)
      .subscribe(data => {
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

    return `/home/trip/${device}/${startDate}/${endDate}`;
  }

}
