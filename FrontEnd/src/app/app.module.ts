import { BrowserModule } from '@angular/platform-browser';
import { NgModule, LOCALE_ID } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { registerLocaleData, DatePipe } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import localeEs from '@angular/common/locales/es';
import { AgmCoreModule, PolygonManager, GoogleMapsAPIWrapper } from '@agm/core';
import { AgmDirectionModule } from 'agm-direction';
import { ChartsModule } from 'ng2-charts';
import { DataTablesModule } from 'angular-datatables';
import { environment } from 'src/environments/environment';
import { UiSwitchModule } from 'ngx-toggle-switch';
import { Maps } from './helpers/maps';
import { Icons } from './helpers/iconos';// fonts provided for pdfmake

registerLocaleData(localeEs);
// Set the fonts to use


// * Router
import { ROUTES } from './app.routes';

// * Core
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';

// * Navs
import { SidebarComponent } from './components/shared/sidebar/sidebar/sidebar.component';
import { NavbarComponent } from './components/shared/navbar/navbar/navbar.component';
import { SidebarRightFiltersMapaComponent } from './components/shared/sidebarRightFiltersMapa/sidebar-right-filters-mapa/sidebar-right-filters-mapa.component';
import { BottombarComponent } from './components/shared/bottombar/bottombar.component';

// * Filters
import { FilterSidebarRightPipe } from './pipes/filter-sidebar-right.pipe';

// * External
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { TreeModule } from '@circlon/angular-tree-component';
import { JwPaginationModule } from 'jw-angular-pagination';
import { ToastrModule } from 'ngx-toastr';

// * Pipes
import { NotextPipe } from './pipes/notext.pipe';
import { TranslatePipe } from './pipes/translate.pipe';

// * Shared
import { MainMapComponent } from './components/shared/mapa/main-map/main-map.component';

// * Components
import { LoginComponent } from './components/login/login/login.component';
import { MapaComponent } from './components/mapa/mapa.component';
import { NuevaGeocercaComponent } from './components/configuracion/geocercas/nueva-geocerca/nueva-geocerca.component';
import { GeocercasComponent } from './components/configuracion/geocercas/geocercas/geocercas.component';
import { ActualizarGeocercaComponent } from './components/configuracion/geocercas/actualizar-geocerca/actualizar-geocerca.component';
import { EliminarAsignacionGeocercaComponent } from './components/configuracion/geocercas/eliminar-asignacion-geocerca/eliminar-asignacion-geocerca.component';
import { AsignacionGeocercasComponent } from './components/configuracion/geocercas/asignacion-geocercas/asignacion-geocercas.component';
import { ConfigurationMenuLgComponent } from './components/shared/menu-lg/configuration-menu-lg/configuration-menu-lg.component';
import { JwPaginationCustomComponent } from './components/shared/pagination/jw-pagination-custom/jw-pagination-custom.component';
import { DevicesComponent } from './components/administracion/devices/devices.component';
import { AdministracionMenuLgComponent } from './components/shared/menu-lg/administracion-menu-lg/administracion-menu-lg.component';
import { SubempresasComponent } from './components/configuracion/subempresas/subempresas.component';
import { DevicesConfigurationComponent } from './components/configuracion/devices-configuration/devices-configuration.component';
import { ResponsablesComponent } from './components/configuracion/responsables/responsables.component';
import { CreateResponsableModalComponent } from './components/shared/modals/responsables/create-responsable-modal/create-responsable-modal.component';
import { UpdateResponsableModalComponent } from './components/shared/modals/responsables/update-responsable-modal/update-responsable-modal.component';
import { FilterEstatusMainMapComponent } from './components/shared/modals/filters-main-map/filter-estatus-main-map/filter-estatus-main-map.component';
import { AnaliticaComponent } from './components/analitica/analitica.component';
import { GraphBarrasAnaliticaComponent } from './components/shared/graphics/analitica/graph-barras-analitica/graph-barras-analitica.component';
import { FilterVehiculosAnaliticaPipe } from './pipes/filter-vehiculos-analitica.pipe';
import { ViajeExternoComponent } from './components/externo/viaje-externo/viaje-externo.component';
import { CardTotalComponent } from './components/shared/cards/analitica/card-total/card-total.component';
import { UsersComponent } from './components/administracion/users/users.component';
import { NotificacionesComponent } from './components/notificaciones/notificaciones.component';
import { UsuariosComponent } from './components/configuracion/usuarios/usuarios.component';
import { CreateUsuarioModalComponent } from './components/shared/modals/usuarios-configuracion/create-usuario-modal/create-usuario-modal.component';
import { UpdateUsuarioModalComponent } from './components/shared/modals/usuarios-configuracion/update-usuario-modal/update-usuario-modal.component';
import { AnaliticaNavComponent } from './components/shared/navs/analiticas/analitica-nav/analitica-nav.component';
import { HeatmapNavComponent } from './components/shared/navs/analiticas/heatmap-nav/heatmap-nav.component';
import { VehiculoInfoComponent } from './components/configuracion/vehiculo-info/vehiculo-info.component';
import { UpdateVehiculoInfoComponent } from './components/shared/modals/vehiculosConfig/update-vehiculo-info/update-vehiculo-info.component';
import { GeocercasMonitoreoComponent } from './components/geocercas-monitoreo/geocercas-monitoreo.component';
import { DashboardNavComponent } from './components/shared/navs/analiticas/dashboard-nav/dashboard-nav.component';
import { CardDashboardComponent } from './components/shared/cards/analitica/card-dashboard/card-dashboard.component';
import { GraphBarrasDashboardComponent } from './components/shared/graphics/analitica/graph-barras-dashboard/graph-barras-dashboard.component';
import { GraphLineDashboardComponent } from './components/shared/graphics/analitica/graph-line-dashboard/graph-line-dashboard.component';
import { CargaMasivaComponent } from './components/administracion/carga-masiva/carga-masiva.component'

