import { Component, OnInit, Input } from '@angular/core';
import { combineLatest } from 'rxjs';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../../services/configuration.service';
import { InfoDeviceService } from '../../services/info-device.service';

@Component({
  selector: 'app-update-info-device',
  templateUrl: './update-info-device.component.html',
  styleUrls: ['./update-info-device.component.css']
})
export class UpdateInfoDeviceComponent implements OnInit {

  @Input() idSend: string;
  @Input() reloadTable: any;

  public device: string;
  public model: string;
  public bateryDuration: string = "";
  public batery: number= 0;
  public motorized: number= 0;


  public bateryIf: boolean;
  public motorizedIf: boolean;

  public performance: number = 0;

  public typeId: number = 0;
  public redId: number = 0;
  public timeId: number = 0;

  public typesDevice: {}
  public reds: {};
  public times: {};

  constructor(
    private configurationService: ConfigurationService,
    private infoDevice: InfoDeviceService
  ) { 
    this.getListsSelects();
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {

    this.getDeviceInfo(this.idSend);
  }

  getListsSelects(){
    combineLatest([
      this.infoDevice.getListRedInfoDevice(),
      this.infoDevice.getListTimeInfoDevice(),
      this.infoDevice.getTypeDevices()
    ]).subscribe(([red, time, types]) => {


      this.typesDevice = types;
      this.reds = red;
      this.times = time;

    })
  }

  getDeviceInfo(id: string): void {
    this.infoDevice.getInfoDispositivioId(id).subscribe(data => {
      this.device = data.Device;
      this.typeId = data.TypeDevice;
      this.redId = data.IdCommunicationMethod;
      this.timeId = data.IdSamplingTime;
      this.bateryIf = data.Batery;
      this.model = data.Model;
      this.motorizedIf = data.Motorized;
      this.bateryDuration = data.BatteryDuration;
      this.performance = data.Performance;
    })
  }

  actualizarInfoDispositivo(){

    if (this.bateryIf || this.motorizedIf) {
      this.batery = 1;
      this.motorized = 1;
    }else{
      this.batery = 0;
      this.motorized = 0;

    }

    const data = {
      Device: this.device,
      TypeDevice: this.typeId,
      Model: this.model,
      IdCommunicationMethod: this.redId,
      Batery: this.batery,
      BatteryDuration: this.bateryDuration,
      IdSamplingTime: this.timeId,
      Motorized: this.motorized,
      Performance: this.performance
    }

    this.infoDevice.postInfoDevice(data).subscribe(data => {

      if (data['success']) {
        Swal.fire('Guardado!', 'Datos guardados', 'success');
        this.reloadTable();
      }

    })

  }

}
