import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SharedService } from 'src/app/Services/shared.service';

@Component({
  selector: 'app-filter-status',
  templateUrl: './filter-status.component.html',
  styleUrls: ['./filter-status.component.css']
})
export class FilterStatusComponent implements OnInit {

  constructor(
    private shared: SharedService
  ) {
    this.updateSelectEstatus();
  }

  public estatus: string = "";
  public type = "";

  public estatusObserv: Subscription;
  public typeObserv: Subscription;

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

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.estatusObserv.unsubscribe();
    this.typeObserv.unsubscribe();


  }

}
