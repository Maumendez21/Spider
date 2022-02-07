import { Injectable } from '@angular/core';
import { ServiceClass } from '../../../utils/services-class';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class EngineStopService {

  public serviceClass = null;

  constructor(
    private http: HttpClient,
  ) {
    this.serviceClass = new ServiceClass(this.http);
  }

  getListEngineStop(device: string, page: number) {
    return this.serviceClass.getQuery(`engine/stop/list?search=${device}&page=${page}`)
      .pipe( map(data => {
        return data['ListEngineStops'];
      }));
    }

    getPagesEngineStop(name: string){
      return this.serviceClass.getQuery(`engine/stop/count?search=${name}`)
        .pipe( map((data: any) => {
          return data.NumberPages;
        }));
  }

  setStopEngine(device: string, status: number){
    return this.serviceClass.postQuery(`engine/stop/execute?device=${device}&status=${status}`);
  }

}