import { AgendaComponent } from './components/configuracion/agenda/agenda.component';
import { FullCalendarModule } from '@fullcalendar/angular';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import timeGridPlugin from '@fullcalendar/timegrid';
import { NuevoEventoComponent } from './components/shared/agenda/nuevo-evento/nuevo-evento.component';
import { ActualizarEventoComponent } from './components/shared/agenda/actualizar-evento/actualizar-evento.component';

import { RutasComponent } from './components/configuracion/rutas/rutas/rutas.component';
import { NuevaRutaComponent } from './components/configuracion/rutas/nueva-ruta/nueva-ruta.component';
import { ActualizarRutaComponent } from './components/configuracion/rutas/actualizar-ruta/actualizar-ruta.component';
import { AgregarVersionComponent } from './components/shared/modals/agregar-version/agregar-version.component';
import { AsignarPermisosComponent } from './components/shared/modals/asignar-permisos/asignar-permisos.component';
import { EmpresasComponent } from './components/administracion/empresas/empresas.component';
import { FormularioRecuperacionComponent } from './components/login/formulario-recuperacion/formulario-recuperacion.component';
import { ChanguePasswordComponent } from './components/configuracion/changue-password/changue-password.component';
import { ChangeLogotipoComponent } from './components/configuracion/change-logotipo/change-logotipo.component';
import { ParametrosGeneralesComponent } from './components/configuracion/parametros-generales/parametros-generales.component';
import { ParametrosGeneralesModalComponent } from './components/shared/modals/parametros-generales-modal/parametros-generales-modal.component';
import { ModalInfoDeviceComponent } from './components/shared/mapa/modal-info-device/modal-info-device.component';
import { ModalFiltroFechasComponent } from './components/shared/mapa/modal-filtro-fechas/modal-filtro-fechas.component';
import { PuntosInteresComponent } from './components/configuracion/puntos-interes/puntos-interes/puntos-interes.component';
import { NuevoPuntoInteresComponent } from './components/configuracion/puntos-interes/nuevo-punto-interes/nuevo-punto-interes.component';
import { ActualizarPuntoInteresComponent } from './components/configuracion/puntos-interes/actualizar-punto-interes/actualizar-punto-interes.component';
import { AsignacionPuntosInteresComponent } from './components/configuracion/puntos-interes/asignacion-puntos-interes/asignacion-puntos-interes.component';
import { EliminarAsignacionPuntosInteresComponent } from './components/configuracion/puntos-interes/eliminar-asignacion-puntos-interes/eliminar-asignacion-puntos-interes.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { OpcionesAgendaModalComponent } from './components/shared/agenda/opciones-agenda-modal/opciones-agenda-modal.component';
import { NuevoEventoRutaAgendaModalComponent } from './components/shared/agenda/nuevo-evento-ruta-agenda-modal/nuevo-evento-ruta-agenda-modal.component';
import { ActualizarEventoRutaAgendaModalComponent } from './components/shared/agenda/actualizar-evento-ruta-agenda-modal/actualizar-evento-ruta-agenda-modal.component';
import { AlarmasComponent } from './components/administracion/alarmas/alarmas.component';
import { RutasAdminComponent } from './components/administracion/rutas-admin/rutas-admin.component';
import { MovilidadComponent } from './components/movilidad/movilidad.component';
import { ResponsablesNavComponent } from './components/shared/navs/movilidad/responsables-nav/responsables-nav.component';
import { FiltradoFechasComponent } from './components/shared/forms/filtrado-movilidad/filtrado-fechas.component';
import { AnalisisPuntoInteresComponent } from './components/shared/navs/movilidad/analisis-punto-interes/analisis-punto-interes.component';
import { CountArrayPipe } from './pipes/count-array.pipe';
import { ParoDeMotorComponent } from './components/configuracion/paro-de-motor/paro-de-motor.component';
import { DayActivityComponent } from './components/shared/navs/analiticas/day-activity/day-activity.component';
import { NotificationsNavComponent } from './components/shared/navs/analiticas/notifications-nav/notifications-nav.component';
import { NotificationsSharedComponent } from './components/shared/forms/notifications-shared/notifications-shared.component';
import { LoadingComponent } from './components/shared/cards/loading/loading.component';
import { MantenimientoComponent } from './components/mantenimiento/mantenimiento.component';
import { GeocercaTiempoRealNavComponent } from './components/shared/navs/geocercas/geocerca-tiempo-real-nav/geocerca-tiempo-real-nav.component';
import { GeocercaHistoricoNavComponent } from './components/shared/navs/geocercas/geocerca-historico-nav/geocerca-historico-nav.component';
import { AgendaNavComponent } from './components/shared/navs/movilidad/agenda-nav/agenda-nav.component';
import { InspeccionesNavComponent } from './components/shared/navs/mantenimiento/inspecciones-nav/inspecciones-nav.component';
import { OrdenTrabajoNavComponent } from './components/shared/navs/mantenimiento/orden-trabajo-nav/orden-trabajo-nav.component';
import { AnalisisRutasNavComponent } from './components/shared/navs/movilidad/analisis-rutas-nav/analisis-rutas-nav.component';
import { RutasProgramadasNavComponent } from './components/shared/navs/movilidad/rutas-programadas-nav/rutas-programadas-nav.component';
import { ExcelPuntoInteresComponent } from './components/shared/modals/excel-punto-interes/excel-punto-interes.component';
import { KmlRutasComponent } from './components/shared/modals/kml-rutas/kml-rutas.component';
import { ReporteTabComponent } from './components/shared/forms/reporte-tab/reporte-tab.component';
import { DispositivoInfoComponent } from './components/configuracion/dispositivo-info/dispositivo-info.component';
import { UpdateDeviceInfoComponent } from './components/shared/modals/vehiculosConfig/update-device-info/update-device-info.component';
import { NuevaInspeccionComponent } from './components/shared/forms/mantenimientoForms/nueva-inspeccion/nueva-inspeccion.component';
import { InspeccionFolioComponent } from './components/shared/navs/mantenimiento/inspecciones-nav/inspeccion-folio/inspeccion-folio.component';
import { GraphVelocidadComponent } from './components/shared/graphics/detalleViaje/graph-velocidad/graph-velocidad.component';




