import { Component, OnInit } from '@angular/core';
import { MobilityService } from '../services/mobility.service';

@Component({
  selector: 'app-rutes',
  templateUrl: './rutes.component.html',
  styleUrls: ['./rutes.component.css']
})
export class RutesComponent implements OnInit {

  public rutas: any = [];
  public pageOfItems:Array<any>;
  public loading: boolean = true;
  constructor(
    private mobilityService: MobilityService
  ) { 
    this.getRoutes();
  }

  async getRoutes() {

    this.loading = true;
    await this.mobilityService.getListRutasConfiguracion()
      .subscribe(rutas => {

        this.rutas = rutas;
        this.loading= false;
      });
  }

  ngOnInit(): void {
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  refresh(event: any){
    if (event) {
      this.getRoutes();
    }
  }

}
