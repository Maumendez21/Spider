import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { map } from 'rxjs/operators';
import { group } from 'console';

@Injectable({
  providedIn: 'root'
})
export class SpiderfleetService {

  url: string = "http://spiderfleetapi.azurewebsites.net/api/";

  constructor(private http: HttpClient) { }

  getQuery(query:string) {
    const url = `${this.url}${ query }`;

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });

    return this.http.get(url, { headers });
  }

  postQuery(query:string, data:any = null) {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });

    const url = `${this.url}${ query }`;

    return this.http.post(url, data, { headers });
  }

  putQuery(query:string, data:any = null) {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });
    const url = `${this.url}${ query }`;

    return this.http.put(url, data, { headers });
  }

  deleteQuery(query:string) {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });

    const url = `${this.url}${ query }`;

    return this.http.delete(url, { headers });
  }

  login(data:any) {
    return this.postQuery("access/login", data);
  }

  recoveryEmail(email: string){
    return this.getQuery(`send/email?email=${email}`);
  }

  getDevicesGeneralNew(device: string) {
    return this.getQuery(`main/last/position/devices?busqueda=${device}`)
      .pipe( map((data: any) => {
        return data;
      }));
  }

  // ListLastPosition

  getDevicesGeneral(tipo:string, device:string) {
    return this.getQuery(`main/last/position/list/device?tipo=${tipo}&valor=${localStorage.getItem("idu")}&busqueda=${device}`)
      .pipe( map(data => {
        return data['ListLastPosition'];
      }));
  }

  getInfoDevice(device: string) {
    return this.getQuery(`vehicle/general/information/trip?device=${device}`)
      .pipe( map(data => {
        return data;
      }));
    }

    getInfoDevice2(device: string){

      return this.getQuery(`configuration/details?device=${device}`)
        .pipe( map(data => {
          return data['Registry'];
        }));
  }

  getLastTravelsDevice(device: string) {
    return this.getQuery(`vehicle/list/trips?device=${device}`)
      .pipe( map(data => {
        return data['listItineraries'];
      }));
  }

  getRouteDevice(device: string, startDate: string, endDate: string) {
    return this.getQuery(`vehicle/stroke?device=${device}&startdate=${startDate}&enddate=${endDate}`);
  }

  getAllRoutesPerDate(device: string, startDate: string, endDate: string) {
    return this.getQuery(`vehicle/trips?device=${device}&startdate=${startDate}&enddate=${endDate}`)
      .pipe( map(data => {
        return data['listItineraries'];
      }));
  }

  getSubempresas() {
    return this.getQuery("main/subempresas")
      .pipe( map(data => {
        return data['listSubEmpresas'];
      }));
  }

  /**
   * GEOCERCAS
   */

  getListGeocercas() {
    return this.getQuery(`geo/fence/list`)
      .pipe( map(data => {
        return data['listGeoFence'];
      }));
  }

  getGeocerca(id: string) {
    return this.getQuery(`geo/fence?id=${id}`)
      .pipe( map(data => {
        return data['GeoFence'];
      }));
  }

  setNuevaGeocerca(data: any) {
    return this.postQuery(`geo/fence`, data);
  }

  setUpdateGeocerca(data: any) {
    return this.putQuery(`geo/fence`, data);
  }

  setDeleteGeocerca(id: string) {
    return this.deleteQuery(`geo/fence?id=${id}`);
  }

  /**
   * REPORTES
   */

  getReporteDispositivoIndividual(idu: string, dispositivo: string, grupo: string, fechaInicio: string, fechaFin: string) {
    return this.getQuery(`report/itinerarios/zip?param=${idu}&grupo=${grupo}&device=${dispositivo}&fechainicio=${fechaInicio}&fechafin=${fechaFin}`);
  }

  /**
   * GEOCERCAS Y VEHICULOS
   */

  getGeocercasAsignacion() {
    return this.getQuery(`asignacion/geo/fence`)
      .pipe( map(data => {
        return data['fence']['GeoFence'];
      }));
  }

  setNuevaAsignacion(data: any) {
    return this.postQuery(`asignacion/geo/fence`, data);
  }

  setDeleteAsignacion(data: any) {
    return this.postQuery(`asignacion/geo/fence/device`, data);
  }

  /**
   * USERS
   */

   getListUsers() {
     return this.getQuery("administration/users/list")
      .pipe( map(data => {
        return data['listUsers'];
      }));
   }

   getInfoUser(user: string) {
     return this.getQuery(`administration/users?idusername=${user}`)
      .pipe( map(data => {
        return data['user'];
      }));
   }
   getDevicesAdmin(compani: string) {
    return this.getQuery(`administration/alarms/devices?company=${compani}`)
    .pipe( map(data => {
      return data['ListDevices'];
    }));
   }

   getAlarmasAdmin(device: string, fechaInicio: string, fechafin: string){
    return this.getQuery(`administration/alarms/list?device=${device}&startdate=${fechaInicio}&enddate=${fechafin}`)
    .pipe( map(data => {
      return data['ListAlarms'];
    }));
  }

  getRutasAdmin(company: string, device: string, fechaIni: string, fechaFin: string){

    return this.getQuery(`administration/raw/data/list?company=${company}&device=${device}&startdate=${fechaIni}&enddate=${fechaFin}`)
    .pipe( map(data => {
      return data['ListRawData'];
    }));
   }

   setNewUser(data: any) {
     return this.postQuery("administration/users", data);
   }

   setUpdateUser(data: any) {
     return this.putQuery("administration/users", data);
   }

   /**
    * MANTENIMIENTO
    */


   getMechanics() {
     return this.getQuery(`administration/mechanics/list`)
     .pipe( map(data => {
       return data['ListMechanics'];
     }));
    }


    getInspecctionFolio(folio: string){
      return this.getQuery(`administration/inspection/list/folio?folio=${folio}`)
      .pipe( map(data => {
        return data['InspectionList'];
      }));
    }


    getPlantilla() {
      return this.getQuery(`administration/inspection/list/new`)
      .pipe( map(data => {
        return data['PlantillaHeader'];
      }));
   }

   postInspection(data: any){
    return this.postQuery("administration/inspection", data);
   }

   putInspeccion(data: any){
    return this.putQuery("administration/inspection", data);
   }

   getInspeccionList(){
    return this.getQuery(`administration/inspection/list`)
    .pipe( map(data => {
      return data['InspectionList'];
    }));
   }



   /**
    * MOVILIDAD
    */

   getResponsableVehicle(device: string){
      return this.getQuery(`configuration/responsible/device?device=${device}`)
      .pipe( map(data => {
        return data['registry'];
      }));
   }

   getPuntosInteresMovilidad(){
     return this.getQuery(`point/interest/list/service`)
     .pipe( map(data => {
       return data['ListPoints'];
     }));

   }

    getResponsablesMovilidad(device: string, fechaInicio: string, fechafin: string){
      return this.getQuery(`diary/responsibles/list?device=${device}&start=${fechaInicio}&end=${fechafin}`)
      .pipe( map(data => {
        return data['ListResponsibles'];
      }));
    }


    getAnalisisPuntosInteresMovilidad(punto: string, fechaInicio: string, fechafin: string, device: string){
      return this.getQuery(`point/interest/analysis/list?mongo=${punto}&start=${fechaInicio}&end=${fechafin}&device=${device}`)
      .pipe( map(data => {
        return data;
      }));
    }




    getNotificatonsAnaliticas(fechaInicio: string, fechafin: string){
      return this.getQuery(`dashboard/activity/day/list/notifications?start=${fechaInicio}&end=${fechafin}`);
    }





   /**
    * RESPONSABLES
    */

    deleteResponsable(id: number) {
      return this.deleteQuery(`configuration/responsible?id=${id}`);
    }

   /**
    * SUBUSERS
    */

    getListSubusers(user: string = "") {
      return this.getQuery(`administration/subusers/list?search=${user}`)
        .pipe( map(data => {
          return data['listUsers'];
        }));
    }

   /**
    * DISPOSITIVOS
    */

    getListSims() {
      return this.getQuery("sim/available")
        .pipe( map(data => {
          return data['listSims'];
        }));
    }

    getListDevicesConfiguration() {
      return this.getQuery("obd/admin/configuration/list/obd")
        .pipe( map(data => {
          return data['listObd'];
        }));
    }

    getListDevicesAdministration() {
      return this.getQuery("obd/admin/manager/list/obd")
        .pipe( map(data => {
          return data['listObd'];
        }));
    }

    getListDevices() {
      return this.getQuery("management/obd/list")
        .pipe( map(data => {
          return data['listObd'];
        }));
    }

    getDevice(id: string) {
      return this.getQuery(`management/obd?id=${id}`)
        .pipe( map(data => {
          return data['obd'];
        }));
    }

    setNewDevice(data: any) {
      return this.postQuery("management/obd", data);
    }

    setUpdateDevice(data: any) {
      return this.putQuery("management/obd", data);
    }

    getTypeDevices() {
      return this.getQuery("inventario/typedevices/list")
        .pipe( map(data => {
          return data['listTypeDevice'];
        }));
    }

    getCompanies() {
      return this.getQuery("administration/companies//list/company")
        .pipe( map(data => {
          return data['listCompany'];
        }));
    }

    setAssignmentSubempresa(data: any) {
      return this.putQuery("obd/admin/obd", data);
    }

    // EMPRESAS
    getListCompanies(){
      return this.getQuery("administration/company/list")
        .pipe( map(data => {
          return data['ListCompany'];
        }));
    }

    setCompanysAccess(data: any){
      return this.putQuery("administration/company", data);
    }

    /**
     * SUBEMPRESAS
     */

     getListSubempresas() {
       return this.getQuery("administration/subcompanies/list")
        .pipe( map(data => {
          return data['listSubCompany'];
        }));
     }

     getSubempresa(id: string) {
       return this.getQuery(`administration/subcompanies?id=${id}`);
     }

     setNewSubempresa(data: any) {
       return this.postQuery("administration/subcompanies", data);
     }

     setUpdateSubempresa(data: any) {
       return this.putQuery("administration/subcompanies ", data);
     }

     getPermissions(user: string){
      return this.getQuery(`configuration/permission/list?user=${user}`)
        .pipe( map(data => {
          return data['modules'];
        }));
    }

    setPermission(data: any){
      return this.postQuery('configuration/permission', data);
    }

     /**
      * RESPONSABLES
      */

     getListResponsables(responsible: string = '') {
       return this.getQuery(`configuration/responsible/list?search=${responsible}`)
        .pipe( map(data => {
          return data['listResponsible'];
        }));
     }

     getResponsable(id: number) {
       return this.getQuery(`configuration/responsible?id=${id}`)
        .pipe( map(data => {
          return data['registry'];
        }));
     }

     setNuevoResponsable(data: any) {
       return this.postQuery("configuration/responsible", data);
     }

     setUpdateResponsable(data: any) {
       return this.putQuery("configuration/responsible", data);
     }

     /**
      * ANALITICAS
      */

    getAnalitcs(fechaInicio: string, fechaFin: string, grupo: string, device: string) {
      return this.getQuery(`dashboard/distancia/litros/tiempo?fechainicio=${fechaInicio}&fechafin=${fechaFin}&grupo=${grupo}&device=${device}`);
    }

    getActivityDay(device: string){
      return this.getQuery(`dashboard/activity/day/list/devices?busqueda=${device}`)
        .pipe( map(data => {
          return data;
        }));
    }




    /**
     * EXTERNAL
     */

    getTripLink(device: string, startDate: string, endDate: string) {
      return this.getQuery(`vehicle/link?device=${device}&startdate=${startDate}&enddate=${endDate}`);
    }

    /**
     * NOTIFICATIONS
     */

    getCountNotifications() {
      return this.getQuery(`main/last/position/devices?busqueda=`);
    }

    getNotifications() {
      return this.getQuery("main/last/position/notifications/priority");
    }

    setChangeStatusNotification(id: string) {
      return this.putQuery(`notifications/priority?id=${id}`);
    }

    /**
      * PARAMETROS GENERALES CONFIGURACIÓN
      */

    getListParametrosGenerales(){
      return this.getQuery('configuration/list')
      .pipe( map(data => {
        return data['ListResgistry'];
      }));
    }

    getParametro(id: number){
      return this.getQuery(`configuration?id=${id}`)
        .pipe( map(data => {
          return data['Registry'];
        }));
    }

    setParametro(data: any){
      return this.putQuery('configuration', data);
    }

    /**
     * USUARIOS CONFIGURACION
     */

    setNewUsuarioConfiguracion(data: any) {
      return this.postQuery('administration/subusers', data);
    }

    setUpdateUsuarioConfiguracion(data: any) {
      return this.putQuery('administration/subusers', data);
    }

    getListUsuariosConfiguracion(value: string = '') {
      return this.getQuery(`administration/subusers/list?search=${value}`)
        .pipe( map(data => {
          return data['listUsers'];
        }));
    }

    getUsuarioConfiguracion(id: number) {
      return this.getQuery(`administration/subusers?id=${id}`)
        .pipe( map(data => {
          return data['user'];
        }));
    }

    getRoles() {
      return this.getQuery('administration/roles/list/roles')
        .pipe( map(data => {
          return data['listRoles'];
        }));
    }

    getEstatus() {
      return this.getQuery('administration/status/list')
        .pipe( map(data => {
          return data['listStatus'];
        }));
    }

    /**
     * CONTRASEÑA CONFIGURACION
     */

    setContrasenia(data: any){
      return this.putQuery('password/changepassword', data);
    }

    /**
     * Informacion de dispositivo
     */

    getListDispositivosInfo(device: string = ''){
      return this.getQuery(`configuration/details/list?search=${device}`)
        .pipe( map(data => {
          return data['ListDetails'];
        }));
    }

    getInfoDispositivioId(id: string){
        return this.getQuery(`configuration/details?device=${id}`)
          .pipe( map(data => {
            return data['Registry'];
          }));
    }

    getListRedInfoDevice(){
      return this.getQuery(`configuration/communication/methods/list`)
        .pipe( map(data => {
          return data['ListComMethods'];
        }));
    }

    getListTimeInfoDevice(){
      return this.getQuery(`configuration/sampling/time/list`)
        .pipe( map(data => {
          return data['ListSamTime'];
        }));
    }

    postInfoDevice(data: any) {
      return this.postQuery(`configuration/details`, data);
    }


    /**
     * PARO DE MOTOR CONFIGURACION
     */

    getEngineDevice(device: string){
      return this.getQuery(`engine/stop?device=${device}`)
        .pipe( map(data => {
          return data['EngineStop'];
        }));

    }

    getListEngineStop(device: string, page: number) {
      return this.getQuery(`engine/stop/list?search=${device}&page=${page}`)
        .pipe( map(data => {
          return data['ListEngineStops'];
        }));
      }

      getPagesEngineStop(name: string){
        return this.getQuery(`engine/stop/count?search=${name}`)
          .pipe( map((data: any) => {
            return data.NumberPages;
          }));
    }

    setStopEngine(device: string, status: number){
      return this.postQuery(`engine/stop/execute?device=${device}&status=${status}`);
    }

    /**
      * Logo Dinamico
      */

    setLogo(data: FormData){
      return this.postQuery('changed/logo', data);
    }

    updateLogo(data: FormData){
      return this.postQuery('changed/logo/image/update', data);
    }




     /**
      * VEHICULOS CONFIGURACION
      */

     setNewVersion(data: any) {
      return this.postQuery('administration/versions', data);
    }

    setUpdateVehiculoInfo(data: any){
      return this.postQuery('administration/addtional/data', data);
    }
    getInfoVehiculo(id: string) {
      return this.getQuery(`administration/addtional/data?device=${id}`)
        .pipe( map(data => {
          return data['addtional'];
        }));
    }
    getListInfoVehiculo(value: string = ''){
      return this.getQuery(`administration/addtional/data/list?search=${value}`)
        .pipe( map(data => {
          return data['ListAddtional'];
        }));
    }

    getMarcaVehiculo(){
      return this.getQuery('administration/trade/marks/list')
      .pipe(map(data => {
        return data['ListMarks'];
      }));
    }

    getModeloVehiculo(idMarca: string){
      return this.getQuery(`administration/models/list?mark=${idMarca}`)
      .pipe(map(data => {
        return data['ListModels'];
      }));
    }

    getVersionVehiculo(idModelo: string){
      return this.getQuery(`administration/versions/list?model=${idModelo}`)
      .pipe(map(data => {
        return data['ListVersions'];
      }));
    }
    getTipoVehiculo(){
      return this.getQuery('administration/type/vehicles/list')
      .pipe(map(data => {
        return data['ListTypeVehicles'];
      }));
    }
    /**
     * DASHBOARD
     */

     getDashboard(idu: string){
        return this.getQuery(`dashboard/general?valor=${idu}`)
        .pipe(map(data => {
          return data;
        }))
     }

     getGraphCombustible(grupo: string, device: string){
       return this.getQuery(`dashboard/general/consumption?group=${grupo}&device=${device}`)
       .pipe(map(data => {
         return data['GraficasConsumption'];
       }))
     }

    /**
     * MAPA DE CALOR
     */

    getHeatMap(startDate: string, endDate: string, group: string, device: string) {
      return this.getQuery(`heat/map?startdate=${startDate}&enddate=${endDate}&group=${group}&device=${device}`)
        .pipe( map(data => {
          return data['Coords'];
        }));
    }

    /**
     * Geocercas Monitoreo
     */

    getListGeocercasMonitoreo() {
      return this.getQuery(`geo/fence/monitoring/list`)
        .pipe( map(data => {
          return data['ListGeoFences'];
        }));
    }
    getListGeocercasHistorico(device: string, geo: string, fechaIni: string, fechaFin: string) {
      return this.getQuery(`geo/fence/history/list?device=${device}&mongo=${geo}&start=${fechaIni}&end=${fechaFin}`)
        .pipe( map(data => {
        return data['ListPointsTimeOut'];
      }));
    }

    getDevicesGeofenceMonitoreo(id: string) {
      return this.getQuery(`geo/fence/monitoring/last/positions?id=${id}`)
        .pipe( map(data => {
          return data['ListLastStatusDevice'];
        }));
    }




    /**
     * Carga Masiva Administración
     */

    sendCargaMasiva(idEmpresa: string, formData: FormData) {
      return this.postQuery(`bulk/load/Obds?empresa=${idEmpresa}`, formData);
    }

    /**
     * Agenda Configuración
     */

     getListEventosAgendaConfiguracion(startDate: string, endDate: string) {
       return this.getQuery(`configuration/diary/list?startdate=${startDate}&enddate=${endDate}`)
        .pipe( map(data => {
          return data['ListEvents'];
        }));
     }

     setNuevoEventoAgendaConfiguracion(event: any) {
       return this.postQuery('configuration/diary', event);
     }

     updateEventoAgendaConfiguracion(event: any) {
       return this.putQuery('configuration/diary', event);
     }

     deleteEventoAgendaConfiguracion(idStart: string, idEnd: string) {
       return this.deleteQuery(`configuration/diary?start=${idStart}&end=${idEnd}`);
     }

     /**
      * Rutas Configuración
      */



    postImportKml(formData: FormData){
      return this.postQuery('bulk/load/routes', formData);
    }

    getListRutasConfiguracion() {
      return this.getQuery('routes/list')
        .pipe( map(data => {
          return data['routes'];
        }));
    }

    getRutaConfiguracion(id: string) {
      return this.getQuery(`routes?id=${id}`)
        .pipe( map(data => {
          return data['routes'];
        }));
    }

    setNuevaRutaConfiguracion(data: any) {
      return this.postQuery('routes', data);
    }

    updateRutaConfiguracion(data: any) {
      return this.putQuery('routes', data);
    }

    deleteRutaConfiguracion(id: string) {
      return this.deleteQuery(`routes?id=${id}`);
    }

    /**
     * Puntos de Interes
     */

    postImportXSLX(formData: FormData){
      return this.postQuery('bulk/load/point/interest', formData);
    }

    getListPuntosInteresDispositivos() {
      return this.getQuery("assignment/point/interest")
        .pipe( map(data => {
          return data['PointInterest']['PointInterest'];
        }));
    }

    getListPuntosInteres() {
      return this.getQuery('point/interest/list')
        .pipe( map(data => {
          return data['ListPointInterest'];
        }));
    }

    getPuntoInteres(id: string) {
      return this.getQuery(`point/interest?id=${id}`)
        .pipe( map(data => {
          return data['PointInterest'];
        }));
    }

    setNuevoPuntoInteres(data: any) {
      return this.postQuery('point/interest', data);
    }

    updatePuntoInteres(data: any) {
      return this.putQuery('point/interest', data);
    }

    deletePuntoInteres(id: string) {
      return this.deleteQuery(`point/interest?id=${id}`);
    }

    /**
     * Vinculación Puntos de Interes
     */

    setVincularPuntoDispositivos(data: any) {
      return this.postQuery(`assignment/point/interest`, data);
    }

    setDesvincularDispositivosPunto(data:any) {
      return this.postQuery(`assignment/point/interest/device`, data);
    }

    /**
     * Agenda Rutas
     */

    getListEventsRoutesAgenda(start: string, end: string) {
      return this.getQuery(`configuration/routes/diary/list?startdate=${start}&enddate=${end}`)
        .pipe( map(data => {
          return data['ListEvents'];
        }));
    }

    getListRoutesAgenda() {
      return this.getQuery("configuration/routes/diary")
        .pipe( map(data => {
          return data['ListRoutes'];
        }));
    }

    setNuevoEventoRutaAgenda(data: any) {
      return this.postQuery("configuration/routes/diary", data);
    }

    updateEventoRutaAgenda(data: any) {
      return this.putQuery("configuration/routes/diary", data);
    }

    deleteEventoRutaAgenda(start: string, end: string) {
      return this.deleteQuery(`configuration/routes/diary?start=${start}&end=${end}`);
    }

}
