import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { UserModel } from 'src/app/models/user/user.model';
import { ToastrService } from 'ngx-toastr';
import { UserUpdateModel } from 'src/app/models/user/user-update.model';
import { SharedService } from 'src/app/services/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})

export class UsersComponent implements OnInit {

  users: any;
  userModel: UserModel;
  userUpdateModel: UserUpdateModel;

  pageOfItems: Array<any>;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {

    this.limpiarFiltrosMapa();

    if (shared.verifyLoggin()) {
      this.getListUsers();
      this.userModel = new UserModel();
      this.userUpdateModel = new UserUpdateModel();
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

  getListUsers() {
    this.spiderService.getListSubusers()
      .subscribe(response => {

        this.users = response.map((x, i) => ({ UserName: x.UserName, Name: x.Name, LastName: x.LastName, Email: x.Email, DescripcionRole: x.DescripcionRole, DescriptionStatus: x.DescriptionStatus, SubEmpresa: x.SubEmpresa }));;
      });
  }

  updateUser(id: string) {
    this.spiderService.getInfoUser(id)
      .subscribe(data => {
        this.userUpdateModel.Name = data.Name;
        this.userUpdateModel.LastName = data.LastName;
        this.userUpdateModel.Email = data.Email;
        this.userUpdateModel.Telephone = data.Telephone;
      });
  }

  onSubmit() {
    this.spiderService.setNewUser(this.userModel)
      .subscribe(data => {
        if (data['success']) {
          this.toastr.success('Usuario agregado', 'Exito!');
          document.getElementById('closeModalAddUser').click();
          this.getListUsers();
        } else {
          this.toastr.warning(data['messages'], 'Campo Vacío');
        }
      });
  }

  onSubmitUpdate() {

    this.spiderService.setUpdateUser(this.userUpdateModel)
      .subscribe(data => {
        if (data['success']) {
          this.toastr.success("Usuario actualizado exitosamente!", "Exito!");
          document.getElementById('closeModalUpdateUser').click();
          this.getListUsers();
        } else {
          this.toastr.error("Hubo un error al tratar de actualizar el usuario, intentalo nuevamente", "Error!");
        }
      });
  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
