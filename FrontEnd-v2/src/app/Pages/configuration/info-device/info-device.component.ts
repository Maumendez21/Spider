import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../services/configuration.service';
import { InfoDeviceService } from '../services/info-device.service';

@Component({
  selector: 'app-info-device',
  templateUrl: './info-device.component.html',
  styleUrls: ['./info-device.component.css']
})
export class InfoDeviceComponent implements OnInit {

  constructor(
    private infoDevice: InfoDeviceService
  ) { }

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
    this.infoDevice.getListDispositivosInfo(device)
    .subscribe(data =>{
      this.devices = data;
    })
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
