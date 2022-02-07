import { Routes } from '@angular/router';

import { LoginComponent } from './components/login/login/login.component';

import { MapaComponent } from './components/mapa/mapa.component';

import { NuevaGeocercaComponent } from './components/configuracion/geocercas/nueva-geocerca/nueva-geocerca.component';
import { GeocercasComponent  } from './components/configuracion/geocercas/geocercas/geocercas.component';
import { ActualizarGeocercaComponent } from './components/configuracion/geocercas/actualizar-geocerca/actualizar-geocerca.component';
import { EliminarAsignacionGeocercaComponent } from './components/configuracion/geocercas/eliminar-asignacion-geocerca/eliminar-asignacion-geocerca.component';
import { AsignacionGeocercasComponent } from './components/configuracion/geocercas/asignacion-geocercas/asignacion-geocercas.component';

import { SubempresasComponent } from './components/configuracion/subempresas/subempresas.component';
import { DevicesConfigurationComponent } from './components/configuracion/devices-configuration/devices-configuration.component';
import { ResponsablesComponent } from './components/configuracion/responsables/responsables.component';

import { DevicesComponent } from './components/administracion/devices/devices.component';
import { AnaliticaComponent } from './components/analitica/analitica.component';
import { ViajeExternoComponent } from './components/externo/viaje-externo/viaje-externo.component';
import { UsersComponent } from './components/administracion/users/users.component';
import { NotificacionesComponent } from './components/notificaciones/notificaciones.component';
import { UsuariosComponent } from './components/configuracion/usuarios/usuarios.component';
import { VehiculoInfoComponent } from './components/configuracion/vehiculo-info/vehiculo-info.component';
import { GeocercasMonitoreoComponent } from './components/geocercas-monitoreo/geocercas-monitoreo.component';
import { AgendaComponent } from './components/configuracion/agenda/agenda.component';

//import { AuthGuardService as AuthGuard } from './auth/auth-guard.service';
import { CargaMasivaComponent } from './components/administracion/carga-masiva/carga-masiva.component';
import { EmpresasComponent } from './components/administracion/empresas/empresas.component';
import { FormularioRecuperacionComponent } from './components/login/formulario-recuperacion/formulario-recuperacion.component';
import { ChanguePasswordComponent } from './components/configuracion/changue-password/changue-password.component';


import { RutasComponent } from './components/configuracion/rutas/rutas/rutas.component';
import { NuevaRutaComponent } from './components/configuracion/rutas/nueva-ruta/nueva-ruta.component';
import { ActualizarRutaComponent } from './components/configuracion/rutas/actualizar-ruta/actualizar-ruta.component';
import { ChangeLogotipoComponent } from './components/configuracion/change-logotipo/change-logotipo.component';
import { ParametrosGeneralesComponent } from './components/configuracion/parametros-generales/parametros-generales.component';

import { PuntosInteresComponent } from './components/configuracion/puntos-interes/puntos-interes/puntos-interes.component';
import { NuevoPuntoInteresComponent } from './components/configuracion/puntos-interes/nuevo-punto-interes/nuevo-punto-interes.component';
import { ActualizarPuntoInteresComponent } from './components/configuracion/puntos-interes/actualizar-punto-interes/actualizar-punto-interes.component';
import { AsignacionPuntosInteresComponent } from './components/configuracion/puntos-interes/asignacion-puntos-interes/asignacion-puntos-interes.component';
import { EliminarAsignacionPuntosInteresComponent } from './components/configuracion/puntos-interes/eliminar-asignacion-puntos-interes/eliminar-asignacion-puntos-interes.component';
import { AlarmasComponent } from './components/administracion/alarmas/alarmas.component';
import { RutasAdminComponent } from './components/administracion/rutas-admin/rutas-admin.component';
import { MovilidadComponent } from './components/movilidad/movilidad.component';
import { ParoDeMotorComponent } from './components/configuracion/paro-de-motor/paro-de-motor.component';
import { NotificationsNavComponent } from './components/shared/navs/analiticas/notifications-nav/notifications-nav.component';
import { DayActivityComponent } from './components/shared/navs/analiticas/day-activity/day-activity.component';
import { HeatmapNavComponent } from './components/shared/navs/analiticas/heatmap-nav/heatmap-nav.component';
import { AnaliticaNavComponent } from './components/shared/navs/analiticas/analitica-nav/analitica-nav.component';
import { MantenimientoComponent } from './components/mantenimiento/mantenimiento.component';
import { GeocercaTiempoRealNavComponent } from './components/shared/navs/geocercas/geocerca-tiempo-real-nav/geocerca-tiempo-real-nav.component';
import { GeocercaHistoricoNavComponent } from './components/shared/navs/geocercas/geocerca-historico-nav/geocerca-historico-nav.component';
import { AgendaNavComponent } from './components/shared/navs/movilidad/agenda-nav/agenda-nav.component';
import { ResponsablesNavComponent } from './components/shared/navs/movilidad/responsables-nav/responsables-nav.component';
import { AnalisisPuntoInteresComponent } from './components/shared/navs/movilidad/analisis-punto-interes/analisis-punto-interes.component';
import { InspeccionesNavComponent } from './components/shared/navs/mantenimiento/inspecciones-nav/inspecciones-nav.component';
import { OrdenTrabajoNavComponent } from './components/shared/navs/mantenimiento/orden-trabajo-nav/orden-trabajo-nav.component';
import { AnalisisRutasNavComponent } from './components/shared/navs/movilidad/analisis-rutas-nav/analisis-rutas-nav.component';
import { RutasProgramadasNavComponent } from './components/shared/navs/movilidad/rutas-programadas-nav/rutas-programadas-nav.component';
import { DispositivoInfoComponent } from './components/configuracion/dispositivo-info/dispositivo-info.component';
import { NuevaInspeccionComponent } from './components/shared/forms/mantenimientoForms/nueva-inspeccion/nueva-inspeccion.component';
import { InspeccionFolioComponent } from './components/shared/navs/mantenimiento/inspecciones-nav/inspeccion-folio/inspeccion-folio.component';

