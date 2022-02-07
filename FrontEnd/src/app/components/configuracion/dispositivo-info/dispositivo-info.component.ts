import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-dispositivo-info',
  templateUrl: './dispositivo-info.component.html',
  styleUrls: ['./dispositivo-info.component.css']
})
export class DispositivoInfoComponent implements OnInit {

  constructor(private spiderService: SpiderfleetService) { }

  ngOnInit(): void {
    this.getDispositivos(this.searchDevice);
  }

  public searchDevice: string = '';
  public devices: any[] = [];
  public id: string;
  public pageOfItems: Array<any>;


  getIdDevice(id: string){
    this.id = id;
  }

  getDispositivos =(device: string = "")=>{
    this.spiderService.getListDispositivosInfo(device)
    .subscribe(data =>{
      this.devices = data;
    })
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
