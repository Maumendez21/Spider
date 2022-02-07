import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-devices',
  templateUrl: './devices.component.html',
  styleUrls: ['./devices.component.css']
})
export class DevicesComponent implements OnInit {

  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};

  index : number;

  idSim: number;
  idDevice: string;
  label: string;
  sim: string;
  type: string;
  company: string;
  motor: boolean = false;
  panico: boolean = false;

  updateIdDevice: string;
  updateIdDeviceAnt: string;
  updateLabel: string;
  updateType: string;
  updateCompany: string;
  updateSim: number;
  updateMotor: boolean = false;
  updatePanico: boolean = false;

  sims:any = [];
  devices: any = [];
  typeDevices: any = [];
  companies: any = [];

  device: string;
  name: string;
  subempresa: string;
  subempresas: any;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {

    this.limpiarFiltrosMapa();

    if (this.shared.verifyLoggin()) {
      this.getListDevices();
      this.getListSims();
      this.getTypeDevices();
      this.getCompanies();
      this.getSubempresas();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }



  getListDevices = () => {

    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      ordering: true,
      responsive: true,
      language: {

        'search': "Buscar",
        'paginate': {
          'first': 'Primero',
          'previous': 'Anterior',
          'next': 'Siguiente',
          'last': 'Ultimo'
        },
        'lengthMenu': 'Mostrar _MENU_ documentos',
        'info': 'Mostrando _PAGE_ de _PAGES_'
      },
    };

    this.spiderService.getListDevicesAdministration()
      .subscribe(data => {
        this.devices = data.map((x, i) => ({
          device: x.Device, company: x.Company,
          hierarchy: x.Hierarchy, name: x.Name
        }));

        this.dtTrigger.next();
      });
  }

  getListSims() {
    this.spiderService.getListSims()
      .subscribe(data => {
        this.sims = data;
      });
  }

  getTypeDevices() {
    this.spiderService.getTypeDevices()
      .subscribe(data => {
        this.typeDevices = data;
      });
  }

  getCompanies() {
    this.spiderService.getCompanies()
      .subscribe(data => {
        this.companies = data;
      });
  }

  getSubempresas() {
    this.spiderService.getListSubempresas()
      .subscribe(response => {
        this.subempresas = response;
      });
  }

  registrarDispositivo() {

    const data = {
      "IdDevice": this.idDevice,
      "IdDeviceAnt": "",
      "Name": "",
      "Label": this.label,
      "Hierarchy": this.company,
      "IdType": this.type,
      "IdSim": this.sim,
      "Motor": (this.motor) ? 1 : 0,
      "Panico": (this.panico) ? 1 :0,
      "Status": 1
    };

    if (this.idDevice != "" && this.label != "" && this.company != null && this.type != null && this.sim != null) {

      this.spiderService.setNewDevice(data)
        .subscribe(data => {
          document.getElementById('closeModalNew').click();
          this.toastr.success("Se agrego exitosamente el dispositivo", "Exito!");
          this.refreshDataTableNew(this.idDevice);

          this.idDevice = "";
          this.label = "";
          this.company = "";
          this.type = "";
          this.sim = "";
          this.motor = false;
          this.panico = false;
        });
    } else {
      this.toastr.warning("Es necesario llenar los campos", "Campos vacios");
    }
  }



  actualizarDispositivo() {
    const data = {
      "IdDevice": this.updateIdDevice,
      "IdDeviceAnt": (this.updateIdDeviceAnt != this.updateIdDevice) ? this.updateIdDeviceAnt : "",
      "Name": "",
      "Label": this.updateLabel,
      "Hierarchy": this.updateCompany,
      "IdType": this.updateType,
      "IdSim": this.updateSim,
      "Motor": (this.updateMotor) ? 1 : 0,
      "Panico": (this.updatePanico) ? 1 : 0,
      "Status": 1
    };

    if (this.updateIdDevice != "" && this.updateLabel != "" && this.updateCompany != null && this.updateType != null) {

      this.spiderService.setUpdateDevice(data)
        .subscribe(data => {
          document.getElementById('closeModalUpdate').click();
          this.refreshDataTableUpdate(this.index, this.updateIdDevice, this.updateCompany);
          this.toastr.success("Se actualizo exitosamente el dispositivo", "Exito!");

          this.updateIdDevice = "";
          this.updateIdDeviceAnt = "";
          this.updateLabel = "";
          this.updateCompany = "";
          this.updateType = "";
          this.updateSim = 0;
          this.updateMotor = false;
          this.updatePanico = false;

          this.index =0;
        });
    } else {
      this.toastr.warning("Es necesario llenar los campos", "Campos vacios");
    }
  }

  comania: string;
  callModalUpdate(id: string, index: number) {
    this.spiderService.getDevice(id)
    .subscribe(data => {

      this.updateIdDevice = data.IdDevice;
      this.updateIdDeviceAnt = data.IdDevice;
      this.updateLabel = data.Label;
      this.updateCompany = data.Hierarchy;
      this.updateType = data.IdType;
      this.updateSim = data.IdSim;
      this.updateMotor = (data.Motor == 1);
      this.updatePanico = (data.Panico == 1);
      this.index = index;
      // this.devices[index].company = this.compania;

      });


  }


  refreshDataTableUpdate(index: number, device: string, compania: string){
    this.devices[index].device = device;
    this.devices[index].company = compania;
  }

  refreshDataTableNew(device: string){
      this.devices.push({
        device: device
      });
  }

  callModalAsignar(device: string, name: string, hierarchy: string, id: number) {
    this.device = device;
    this.name = name;
    this.subempresa = hierarchy;
    this.index = id;

  }

  refreshDataTableAsignacion( name: string, hierarchy: string, id: number){
    this.devices[id].name = name;
    this.devices[id].hierarchy = hierarchy;

  }

  asignarSubempresa() {

    const data = {
      IdDevice: this.device,
      Name: this.name,
      Hierarchy: this.subempresa
    };

    this.spiderService.setAssignmentSubempresa(data)
      .subscribe(response => {
        if (response['success']) {
          this.toastr.success("Se actualizo exitosamente la información del dispositivo", "Exito!");
          this.refreshDataTableAsignacion(data.Name, data.Hierarchy, this.index);
          this.index = 0;
        } else {
          this.toastr.error("Al parecer ocurrio un error al tratar de actualizar la información del dispositivo", "Error!");
        }
      });
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

}
