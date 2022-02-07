import { Component, OnInit, Input } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SharedService } from 'src/app/services/shared.service';
import { Maps } from 'src/app/helpers/maps';

declare const google: any;

@Component({
  selector: 'app-nueva-geocerca',
  templateUrl: './nueva-geocerca.component.html',
  styleUrls: ['./nueva-geocerca.component.css']
})
export class NuevaGeocercaComponent implements OnInit {

  name: string = "";
  grupo: string = "";
  descripcion: string = "";
  estatus: boolean = true;

  center: any = {
    lat: 19.4525976,
    lng: -99.1182164
  };

  gruposList: any = [];
  pointList: any = [];
  selectedArea = 0;

  drawingManager: any;
  style: any;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService, private router: Router, private shared: SharedService, private mapHelper: Maps) {
    if (shared.verifyLoggin()) {
       this.getGrupos();
       this.style = mapHelper.style;
    } else {
      this.router.navigate(['/login']);
    }
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

  registrarGeocerca() {

    if (this.name != "" && this.grupo != "" && this.pointList.length >= 3) {

      this.pointList.push(this.pointList[0]);

      const data = {
        Name: this.name,
        Hierarchy: this.grupo,
        Coordinates: this.pointList,
        Description: this.descripcion,
        Active: this.estatus
      };

      this.spiderService.setNuevaGeocerca(data)
        .subscribe(response => {
          if (response['success']) {
            this.toastr.success('Geocerca creada exitosamente', 'Exito!');
            this.router.navigate(['/configuration/geocercas']);
          } else  {
            this.toastr.error(response['messages'][0]);
          }
        });
    } else {
      this.toastr.error('Es necesario llenar todos los campos', 'Campos vacios');
    }

  }

}
