import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import * as moment from 'moment';

@Component({
  selector: 'app-nuevo-evento-ruta-agenda-modal',
  templateUrl: './nuevo-evento-ruta-agenda-modal.component.html',
  styleUrls: ['./nuevo-evento-ruta-agenda-modal.component.css']
})
export class NuevoEventoRutaAgendaModalComponent implements OnInit {

  @Input() fechaAgenda: string;
  @Input() agendaReload: any;

  rutas: any;
  grupos: any;
  dispositivos: any;

  ruta: string = "";
  grupo: string = "";
  dispositivo: string = "";
  fecha_inicio: string;
  fecha_fin: string;
  notas: string;
  repeticion: string = "Unica";

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService) {
    this.getRutas();
    this.getGrupos();
    this.getDispositivos();
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.fecha_inicio = this.fechaAgenda+"T00:00";
    this.fecha_fin = this.fechaAgenda+"T23:59";
  }

  getRutas() {
    this.spiderService.getListRutasConfiguracion()
      .subscribe(rutas => {
        this.rutas = rutas;
      });
  }

  getGrupos() {
    this.spiderService.getSubempresas()
      .subscribe(grupos => {
        this.grupos = grupos;
      });
  }

  getDispositivos() {
    this.spiderService.getListDevices()
      .subscribe(dispositivos => {
        this.dispositivos = dispositivos;
      })
  }

  agregarEvento() {

    if (moment(this.fecha_inicio).isSameOrBefore(this.fecha_fin) && this.dispositivo != "" && this.ruta != "") {

      const event = {
        StartDate: this.fecha_inicio,
        EndDate: this.fecha_fin,
        Notes: this.notas,
        Device: this.dispositivo,
        IdRoute: this.ruta,
        Frecuency: this.repeticion
      }

      // console.log(event);


      this.spiderService.setNuevoEventoRutaAgenda(event)
        .subscribe(response => {
          if (response['success']) {
            this.toastr.success("Exito al registrar el evento", "Exito!");
            this.agendaReload(true);
          } else {
            this.toastr.error(response['messages'] , "Error!");
          }
        });
    } else {
      this.toastr.warning('Es necesario llenar todos los campos y la Fecha Inicio no debe ser mayor a la Fecha Final', 'Campos vacios');
    }
  }

}
