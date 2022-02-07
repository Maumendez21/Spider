import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {

  rol = localStorage.getItem('role');

  id: number;
  user: string;
  usuarios: any;
  pageOfItems: Array<any>;

  public searchValue: string = '';

  constructor(private spiderService: SpiderfleetService) {
    this.getListUsuarios(this.searchValue);
  }

  ngOnInit(): void {}

  getListUsuarios = (value: string = '') => {
    this.spiderService.getListUsuariosConfiguracion(value)
      .subscribe(response => {

        this.usuarios = response.map((x, i) => ({ name: x.Name + " " + x.LastName, email: x.Email, group: x.Grupo, phone: x.Telephone, role: x.DescripcionRol, id: x.UserName }));

      });
  }

  getUsuario(id: number) {
    this.id = id;

  }
  getNombre(user: string){
    this.user = user;


  }

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

}
