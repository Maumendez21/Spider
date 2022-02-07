import { Component, OnInit, Input } from '@angular/core';
import { SpiderService } from '../../../../../Services/spider.service';
import { ConfigurationService } from '../../../services/configuration.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { ScheduleService } from '../../../services/schedule.service';

@Component({
  selector: 'app-update-event-route',
  templateUrl: './update-event-route.component.html',
  styleUrls: ['./update-event-route.component.css']
})
export class UpdateEventRouteComponent implements OnInit {

  @Input() evento: any;
  @Input() agendaReload: any;

  rutas: any;
  grupos: any;
  dispositivos: any;

  idStart: string;
  idEnd: string;
  ruta: string = "";
  grupo: string = "";
  dispositivo: string = "";
  fecha_inicio: string;
  fecha_fin: string;
  notas: string;

  constructor(
    private spiderService: SpiderService,
    private configurationService: ConfigurationService,
    private scheduleService: ScheduleService
  ) { 
    this.getRutas();
    this.getGrupos();
    this.getDispositivos();
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {

    if (this.evento) {
      this.idStart = this.evento.IdStart;
      this.idEnd = this.evento.IdEnd;
      this.ruta = this.evento.Route;
      this.dispositivo = this.evento.Device;
      this.fecha_inicio = this.evento.StartDate;
      this.fecha_fin = this.evento.EndDate;
      this.notas = this.evento.Notes;
    }

  }

  getRutas() {
    this.scheduleService.getListRutasConfiguracion()
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
    this.scheduleService.getListDevices()
    .subscribe(dispositivos => {
      this.dispositivos = dispositivos;
    })
  }

  actualizarEvento() {
    if (moment(this.fecha_inicio).isSameOrBefore(this.fecha_fin) && this.dispositivo != "" && this.ruta != "") {

      const event = {
        IdStart: this.idStart,
        StartDate: this.fecha_inicio,
        IdEnd: this.idEnd,
        EndDate: this.fecha_fin,
        Notes: this.notas,
        Device: this.dispositivo,
        IdRoute: this.ruta
      }

      this.scheduleService.updateEventoRutaAgenda(event)
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
    console.log(this.idStart, this.idEnd);
    
    this.scheduleService.deleteEventoRutaAgenda(this.idStart, this.idEnd)
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
