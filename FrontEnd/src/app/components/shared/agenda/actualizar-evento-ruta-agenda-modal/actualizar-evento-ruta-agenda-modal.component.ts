import { Component, Input, OnInit } from '@angular/core';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-actualizar-evento-ruta-agenda-modal',
  templateUrl: './actualizar-evento-ruta-agenda-modal.component.html',
  styleUrls: ['./actualizar-evento-ruta-agenda-modal.component.css']
})
export class ActualizarEventoRutaAgendaModalComponent implements OnInit {

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

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService) {
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

      this.spiderService.updateEventoRutaAgenda(event)
        .subscribe(response => {
          if (response['success']) {
            this.toastr.success("Exito al actualizar el evento", "Exito!");
            this.agendaReload(true);
          } else {
            this.toastr.error(response['messages'] , "Error!");
          }
        });
    } else {
      this.toastr.warning('Es necesario llenar todos los campos y la Fecha Inicio no debe ser mayor a la Fecha Final', 'Campos vacios');
    }
  }

  eliminarEvento() {
    this.spiderService.deleteEventoRutaAgenda(this.idStart, this.idEnd)
      .subscribe(response => {
        if (response['success']) {
          this.toastr.success("Exito al eliminar el evento", "Exito!");
          this.agendaReload(true);
        } else {
          this.toastr.error(response['messages'] , "Error!");
        }
      });
  }

}
