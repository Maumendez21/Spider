import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { SharedService } from 'src/app/services/shared.service';

@Component({
  selector: 'app-filter-estatus-main-map',
  templateUrl: './filter-estatus-main-map.component.html',
  styleUrls: ['./filter-estatus-main-map.component.css']
})
export class FilterEstatusMainMapComponent implements OnInit {

  estatus: string = "";
  type = "";

  estatusObserv: Subscription;
  typeObserv: Subscription;

  constructor(private shared: SharedService) {
    this.updateSelectEstatus();
  }

  ngOnInit(): void {
  }

  updateSelectEstatus() {
     this.estatusObserv = this.shared.filterEstatusStream$.subscribe(data => {

      this.estatus = data.estatus;
    });

    this.typeObserv = this.shared.filterTypeStream$.subscribe(data => {

      this.type = data.typeV;
    });




  }

  filtrarEstatus() {
    this.shared.broadcastFilterEstatusStream({
      estatus: this.estatus,
      search: true
    });
    this.shared.broadcastFiltertypeStream({
      typeV: this.type,
      search: true
    });




  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.

    this.estatusObserv.unsubscribe();
    this.typeObserv.unsubscribe();

  }

}
