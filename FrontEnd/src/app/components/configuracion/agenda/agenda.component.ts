import { Component, OnInit, ViewChild } from '@angular/core';

import { CalendarOptions, FullCalendarComponent } from '@fullcalendar/angular';
import esLocale from '@fullcalendar/core/locales/es';

import * as moment from 'moment';
import $ from "jquery";
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-agenda',
  templateUrl: './agenda.component.html',
  styleUrls: ['./agenda.component.css']
})
export class AgendaComponent implements OnInit {

  @ViewChild('calendar') calendarComponent: FullCalendarComponent;

  calendarOptions: CalendarOptions = {
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

  listEvents: any = [];
  fechaAgenda: string;
  viewCalendar: string = "Month";
  startDateCalendar: string;
  endDateCalendar: string;

  eventUpdate: any;

  constructor(private spiderService: SpiderfleetService) {
  }

  ngOnInit(): void {
  }

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

    // console.log(this.startDateCalendar);
    // console.log(this.endDateCalendar);


    this.getEventsResponsables();
    this.getEventsRoutes();
  }

  async getEventsResponsables() {
    await this.spiderService.getListEventosAgendaConfiguracion(this.startDateCalendar, this.endDateCalendar)
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
    await this.spiderService.getListEventsRoutesAgenda(this.startDateCalendar, this.endDateCalendar)
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

  handleDateClick(arg) {
    this.fechaAgenda = arg.dateStr;
    document.getElementById("clickOptionsModal").click();
  }

}
