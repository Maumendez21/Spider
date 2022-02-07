import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SpiderService } from 'src/app/Services/spider.service';
import { Maps } from 'src/app/utils/maps';
import Swal from 'sweetalert2';
import { GeofencesService } from '../../services/geofences.service';
declare const google: any;


@Component({
  selector: 'app-new-geofence',
  templateUrl: './new-geofence.component.html',
  styleUrls: ['./new-geofence.component.css']
})
export class NewGeofenceComponent implements OnInit {

  public name: string = "";
  public grupo: string = "";
  public descripcion: string = "";
  public estatus: boolean = true;

  public center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  public gruposList: any = [];
  public pointList: any = [];
  public selectedArea = 0;

  public drawingManager: any;
  style: any;

  constructor(
    private spiderService: SpiderService,
    private geofencesService: GeofencesService,
    private mapHelper: Maps,
    private router: Router
  ) { 
    this.getGrupos();
    this.style = mapHelper.style;
  }

  ngOnInit(): void {
  }

  onMapReady(map) {
    this.initDrawingManager(map);
  }


  getGrupos() {
    this.spiderService.getSubempresas()
      .subscribe(data => {
        this.gruposList = data;
      });
  }

  registrarGeocerca() {

    if (this.name != "" && this.grupo != "" && this.pointList.length >= 3) {

      this.pointList.push(this.pointList[0]);

      const data = {
        Name: this.name,        Hierarchy: this.grupo,
        Coordinates: this.pointList,
        Description: this.descripcion,
        Active: this.estatus
      };

      this.geofencesService.setNuevaGeocerca(data)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire({
              title: 'Geocerca registrada!',
              confirmButtonText: 'OK',
              icon: 'success'
            }).then((result) => {

              if (result.isConfirmed) {
                this.router.navigate(['/geofences/list']);
              }
            })
          } else  {
            Swal.fire({
              icon: 'error',
              title: '' + response['messages'][0]
            })
          }
        });
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Es necesario llenar todos los campos'
      })
    }

  }

  initDrawingManager(map: any) {
    const self = this;
    const options = {
      drawingControl: true,
      drawingControlOptions: {
        drawingModes: ['polygon'],
      },
      polygonOptions: {
        draggable: true,
        editable: true,
      },
      drawingMode: google.maps.drawing.OverlayType.POLYGON,
    };
    this.drawingManager = new google.maps.drawing.DrawingManager(options);
    this.drawingManager.setMap(map);

    google.maps.event.addListener(
      this.drawingManager,
      'overlaycomplete',
      (event) => {
        if (event.type === google.maps.drawing.OverlayType.POLYGON)    {
          const paths = event.overlay.getPaths();
          for (let p = 0; p < paths.getLength(); p++) {
            google.maps.event.addListener(
              paths.getAt(p),
              'set_at',
              () => {
                if (!event.overlay.drag) {
                  self.updatePointList(event.overlay.getPath());
                }
              }
            );
            google.maps.event.addListener(
              paths.getAt(p),
              'insert_at',
              () => {
                self.updatePointList(event.overlay.getPath());
              }
            );
            google.maps.event.addListener(
              paths.getAt(p),
              'remove_at',
              () => {
                self.updatePointList(event.overlay.getPath());
              }
            );
          }
          self.updatePointList(event.overlay.getPath());
        }
        if (event.type !== google.maps.drawing.OverlayType.MARKER) {
          // Switch back to non-drawing mode after drawing a shape.
          self.drawingManager.setDrawingMode(null);
          // To hide:
          self.drawingManager.setOptions({
            drawingControl: false,
          });
        }
      }
    );
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
