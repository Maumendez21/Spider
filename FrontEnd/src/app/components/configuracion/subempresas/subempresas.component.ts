import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { TREE_ACTIONS, KEYS, IActionMapping, ITreeOptions } from '@circlon/angular-tree-component';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-subempresas',
  templateUrl: './subempresas.component.html',
  styleUrls: ['./subempresas.component.css']
})
export class SubempresasComponent implements OnInit {

  id: string;
  title: string;

  newSubempresa: string;
  updateSubempresa: string;

  nodes: any = [];
  options: ITreeOptions = {
    actionMapping: {
      mouse: {
        dblClick: (tree, node, $event) => {
          this.id = node.data.id;
          this.title = node.data.name;
          this.updateSubempresa = node.data.name;
          document.getElementById("optionsChildModal").click();
        },
        click: (tree, node, $event) => {
          if (node.hasChildren) TREE_ACTIONS.TOGGLE_EXPANDED(tree, node, $event);
        }
      },
      keys: {
        [KEYS.ENTER]: (tree, node, $event) => {
          node.expandAll();
        }
      }
    },
  };

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private shared: SharedService, private router: Router) {
    
    this.limpiarFiltrosMapa();

    if (shared.verifyLoggin()) {
      this.getDataNodes();
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

  async getDataNodes() {

    let tree = [];

    await this.spiderService.getListSubempresas()
      .subscribe(data => {

        data.map(function (x, i) {

          let hierarchy = x.Hierarchy.split("/");
          hierarchy =  hierarchy.filter(function (el) {
            return el != "";
          });

          tree.push({
            id: x.Hierarchy,
            name: x.Name,
            level: hierarchy.length
          });

        });
        this.nodes = this.getTree(tree);
      });
  }

  getTree(array) {
    let levels = [{}];
    array.forEach(function (a) {
      levels.length = a.level;
      levels[a.level - 1]['children'] = levels[a.level - 1]['children'] || [];
      levels[a.level - 1]['children'].push(a);
      levels[a.level] = a;
    });
    return levels[0]['children'];
  }

  addChild() {

    const data = {
      IdFather: this.id,
      NameSubCompany: this.newSubempresa
    };

    this.spiderService.setNewSubempresa(data)
      .subscribe(response => {

        if (response['success']) {
          this.getDataNodes();
          this.newSubempresa = "";
          this.toastr.success("Se añadio exitosamente la subempresa", "Exito!");
        } else {
          this.toastr.error("Al parecer hubo un error al tratar de añadir la subempresa", "Error!");
        }
      });
  }

  updateChild() {
    
    const data = {
      idSubCompany: this.id,
      name: this.updateSubempresa
    };

    this.spiderService.setUpdateSubempresa(data)
      .subscribe(response => {
        if (response['success']) {
          this.getDataNodes();
          this.updateSubempresa = "";
          this.toastr.success("Se actualizo exitosamente la subempresa", "Exito!");
        } else {
          this.toastr.error("Al parecer hubo un error al tratar de actualizar la subempresa", "Error!");
        }
      });

  }

}
