import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { Maps } from 'src/app/helpers/maps';

declare const google: any;

@Component({
  selector: 'app-actualizar-geocerca',
  templateUrl: './actualizar-geocerca.component.html',
  styleUrls: ['./actualizar-geocerca.component.css']
})
export class ActualizarGeocercaComponent implements OnInit {

  idGeocerca: string;
  nameUpdate: string;
  pointsUpdate: any = [];

  name: string = "";
  grupo: string = "";
  descripcion: string = "";
  estatus: boolean = true;

  center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  pointList: any = [];
  gruposList: any = [];
  style: any;

  selectedArea = 0;

  constructor(private spiderService: SpiderfleetService, private router: Router, private toastr: ToastrService, public route: ActivatedRoute, private shared: SharedService, private mapHelper: Maps) {

    if (shared.verifyLoggin()) {
      route.paramMap.subscribe(params => {
        this.idGeocerca = params.get('id');
      });

      this.getGrupos();
      this.style = mapHelper.style;
    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  onMapReady(map) {
    this.getGeocercaData(map);
  }

  getGrupos() {
    this.spiderService.getSubempresas()
      .subscribe(data => {
        this.gruposList = data;
      });
  }

  getGeocercaData(map: any) {
    this.spiderService.getGeocerca(this.idGeocerca)
      .subscribe(data => {
        this.nameUpdate = data.Name;
        this.descripcion = data.Description;
        this.estatus = data.Active;
        this.grupo = data.Hierarchy;
        this.pointsUpdate = [...data.Polygon['Coordinates']];
        this.initUpdateManager(map);
      });
  }

  initUpdateManager(map: any) {

    const self = this;

    this.setLocalData();

    var bermudaTriangle = new google.maps.Polygon({
      paths: this.pointsUpdate,
      strokeColor: "#FF0000",
      strokeOpacity: 0.8,
      strokeWeight: 2,
      fillColor: "#FF0000",
      fillOpacity: 0.35,
      editable: true,
      draggable: true,
    });

    bermudaTriangle.setMap(map);

    bermudaTriangle.getPaths().forEach(function(path, index){

      google.maps.event.addListener(path, 'insert_at', function(){
        self.updatePointList(path);
      });

      google.maps.event.addListener(path, 'remove_at', function(){
        self.updatePointList(path);
      });

      google.maps.event.addListener(path, 'set_at', function(){
        self.updatePointList(path);
      });

    });
  }

  setLocalData() {

    this.name = this.nameUpdate;

    const len = this.pointsUpdate.length;

    for (let i = 0; i < len; i++) {
      let coord = this.pointsUpdate[i];
      this.pointList.push(
        [ coord.lng, coord.lat ]
      );
    }
  }

  updatePointList = (path) => {
    this.pointList = [];
    const len = path.getLength();
    for (let i = 0; i < len; i++) {
      let coord = path.getAt(i).toJSON();
      this.pointList.push(
        [ coord.lng, coord.lat ]
      );
    }
    this.selectedArea = google.maps.geometry.spherical.computeArea(path);

  }

  actualizarGeocerca() {

    if (this.name != "" && this.grupo != "" && this.pointList.length >= 3) {

      if (!(this.pointList[0][0] == this.pointList[this.pointList.length - 1][0]) && !(this.pointList[0][1] == this.pointList[this.pointList.length - 1][1])) {
        this.pointList.push(this.pointList[0]);
      }

      const data = {
        Id: this.idGeocerca,
        Name: this.name,
        Hierarchy: this.grupo,
        Description: this.descripcion,
        Active: this.estatus,
        Coordinates: this.pointList
      }

      this.spiderService.setUpdateGeocerca(data)
        .subscribe(response => {
          if (response['success']) {
            this.router.navigate(['/configuration/geocercas']);
            this.toastr.success("La geocerca ha sido actualizada exitosamente!", "Exito!");
          } else {
            this.toastr.error(response['messages'][0]);
          }
        });

    }
  }

}
