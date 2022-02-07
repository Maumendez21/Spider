import { Component, OnInit } from '@angular/core';
import { AdminService } from '../services/admin.service';
import { Subject } from 'rxjs';
import { InfoDeviceService } from '../../configuration/services/info-device.service';
import { ConfigurationService } from '../../configuration/services/configuration.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-devices-a',
  templateUrl: './devices-a.component.html',
  styleUrls: ['./devices-a.component.css']
})
export class DevicesAComponent implements OnInit {

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

  constructor(
    private adminService: AdminService,
    private infoDevice: InfoDeviceService,
    private configurationService: ConfigurationService
  ) { 
    // this.getListDevicesAdmin();
    this.getListDevices();
    this.getListSims();
    this.getTypeDevices();
    this.getCompanies();
    this.getSubempresas();
  }

  ngOnInit(): void {
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

    this.adminService.getListDevicesAdministration()
      .subscribe(data => {
        this.devices = data.map((x, i) => ({
          device: x.Device, company: x.Company,
          hierarchy: x.Hierarchy, name: x.Name
        }));

        console.log(this.devices);
        

        this.dtTrigger.next();
      });
  }

  getListSims() {
    this.adminService.getListSims()
      .subscribe(data => {
        this.sims = data;
      });
  }

  getTypeDevices() {
    this.infoDevice.getTypeDevices()
      .subscribe(data => {
        this.typeDevices = data;
      });
  }

  getCompanies() {
    this.adminService.getCompanies()
      .subscribe(data => {
        this.companies = data;
      });
  }

  getSubempresas() {
    this.configurationService.getListSubempresas()
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

      this.adminService.setNewDevice(data)
        .subscribe(data => {
          document.getElementById('closeModalNew').click();
          Swal.fire('Agregado!', 'Se agrego exitosamente el dispositivo', 'success')
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
      Swal.fire('Atención!', 'Es necesario llenar los campos', 'warning')
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

      this.adminService.setUpdateDevice(data)
        .subscribe(data => {
          // document.getElementById('closeModalUpdate').click();
          Swal.fire('Actualizado!', 'Se actualizo exitosamente el dispositivo', 'success')
          this.refreshDataTableUpdate(this.index, this.updateIdDevice, this.updateCompany);

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
        Swal.fire('Atención!', 'Es necesario llenar los campos', 'warning')
    }
  }

  asignarSubempresa() {

    const data = {
      IdDevice: this.device,
      Name: this.name,
      Hierarchy: this.subempresa
    };

    this.adminService.setAssignmentSubempresa(data)
      .subscribe(response => {
        if (response['success']) {
          Swal.fire('Actualizado!', 'Se actualizo exitosamente la información del dispositivo', 'success')
          this.refreshDataTableAsignacion(data.Name, data.Hierarchy, this.index);
          this.index = 0;
        } else {
          Swal.fire('Atención!', 'Al parecer ocurrio un error al tratar de actualizar la información del dispositivo', 'warning')
        }
      });
  }

  ngOnDestroy(): void {
    this.dtTrigger.unsubscribe();
  }

  comania: string;
  callModalUpdate(id: string, index: number) {
    this.adminService.getDevice(id)
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
    // console.log(id);
    
    this.device = device;
    this.name = name;
    this.subempresa = hierarchy;
    this.index = id;

  }

  refreshDataTableAsignacion( name: string, hierarchy: string, id: number){
    console.log(name, hierarchy, id);
    
    this.devices[id].name = name;
    this.devices[id].hierarchy = hierarchy;

  }




 
}
