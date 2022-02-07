import { Component, OnInit, Input } from '@angular/core';
import { ConfigurationService } from '../../../services/configuration.service';
import { SpiderService } from '../../../../../Services/spider.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { ScheduleService } from '../../../services/schedule.service';

@Component({
  selector: 'app-update-event-responsable',
  templateUrl: './update-event-responsable.component.html',
  styleUrls: ['./update-event-responsable.component.css']
})
export class UpdateEventResponsableComponent implements OnInit {

  @Input() evento: any;
  @Input() agendaReload: any;

  responsables: any;
  grupos: any;
  dispositivos: any;
  
  idStart: string;
  idEnd: string;
  responsable: string = "";
  grupo: string = "";
  dispositivo: string = "";
  fecha_inicio: string;
  fecha_fin: string;
  notas: string;

  constructor(
    private configuracionService: ConfigurationService,
    private spiderService: SpiderService,
    private scheduleService: ScheduleService
  ) { }

  ngOnInit(): void {
    this.getResponsables();
    this.getGrupos();
    this.getDispositivos();
  }

  ngOnChanges(): void {

    if (this.evento) {
      this.idStart = this.evento.IdStart;
      this.idEnd = this.evento.IdEnd;
      this.responsable = this.evento.Responsable;
      this.dispositivo = this.evento.Device;
      this.fecha_inicio = this.evento.StartDate;
      this.fecha_fin = this.evento.EndDate;
      this.notas = this.evento.Notes;
    }

  }

  getResponsables() {
    this.configuracionService.getListResponsables()
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
    this.scheduleService.getListDevices()
    .subscribe(dispositivos => {
      this.dispositivos = dispositivos;
    })
  }

  actualizarEvento() {
    if (moment(this.fecha_inicio).isSameOrBefore(this.fecha_fin) && this.dispositivo != "" && this.responsable != "") {

      const event = {
        IdStart: this.idStart,
        StartDate: this.fecha_inicio,
        IdEnd: this.idEnd,
        EndDate: this.fecha_fin,
        Notes: this.notas,
        Device: this.dispositivo,
        Responsable: this.responsable
      }

      this.scheduleService.updateEventoAgendaConfiguracion(event)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire('Exito!', 'Exito al actualizar el evento.', 'success')
            this.agendaReload(true);
          } else {
            Swal.fire('Error', '' + response['messages'], 'error')
          }
        });
    } else {
      Swal.fire('Warning', 'Es necesario llenar todos los campos y la Fecha Inicio no debe ser mayor a la Fecha Final', 'error' )
    }
  }

  eliminarEvento() {
    this.scheduleService.deleteEventoAgendaConfiguracion(this.idStart, this.idEnd)
      .subscribe(response => {
        if (response['success']) {

          Swal.fire('Exito', 'Exito al eliminar el evento', 'success' )
          this.agendaReload(true);

        } else {
          Swal.fire('Error', '' + response['messages'], 'error' )
        }
      });
  }




}
