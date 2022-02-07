import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../services/configuration.service';

@Component({
  selector: 'app-devices-c',
  templateUrl: './devices-c.component.html',
  styleUrls: ['./devices-c.component.css']
})
export class DevicesCComponent implements OnInit {

  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};

  devices: any = [];
  subempresas: any;
  responsables: any;

  index: number;
  device: string;
  name: string;
  subempresa: string;
  responsable: string;

  pageOfItems: Array<any>;

  constructor(
    private configurationService: ConfigurationService
  ) { 
    this.getListDevices();
    this.getSubempresas();
    this.getResponsables();
  }

  ngOnInit(): void {
  }

  getListDevices() {

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

    this.configurationService.getListDevicesConfiguration()
      .subscribe(response => {
        this.devices = response.map((x, i) => ({
          device: x.Device,
          company: x.Company,
          hierarchy: x.Hierarchy,
          name: x.Name,
          responsable: x.Responsable,
          idResponsable: x.IdResponsable
        }));

        this.dtTrigger.next();
      });
  }

  getSubempresas() {
    this.configurationService.getListSubempresas()
      .subscribe(response => {
        this.subempresas = response;
      });
  }

  getResponsables() {
    this.configurationService.getListResponsables()
      .subscribe(response => {
        this.responsables = response;
      });
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  responsableNombre: string;

  callModalAssignment(device: string, name: string, hierarchy: string, responsable: string, responsableName: string, id: number) {
    this.device = device;
    this.name = name;
    this.subempresa = hierarchy;
    this.responsable = responsable;
    this.responsableNombre = responsableName;
    this.index = id;
  }

  refreshTable(index: number,  Nombre: string, compañia: string, responsable: string  ){
      this.devices[index].name = Nombre;
      this.devices[index].hierarchy = compañia;
      this.devices[index].responsable = responsable;
  }

  asignarSubempresa() {

    const data = {
      IdDevice: this.device,
      Name: this.name,
      Hierarchy: this.subempresa,
      Responsable: this.responsable,
      ResponsableNombre: this.responsableNombre
    };

    this.configurationService.setAssignmentSubempresa(data)
      .subscribe(response => {
        if (response['success']) {
          Swal.fire('Correcto!', 'e actualizo exitosamente la información del dispositivo', 'success');
          this.refreshTable(this.index, data.Name, data.Hierarchy, data.ResponsableNombre);
          this.index = 0;
        } else {
          Swal.fire('ERROR!', '' + response["messages"][0] , 'error');
        }
      });
  }
}
