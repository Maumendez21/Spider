import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { permission } from 'src/app/models/permission/permission';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-asignar-permisos',
  templateUrl: './asignar-permisos.component.html',
  styleUrls: ['./asignar-permisos.component.css']
})
export class AsignarPermisosComponent implements OnInit {

  @Input() user: string;
  @Input() reloadTable: any;

  permisos: any = [];
  responsePermisos = new Array<permission>();


  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) {
    if (this.shared.verifyLoggin()) {

    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.listPermisos(this.user);
  }

  listPermisos(user: string){
    this.spiderService.getPermissions(user)
    .subscribe(response => {
      this.permisos = response;
    })
  }

  generarArreglo(modulo: string, estatus: boolean){
    const data: permission = {
      IdUser: this.user,
      Modulo: modulo,
      Active: estatus
    }
    this.responsePermisos.push(data);
    // console.log(this.responsePermisos);
  }

  asignarPermisos(){
    let cadena: string;
    let bandera: boolean = false;
    // Se recorre el arreglo
    this.responsePermisos.some(function(item){
      //Se asigna el valor en una variable
      cadena = item.Modulo;
      // Valida si existe un elemento con la propiedad CON
      if (cadena.indexOf('CON') != -1) {
        bandera = true;
        return true;
      }
    })

    if (bandera) {
      const data: permission = {
        IdUser: this.user,
        Modulo: 'CON',
        Active: true
      }
      this.responsePermisos.push(data);
    }

    const data = {
      PermissionList: this.responsePermisos
    };

    this.spiderService.setPermission(data)
      .subscribe(response => {
        if (response['success']) {
          this.toastr.success("Exito al asignar permisos.", "Exito!");
          this.responsePermisos = [];
          this.reloadTable();
        } else {
          this.toastr.error(response['messages'] , "Error!");
        }
      });
  }

}
