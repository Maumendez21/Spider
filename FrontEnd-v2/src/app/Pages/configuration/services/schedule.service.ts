import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  setNuevoEventoRutaAgenda(data: any) {
    return this.serviceClass.postQuery("configuration/routes/diary", data);
  }

  updateEventoAgendaConfiguracion(event: any) {
    return this.serviceClass.putQuery('configuration/diary', event);
  }

  deleteEventoAgendaConfiguracion(idStart: string, idEnd: string) {
    return this.serviceClass.deleteQuery(`configuration/diary?start=${idStart}&end=${idEnd}`);
  }

  deleteEventoRutaAgenda(start: string, end: string) {
    return this.serviceClass.deleteQuery(`configuration/routes/diary?start=${start}&end=${end}`);
  }

  updateEventoRutaAgenda(data: any) {
    return this.serviceClass.putQuery("configuration/routes/diary", data);
  }

  setNuevoEventoAgendaConfiguracion(event: any) {
    return this.serviceClass.postQuery('configuration/diary', event);
  }

  getListRutasConfiguracion() {
    return this.serviceClass.getQuery('routes/list')
      .pipe( map(data => {
        return data['routes'];
      }));
  }

  getListDevices() {
    return this.serviceClass.getQuery("management/obd/list")
    .pipe( map(data => {
      return data['listObd'];
    }));
  }
}
