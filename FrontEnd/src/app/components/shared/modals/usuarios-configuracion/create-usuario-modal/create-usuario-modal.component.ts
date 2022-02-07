import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { last } from 'rxjs/operators';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-create-usuario-modal',
  templateUrl: './create-usuario-modal.component.html',
  styleUrls: ['./create-usuario-modal.component.css']
})
export class CreateUsuarioModalComponent implements OnInit {

  @Input() reloadTable: any;

  rolesList: any;
  estatusList: any;
  gruposList: any;

  name: string;
  lastname: string;
  email: string;
  username: string;
  password: string;
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

  createUsuario() {

    if (this.name != "" && this.lastname != "" && this.email != "" && this.username != "" && this.password != "" && this.phone != "" && this.group != "" && this.rol != "" && this.estatus != "") {

      const data = {
        Name: this.name,
        LastName: this.lastname,
        Email: this.email,
        UserName: this.username,
        Password: this.password,
        Telephone: this.phone,
        Grupo: this.group,
        Role: this.rol,
        Status: this.estatus
      };

      this.spiderService.setNewUsuarioConfiguracion(data)
        .subscribe(response => {
          if (response['success']) {

            this.reloadTable();
            this.toastr.success("Exito al registrar el usuario", "Exito!");

            this.name = "";
            this.lastname = "";
            this.email = "";
            this.username = "";
            this.password = "";
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