export const ROUTES: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'recovery', component: FormularioRecuperacionComponent },
    { path: 'mapa', component: MapaComponent },//, canActivate: [AuthGuard] },
    { path: 'analitica', component: AnaliticaComponent },
    { path: 'analitica/notifications', component: NotificationsNavComponent },
    { path: 'analitica/activityDay', component: DayActivityComponent },
    { path: 'analitica/heatMap', component: HeatmapNavComponent },
    { path: 'analitica/rendimiento', component: AnaliticaNavComponent },
    { path: 'geocercas', component: GeocercasMonitoreoComponent },
    { path: 'geocercas/monitoreo', component: GeocercaTiempoRealNavComponent },
    { path: 'geocercas/historico', component: GeocercaHistoricoNavComponent },
    { path: 'movilidad', component: MovilidadComponent },
    { path: 'movilidad/agenda', component: AgendaNavComponent },
    { path: 'movilidad/responsables', component: ResponsablesNavComponent },
    { path: 'movilidad/puntos-interes', component: AnalisisPuntoInteresComponent },
    { path: 'movilidad/analisis', component: AnalisisRutasNavComponent },
    { path: 'movilidad/rutas-programadas', component: RutasProgramadasNavComponent },
    { path: 'mantenimiento', component: MantenimientoComponent },
    { path: 'mantenimiento/inspecciones', component: InspeccionesNavComponent },
    { path: 'mantenimiento/inspeccion', component: NuevaInspeccionComponent },
    { path: 'mantenimiento/inspeccion/:folio', component: InspeccionFolioComponent },
    { path: 'mantenimiento/ordenes', component: OrdenTrabajoNavComponent },
    { path: 'configuration/agenda', component: AgendaComponent  },
    { path: 'configuration/grupos', component: SubempresasComponent },
    { path: 'configuration/devices', component: DevicesConfigurationComponent },
    { path: 'configuration/responsables', component: ResponsablesComponent },
    { path: 'configuration/usuarios', component: UsuariosComponent },
    { path: 'configuration/vehiculoInfo', component: VehiculoInfoComponent},
    { path: 'configuration/dispositivoInfo', component: DispositivoInfoComponent},
    { path: 'configuration/parametros-generales', component: ParametrosGeneralesComponent},
    { path: 'configuration/geocercas', component: GeocercasComponent },
    { path: 'configuration/nueva-geocerca', component: NuevaGeocercaComponent },
    { path: 'configuration/actualizar-geocerca/:id', component: ActualizarGeocercaComponent },
    { path: 'configuration/nueva-asignacion', component: AsignacionGeocercasComponent },
    { path: 'configuration/desvincular-asignacion', component: EliminarAsignacionGeocercaComponent },
    { path: 'configuration/rutas', component: RutasComponent },
    { path: 'configuration/nueva-ruta', component: NuevaRutaComponent },
    { path: 'configuration/actualizar-ruta/:id', component: ActualizarRutaComponent },
    { path: 'configuration/logotipo-change', component: ChangeLogotipoComponent },
    { path: 'configuration/password-change', component: ChanguePasswordComponent },
    { path: 'configuration/paro-motor', component: ParoDeMotorComponent },
    { path: 'configuration/puntos-interes', component: PuntosInteresComponent },
    { path: 'configuration/nuevo-punto-interes', component: NuevoPuntoInteresComponent },
    { path: 'configuration/actualizar-punto-interes/:id', component: ActualizarPuntoInteresComponent },
    { path: 'configuration/puntos-interes/asignacion', component: AsignacionPuntosInteresComponent },
    { path: 'configuration/puntos-interes/desvincular-asignacion', component: EliminarAsignacionPuntosInteresComponent },
    { path: 'administration/devices', component: DevicesComponent },
    { path: 'administration/users', component: UsersComponent },
    { path: 'administration/cargamasiva', component: CargaMasivaComponent },
    { path: 'administration/empresas', component: EmpresasComponent },
    { path: 'administration/alarmas', component: AlarmasComponent },
    { path: 'administration/rutasAdmin', component: RutasAdminComponent },
    { path: 'trip/:device/:startDate/:endDate', component: ViajeExternoComponent },
    { path: 'notifications', component: NotificacionesComponent },
    // Restrictions
    { path: '', pathMatch: 'full', redirectTo: 'login' },
    { path: '**', pathMatch: 'full', redirectTo: 'login' }
];
