import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-devices-configuration',
  templateUrl: './devices-configuration.component.html',
  styleUrls: ['./devices-configuration.component.css']
})
export class DevicesConfigurationComponent implements OnInit {

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

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {
    if (this.shared.verifyLoggin()) {
      this.getListDevices();
      this.getSubempresas();
      this.getResponsables();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {}

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

    this.spiderService.getListDevicesConfiguration()
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
    this.spiderService.getListSubempresas()
      .subscribe(response => {
        this.subempresas = response;
      });
  }

  getResponsables() {
    this.spiderService.getListResponsables()
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

    this.spiderService.setAssignmentSubempresa(data)
      .subscribe(response => {
        if (response['success']) {
          this.toastr.success("Se actualizo exitosamente la información del dispositivo", "Exito!");
          this.refreshTable(this.index, data.Name, data.Hierarchy, data.ResponsableNombre);
          this.index = 0;
        } else {
          this.toastr.error(response["messages"][0], "Error!");
        }
      });
  }

}
