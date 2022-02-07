import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { combineLatest } from 'rxjs';

@Component({
  selector: 'app-update-device-info',
  templateUrl: './update-device-info.component.html',
  styleUrls: ['./update-device-info.component.css']
})
export class UpdateDeviceInfoComponent implements OnInit, OnChanges {

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

  public typesDevice: any[];
  public reds: any[];
  public times: any[];





  constructor(private spiderService: SpiderfleetService,
              private shared: SharedService,
              private router: Router,
              private toastr: ToastrService)
  {
    if (this.shared.verifyLoggin()) {

      this.getListsSelects();

    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnChanges(): void {

    this.getDeviceInfo(this.idSend);
  }

  ngOnInit(): void {
  }

  getListsSelects(){
    combineLatest([
      this.spiderService.getListRedInfoDevice(),
      this.spiderService.getListTimeInfoDevice(),
      this.spiderService.getTypeDevices()
    ]).subscribe(([red, time, types]) => {
      this.typesDevice = types;
      this.reds = red;
      this.times = time;
    })
  }

  getDeviceInfo(id: string): void {
    this.spiderService.getInfoDispositivioId(id).subscribe(data => {
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

    this.spiderService.postInfoDevice(data).subscribe(data => {

      if (data['success']) {
        this.toastr.success("Información actualizada correctamente", 'Correcto')
        this.reloadTable();
      }

    })

  }

}
