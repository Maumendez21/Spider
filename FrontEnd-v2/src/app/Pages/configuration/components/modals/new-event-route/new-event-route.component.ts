import { Component, OnInit, Input } from '@angular/core';
import { SpiderService } from '../../../../../Services/spider.service';
import { ConfigurationService } from '../../../services/configuration.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { ScheduleService } from '../../../services/schedule.service';

@Component({
  selector: 'app-new-event-route',
  templateUrl: './new-event-route.component.html',
  styleUrls: ['./new-event-route.component.css']
})
export class NewEventRouteComponent implements OnInit {

  @Input() evento: any;
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

  idStart: string;
  idEnd: string;
  
  @Input() create: boolean;

  update: boolean;

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



  eventoTemp: any;

  ngOnChanges(): void {
    // this.clean();

    this.fecha_inicio = this.fechaAgenda+"T00:00";
    this.fecha_fin = this.fechaAgenda+"T23:59";

 

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
      this.scheduleService.setNuevoEventoRutaAgenda(event)
        .subscribe(response => {
          if (response['success']) {
            Swal.fire('Exito!', 'Exito al registrar el evento.', 'success')
            this.agendaReload(true);
          } else {
            Swal.fire('Error', '' + response['messages'], 'error')
          }
        });
    } else {
      Swal.fire('Warning', 'Es necesario llenar todos los campos y la Fecha Inicio no debe ser mayor a la Fecha Final', 'error' )
    }
  }
  
  

  



}
