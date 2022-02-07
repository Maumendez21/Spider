import { Component, Input, OnInit } from '@angular/core';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nuevo-evento',
  templateUrl: './nuevo-evento.component.html',
  styleUrls: ['./nuevo-evento.component.css']
})
export class NuevoEventoComponent implements OnInit {

  @Input() fechaAgenda: string;
  @Input() agendaReload: any;

  responsables: any;
  grupos: any;
  dispositivos: any;

  responsable: string = "";
  grupo: string = "";
  dispositivo: string = "";
  fecha_inicio: string;
  fecha_fin: string;
  notas: string;
  repeticion: string = "Unica";

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService) {
    this.getResponsables();
    this.getGrupos();
    this.getDispositivos();
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.fecha_inicio = this.fechaAgenda+"T00:00";
    this.fecha_fin = this.fechaAgenda+"T23:59";
  }

  getResponsables() {
    this.spiderService.getListResponsables()
      .subscribe(responsables => {
        this.responsables = responsables;
      });
  }

  getGrupos() {
    this.spiderService.getSubempresas()
      .subscribe(grupos => {
        this.grupos = grupos;
      });
  }

  getDispositivos() {
    this.spiderService.getListDevicesConfiguration()
      .subscribe(dispositivos => {
        this.dispositivos = dispositivos;
      })
  }

  agregarEvento() {

    if (moment(this.fecha_inicio).isSameOrBefore(this.fecha_fin) && this.dispositivo != "" && this.responsable != "") {

      const event = {
        StartDate: this.fecha_inicio,
        EndDate: this.fecha_fin,
        Notes: this.notas,
        Device: this.dispositivo,
        Responsable: this.responsable,
        Frecuency: this.repeticion
      }
      // console.log(event);

      this.spiderService.setNuevoEventoAgendaConfiguracion(event)
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