/*import { AuthGuardService } from './auth/auth-guard.service';

import { JwtModule } from '@auth0/angular-jwt';

export function tokenGetter() {
  return localStorage.getItem("token");
}*/

FullCalendarModule.registerPlugins([ // register FullCalendar plugins
  dayGridPlugin, interactionPlugin, timeGridPlugin
]);


@NgModule({
  declarations: [
    AppComponent,
    SidebarComponent,
    NavbarComponent,
    SidebarRightFiltersMapaComponent,
    LoginComponent,
    FilterSidebarRightPipe,
    MapaComponent,
    BottombarComponent,
    NotextPipe,
    MainMapComponent,
    GeocercasComponent,
    ActualizarGeocercaComponent,
    AsignacionGeocercasComponent,
    UsersComponent,
    NuevaGeocercaComponent,
    EliminarAsignacionGeocercaComponent,
    ConfigurationMenuLgComponent,
    DevicesComponent,
    JwPaginationCustomComponent,
    AdministracionMenuLgComponent,
    SubempresasComponent,
    DevicesConfigurationComponent,
    ResponsablesComponent,
    CreateResponsableModalComponent,
    UpdateResponsableModalComponent,
    FilterEstatusMainMapComponent,
    AnaliticaComponent,
    GraphBarrasAnaliticaComponent,
    FilterVehiculosAnaliticaPipe,
    ViajeExternoComponent,
    CardTotalComponent,
    NotificacionesComponent,
    UsuariosComponent,
    CreateUsuarioModalComponent,
    UpdateUsuarioModalComponent,
    AnaliticaNavComponent,
    HeatmapNavComponent,
    VehiculoInfoComponent,
    UpdateVehiculoInfoComponent,
    GeocercasMonitoreoComponent,
    DashboardNavComponent,
    CardDashboardComponent,
    GraphBarrasDashboardComponent,
    GraphLineDashboardComponent,
    TranslatePipe,
    AgendaComponent,
    CargaMasivaComponent,
    NuevoEventoComponent,
    ActualizarEventoComponent,
    RutasComponent,
    NuevaRutaComponent,
    ActualizarRutaComponent,
    AgregarVersionComponent,
    AsignarPermisosComponent,
    EmpresasComponent,
    FormularioRecuperacionComponent,
    ChanguePasswordComponent,
    ChangeLogotipoComponent,
    ParametrosGeneralesComponent,
    ParametrosGeneralesModalComponent,
    ModalInfoDeviceComponent,
    ModalFiltroFechasComponent,
    PuntosInteresComponent,
    NuevoPuntoInteresComponent,
    ActualizarPuntoInteresComponent,
    AsignacionPuntosInteresComponent,
    EliminarAsignacionPuntosInteresComponent,
    OpcionesAgendaModalComponent,
    NuevoEventoRutaAgendaModalComponent,
    ActualizarEventoRutaAgendaModalComponent,
    AlarmasComponent,
    RutasAdminComponent,
    MovilidadComponent,
    ResponsablesNavComponent,
    FiltradoFechasComponent,
    AnalisisPuntoInteresComponent,
    CountArrayPipe,
    ParoDeMotorComponent,
    DayActivityComponent,
    NotificationsNavComponent,
    NotificationsSharedComponent,
    LoadingComponent,
    MantenimientoComponent,
    GeocercaTiempoRealNavComponent,
    GeocercaHistoricoNavComponent,
    AgendaNavComponent,
    InspeccionesNavComponent,
    OrdenTrabajoNavComponent,
    AnalisisRutasNavComponent,
    RutasProgramadasNavComponent,
    ExcelPuntoInteresComponent,
    KmlRutasComponent,
    ReporteTabComponent,
    DispositivoInfoComponent,
    UpdateDeviceInfoComponent,
    NuevaInspeccionComponent,
    InspeccionFolioComponent,
    GraphVelocidadComponent,
  ],
  imports: [
    UiSwitchModule,
    HttpClientModule,
    FormsModule,
    BrowserModule,
    FontAwesomeModule,
    ChartsModule,
    DataTablesModule,
    NgSelectModule,
    AgmCoreModule.forRoot({
      apiKey: environment.apiKey,
      libraries: environment.libraries
    }),
    AgmDirectionModule,
    RouterModule.forRoot(ROUTES, { useHash: true }),
    ToastrModule.forRoot(),
    BrowserAnimationsModule,
    JwPaginationModule,
    TreeModule,
    FullCalendarModule,
    /*JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:4200", "http://spiderfleetapi.azurewebsites.net/"]
      }
    }),*/
  ],
  providers: [
    //AuthGuardService,
    DatePipe,
    GoogleMapsAPIWrapper,
    PolygonManager,
    FilterSidebarRightPipe,
    {
      provide: LOCALE_ID,
      useValue: 'es'
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
