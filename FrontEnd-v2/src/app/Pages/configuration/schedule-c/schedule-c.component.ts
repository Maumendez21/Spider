import { Component, OnInit, ViewChild } from '@angular/core';
import { FullCalendarComponent, CalendarOptions } from '@fullcalendar/angular';
import esLocale from '@fullcalendar/core/locales/es';
import * as moment from 'moment';
import { MobilityService } from '../../mobility/services/mobility.service';

@Component({
  selector: 'app-schedule-c',
  templateUrl: './schedule-c.component.html',
  styleUrls: ['./schedule-c.component.css']
})
export class ScheduleCComponent implements OnInit {

  
  constructor(
    private mobilityService: MobilityService
  ) { }

  ngOnInit(): void {
  }

  @ViewChild('calendar') calendarComponent: FullCalendarComponent;
  public listEvents: any = [];
  public fechaAgenda: string;
  public viewCalendar: string = "Month";
  public startDateCalendar: string;
  public endDateCalendar: string;

  public eventUpdate: any;
  public calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    timeZone: 'America/Mexico_City',
    dayHeaderFormat: {
      weekday: 'short'
    },
    buttonText: {
      today: 'Hoy',
      month: 'Mes',
      week: 'Semana',
      day: 'Dia'
    },
    headerToolbar: {
      left: 'today prev,next',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay'
    },
    firstDay: 1,
    dayMaxEvents: 2,
    weekends: true,
    navLinks: true,
    editable: true,
    displayEventTime: false,
    allDayText: "Todo el dia",
    eventClick: this.handleEventClick.bind(this),
    locale: esLocale,
    dateClick: this.handleDateClick.bind(this), // bind is important!F
    events: [],
  };

  ngAfterViewInit() {
    this.getRangeDates(false);
    this.initClicksEvents();
  }

  initClicksEvents() {
    $('.fc-today-button').on('click', x => {
      this.getRangeDates(false);
    });

    $('.fc-prev-button').on('click', x => {
      this.getRangeDates(true);
    });

    $('.fc-next-button').on('click', x => {
      this.getRangeDates(true);
    });

    $('.fc-dayGridMonth-button').on('click', x => {
      this.viewCalendar = "Month";
      this.getRangeDates(true);
    });

    $('.fc-timeGridWeek-button').on('click', x => {
      this.viewCalendar = "Week";
      this.getRangeDates(true);
    });

    $('.fc-timeGridDay-button').on('click', x => {
      this.viewCalendar = "Day";
      this.getRangeDates(true);
    });
  }

  getRangeDates = (addDay: boolean) => {

    const calendarApi = this.calendarComponent.getApi();
    const date = (addDay) ? moment(calendarApi.getDate()).add(1, 'days').format("YYYY-MM-DD") : moment(calendarApi.getDate()).format("YYYY-MM-DD");

    switch (this.viewCalendar) {

      case "Month":

        this.startDateCalendar = moment(date).clone().startOf('month').format('YYYY-MM-DDTHH:mm');
        this.endDateCalendar = moment(date).clone().endOf('month').format('YYYY-MM-DDTHH:mm');
        break;

      case "Week":

        this.startDateCalendar = moment(date).clone().startOf('isoWeek').format('YYYY-MM-DDTHH:mm');
        this.endDateCalendar = moment(date).clone().endOf('isoWeek').format('YYYY-MM-DDTHH:mm');
        break;

      case "Day":
        this.startDateCalendar = moment(date).clone().startOf('day').format('YYYY-MM-DD')+"T00:00";
        this.endDateCalendar = moment(date).clone().startOf('day').format('YYYY-MM-DD')+"T23:59";
        break;
      default:
        break;
    }
    this.getEvents();
  }

  
  getEvents() {
    this.listEvents = [];
    this.getEventsResponsables();
    this.getEventsRoutes();
  }

  async getEventsResponsables() {
    await this.mobilityService.getListEventosAgenda(this.startDateCalendar, this.endDateCalendar)
    .subscribe(events => {
      events.forEach(event => {
        event.title = (event.Vehicle) ? event.Vehicle : event.Device;
        event.start = event.StartDate;
        event.end = event.EndDate;
        event.color = '#64b5f6';
        event.Tipo = "Responsable";
      });
      this.listEvents.push(...events);
    });
  }

  async getEventsRoutes() {
    await this.mobilityService.getListEventsRoutesAgenda(this.startDateCalendar, this.endDateCalendar)
      .subscribe(events => {

        events.forEach(event => {
          event.title = event.Name;
          event.start = event.StartDate;
          event.end = event.EndDate;
          event.color = '#4caf50';
          event.Tipo = "Ruta";
        });

        this.listEvents.push(...events);
        this.calendarOptions.events = this.listEvents;
      });
  }

  handleEventClick(arg) {
    this.eventUpdate = arg.event._def.extendedProps;

    if (arg.event._def.extendedProps.Tipo == "Responsable") {
      document.getElementById("clickActualizarEventoModal").click();
    } else {
      document.getElementById("clickActualizarEventoRutaModal").click();
    }

  }

  create: boolean = false;

  handleDateClick(arg) {
    this.create = true;
    this.fechaAgenda = arg.dateStr;
    document.getElementById("clickOptionsModal").click();
  }

}
