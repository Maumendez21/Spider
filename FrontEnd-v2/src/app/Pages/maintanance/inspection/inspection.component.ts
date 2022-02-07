import { Component, OnInit } from '@angular/core';
import { MaitananceService } from '../services/maitanance.service';

@Component({
  selector: 'app-inspection',
  templateUrl: './inspection.component.html',
  styleUrls: ['./inspection.component.css']
})
export class InspectionComponent implements OnInit {


  public inspecciones: any = [];
  public pageOfItems:Array<any>;
  constructor(
    private maitanaceService: MaitananceService
  ) { 
    this.getInspecciones();
  }

  getInspecciones(){
    this.maitanaceService.getInspeccionList().subscribe(data => {
      this.inspecciones = data;
    })
  }

  ngOnInit(): void {
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }


}
