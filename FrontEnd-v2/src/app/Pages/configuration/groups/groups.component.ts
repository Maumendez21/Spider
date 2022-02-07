import { Component, OnInit } from '@angular/core';
import { TREE_ACTIONS, KEYS, IActionMapping, ITreeOptions } from '@circlon/angular-tree-component';
import Swal from 'sweetalert2';
import { ConfigurationService } from '../services/configuration.service';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {

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

  constructor(
    private configurationService: ConfigurationService
  ) { 
    this.getDataNodes();
  }

  async getDataNodes() {

    let tree = [];

    await this.configurationService.getListSubempresas()
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

    this.configurationService.setNewSubempresa(data)
      .subscribe(response => {

        if (response['success']) {
          this.getDataNodes();
          this.newSubempresa = "";
          Swal.fire(
            'Correcto!',
            'Se añadio exitosamente la subempresa.',
            'success'
          )


        } else {
          Swal.fire(
            'Error!',
            'Al parecer hubo un error al tratar de añadir la subempresa.',
            'error'
          )
        }
      });
  }

  updateChild() {
    
    const data = {
      idSubCompany: this.id,
      name: this.updateSubempresa
    };

    this.configurationService.setUpdateSubempresa(data)
      .subscribe(response => {
        if (response['success']) {
          this.getDataNodes();
          this.updateSubempresa = "";
          Swal.fire(
            'Correcto!',
            'Se actualizo exitosamente la subempresa.',
            'success'
          )
        } else {
          Swal.fire(
            'Error!',
            'Al parecer hubo un error al tratar de actualizar la subempresa.',
            'error'
          )
        }
      });

  }

  ngOnInit(): void {
  }

}
