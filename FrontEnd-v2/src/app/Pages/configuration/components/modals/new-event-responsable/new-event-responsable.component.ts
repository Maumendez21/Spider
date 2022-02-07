import { Component, OnInit, Input } from '@angular/core';
import { ConfigurationService } from '../../../services/configuration.service';
import { SpiderService } from '../../../../../Services/spider.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { ScheduleService } from '../../../services/schedule.service';

@Component({
  selector: 'app-new-event-responsable',
  templateUrl: './new-event-responsable.component.html',
  styleUrls: ['./new-event-responsable.component.css']
})
export class NewEventResponsableComponent implements OnInit {

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

  constructor(
    private configurationService: ConfigurationService,
    private spiderService: SpiderService,
    private scheduleService: ScheduleService
  ) { 
    this.getResponsables();
    this.getDispositivos();
    this.getGrupos();
  }

  ngOnInit(): void {
  }

  ngOnChanges(): void {
    this.fecha_inicio = this.fechaAgenda+"T00:00";
    this.fecha_fin = this.fechaAgenda+"T23:59";
  }

  getResponsables() {
    this.configurationService.getListResponsables()
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
    this.configurationService.getListDevicesConfiguration()
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

      this.scheduleService.setNuevoEventoAgendaConfiguracion(event)
        .subscribe(response => {
          console.log(response);
          
          if (response['success']) {
            this.agendaReload(true);
            Swal.fire('Exito!' , 'Exito al registrar evento', 'success');
          } else {
            Swal.fire('Error!' , '' + response['messages'] , 'error');
          }
        });
    }else {
      Swal.fire('Error!' , 'Es necesario llenar todos los campos y la Fecha Inicio no debe ser mayor a la Fecha Final', 'error');
    }
  }






}
