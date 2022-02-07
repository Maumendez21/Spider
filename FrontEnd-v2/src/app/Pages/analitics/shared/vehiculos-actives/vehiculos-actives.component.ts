import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AnaliticService } from '../../services/analitic.service';

@Component({
  selector: 'app-vehiculos-actives',
  templateUrl: './vehiculos-actives.component.html',
  styleUrls: ['./vehiculos-actives.component.css']
})
export class VehiculosActivesComponent implements OnInit {

  @Input() data: any[];

  constructor(
    private analiticService: AnaliticService,
    private router: Router,
  ) { }
  deviceSearch: string = "";

  ngOnInit(): void {
  }

  showDetailTravel(device: string, startDate: string, endDate: string): string {
    // if (index+1 <= this.lastTravels.length-1) {
    //   startDate = (this.diffTimeTravelStartEnd(startDate, this.lastTravels[index+1]['EndDate'], 30)) ? this.lastTravels[index+1]['EndDate'] : startDate;
    // }
    console.log('dhjdh');
    this.router.navigateByUrl(`/home/trip/${device}/${startDate}/${endDate}`)

    return `/home/trip/${device}/${startDate}/${endDate}`;
  }

  pageOfItems:Array<any>;

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  buscarDevice(){
    // this.getActivityDat(this.deviceSearch)
    this.analiticService.getActivityDay(this.deviceSearch).subscribe(data => {
      this.data = data['ListDay'];
    })
  }

}
