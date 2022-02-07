import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SpiderService } from 'src/app/Services/spider.service';
import { Maps } from 'src/app/utils/maps';
import Swal from 'sweetalert2';
import { GeofencesService } from '../../services/geofences.service';
declare const google: any;
@Component({
  selector: 'app-update-geofence',
  templateUrl: './update-geofence.component.html',
  styleUrls: ['./update-geofence.component.css']
})
export class UpdateGeofenceComponent implements OnInit {

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

  constructor(
    private spiderService: SpiderService,
    private geofencesService: GeofencesService,
    public route: ActivatedRoute,
    private router: Router,
    private mapHelper: Maps
  ) { 
    route.paramMap.subscribe(params => {
      this.idGeocerca = params.get('id');
    });

    this.getGrupos();
    this.style = mapHelper.style;
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
    this.geofencesService.getGeocerca(this.idGeocerca)
      .subscribe(data => {
        this.nameUpdate = data.Name;
        this.descripcion = data.Description;
        this.estatus = data.Active;
        this.grupo = data.Hierarchy;
        this.pointsUpdate = [...data.Polygon['Coordinates']];
        this.initUpdateManager(map);
      });
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

      this.geofencesService.setUpdateGeocerca(data)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire({
              title: 'Geocerca Actualizada!',
              confirmButtonText: 'OK',
              icon: 'success'
            }).then((result) => {
              if (result.isConfirmed) {
                this.router.navigate(['/geofences/list']);
              }
            })
          } else {
            Swal.fire({
              icon: 'error',
              title: '' + response['messages'][0]
            })
          }
        });

    }
  }

  public markersFit: any = [];
  initUpdateManager(map: any) {

    const self = this;

    let latlngbounds = new google.maps.LatLngBounds();

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
    this.pointsUpdate.map(point => {

      this.markersFit.push(new google.maps.LatLng(point['lat'], point['lng']));
    })

    bermudaTriangle.setMap(map);
    for (let i = 0; i < this.markersFit.length; i++) {
      latlngbounds.extend(this.markersFit[i]);
    }

    map.fitBounds(latlngbounds);
    
    






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



}
