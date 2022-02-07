import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-update-usuario-modal',
  templateUrl: './update-usuario-modal.component.html',
  styleUrls: ['./update-usuario-modal.component.css']
})
export class UpdateUsuarioModalComponent implements OnInit {

  @Input() reloadTable: any;
  @Input() idSend: number;

  rolesList: any;
  estatusList: any;
  gruposList: any;

  name: string;
  lastname: string;
  email: string;
  username: string;
  phone: string;
  group: string;
  rol: string;
  estatus: string;

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) { 
    
    if (this.shared.verifyLoggin()) {
      this.getRoles();
      this.getEstatus();
      this.getGrupos();
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {}

  ngOnChanges(): void {
    this.getUserInfo(this.idSend);
    
  }

  getUserInfo(id: number) {
    this.spiderService.getUsuarioConfiguracion(id)
      .subscribe(data => {
        // console.log(data.Name);
        this.name = data.Name;
        this.lastname = data.LastName;
        this.email = data.Email;
        this.username = data.UserName;
        this.phone = data.Telephone;
        this.group = data.Hierarchy;
        this.rol = data.IdRole;
        this.estatus = data.IdStatus;
      });
  }

  getRoles() {
    this.spiderService.getRoles()
      .subscribe(data => {
        // console.log(data);
        this.rolesList = data;
      });
  }

  getEstatus() {
    this.spiderService.getEstatus()
      .subscribe(data => {
        // console.log(data);
        this.estatusList = data;
      });
  }

  getGrupos() {
    this.spiderService.getListSubempresas()
      .subscribe(data => {
        // console.log(data);
        this.gruposList = data;
      });
  }

  actualizarUsuario() {

    if (this.name != "" && this.lastname != "" && this.email != "" && this.username != "" && this.phone != "" && this.group != "" && this.rol != "" && this.estatus != "") {
      const data = {
        Name: this.name,
        LastName: this.lastname,
        Email: this.email,
        UserName: this.username,
        Telephone: this.phone,
        Grupo: this.group,
        Role: this.rol,
        Status: this.estatus
      };
      this.spiderService.setUpdateUsuarioConfiguracion(data)
        .subscribe(response => {
          // console.log(response);
          if (response['success']) {
            this.reloadTable();
            this.toastr.success("Exito al registrar el usuario", "Exito!");
            this.name = "";
            this.lastname = "";
            this.email = "";
            this.phone = "";
            this.group = "";
            this.rol = "";
            this.estatus = "";
          } else {
            this.toastr.error(response['messages'] , "Error!");
          }
        });
    } else {
      this.toastr.warning("Es necesario llenar todos los campos", "Campos vacios!");
    }
    
  }

}
