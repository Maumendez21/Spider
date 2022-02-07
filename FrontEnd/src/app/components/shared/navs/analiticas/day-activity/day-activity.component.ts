import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';
import * as jspdf from 'jspdf';
import html2canvas from 'html2canvas';

@Component({
  selector: 'app-day-activity',
  templateUrl: './day-activity.component.html',
  styleUrls: ['./day-activity.component.css']
})
export class DayActivityComponent implements OnInit {

  data: any[];
  unidadesActivas: string;
  unidadesNoActivas: string;
  notifications: string;
  notificationes: any[] = [];
  cargando: boolean = true;

  unidades: string;


  dtTrigger = new Subject();
  dtOptions: DataTables.Settings = {};

  constructor(private spiderService: SpiderfleetService, private router: Router, private shared: SharedService) {
    this.limpiarFiltrosMapa();
    if (shared.verifyLoggin()) {
      this.getNotifications();
      this.getActivityDat(this.deviceSearch);
      this.shared.notificationsStream$.subscribe(data => {
        this.notifications = data;
      })
    } else {
      this.router.navigate(['/login']);
    }
  }

  limpiarFiltrosMapa() {

    this.shared.limpiarFiltros();

    if (!document.getElementById("sidebarRight").classList.toggle("active")) {
      document.getElementById("sidebarRight").classList.toggle("active")
    }
  }

  buscarDevice(){
    // this.getActivityDat(this.deviceSearch)
    this.spiderService.getActivityDay(this.deviceSearch).subscribe(data => {
      this.data = data['ListDay'];
    })
  }

  getActivityDat(device: string){
    this.cargando = true;
    this.spiderService.getActivityDay(device).subscribe(data => {


      this.unidades = data['Actives'] + "/" + data['Inactives'];
      this.data = data['ListDay'];
      // console.log(this.data);

      this.cargando = false;
    })
  }

  deviceSearch: string = "";
  pageOfItems:Array<any>;

  onChangePage(pageOfItems: Array<any>) {
    this.pageOfItems = pageOfItems;
  }

  async getNotifications() {

    this.dtOptions = {
      pagingType: 'full_numbers',
      pageLength: 10,
      ordering: true,
      responsive: true,
      language: {
        'search': "Buscar",
        'paginate': {
          'first': 'Primero',
          'previous': 'Anterior',
          'next': 'Siguiente',
          'last': 'Ultimo'
        },
        'lengthMenu': 'Mostrar _MENU_ documentos',
        'info': 'Mostrando _PAGE_ de _PAGES_'
      },
    };

    await this.spiderService.getNotifications()
      .subscribe(data => {


        this.notificationes = data['listNotifications'];
        this.dtTrigger.next();
      }, error => {
        this.shared.broadcastLoggedStream(false);
        this.shared.clearSharedSession();
        this.router.navigate(['/login']);
      });
  }


  showDetailTravel(device: string, startDate: string, endDate: string): string {
    // if (index+1 <= this.lastTravels.length-1) {
    //   startDate = (this.diffTimeTravelStartEnd(startDate, this.lastTravels[index+1]['EndDate'], 30)) ? this.lastTravels[index+1]['EndDate'] : startDate;
    // }
    return `/trip/${device}/${startDate}/${endDate}`;
  }

  getBase64(img): string{
    var canvas = document.createElement('canvas');
    canvas.width = img.width;
    canvas.height = img.height;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0);
    const dataurl = canvas.toDataURL();
    return dataurl;
  }



  generatePDF(){
    var data = document.getElementById("pdf");

    html2canvas(data).then(canvas => {

      var imgWidth = 208;
      var paheHeight = 295;
      var imgHeight = canvas.height * imgWidth / canvas.width;
      var heightLeft = imgHeight;

      // var base64 = this.getBase64(document.getElementById('logotipo'));
      console.log(document.getElementById('logotipo'));


      const contentDataURL = canvas.toDataURL('image/png')
      const logo = canvas.toDataURL('image/png')
      /*
      TODO: This class is creating a logic error on Prod and generating extra chunk files blowing up
            plugins like the calendar. We must migrate PDF generation to a dedicated service, not here !
      */
      // const pdf = new jspdf.jsPDF('p', 'mm', 'a4'); // Página tamaño A4 de PDF
      // var position = 0;
      // pdf.addImage(contentDataURL, 'PNG', 3, 30, imgWidth, imgHeight)
      // // pdf.addImage(img, 'PNG', 15, 15, 15, 15)
      // pdf.save('ActividadDia.pdf'); // PDF generado
    })
  }

  // async generatePDF(){
  //   const pdf = new PdfMakeWrapper();
  //   D:\SpiderFleet\FrontEnd\src\app\components\shared\navs\analiticas\day-activity\day-activity.component.ts
  //   pdf.images({
  //     picture1: await new Img('https://1.bp.blogspot.com/-0kt6ekXtQrA/VQzbzppVR-I/AAAAAAACi6U/-8DXV8UxDKs/s1600/Atardecer%2Ben%2Bla%2Bplaya.jpg').build()
  //   })

  //   pdf.add(this.createTable(this.data));


  //   pdf.create().open();


  // }

  // }

  // createTable(data: any[]): ITable{
  //   [{}]
  //   return new Table([
  //     ['Vehiculos', 'Viajes'],
  //     ...this.extractData(data)
  //   ]).end
  // }



  // extractData(data: any): TableRow[] {
  //   return data.map(row => [row.VehicleName, row.ListItineraries.length]);
  // }


  ngOnInit(): void {
  }




}
