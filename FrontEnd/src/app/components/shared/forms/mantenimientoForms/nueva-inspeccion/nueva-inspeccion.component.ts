import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SpiderfleetService } from '../../../../../services/spiderfleet.service';
import { SharedService } from '../../../../../services/shared.service';
import { ToastrService } from 'ngx-toastr';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-nueva-inspeccion',
  templateUrl: './nueva-inspeccion.component.html',
  styleUrls: ['./nueva-inspeccion.component.css']
})
export class NuevaInspeccionComponent implements OnInit {

  public fechaInicio: string = "";
  public mechanic: string = "";
  public compani: string = "";
  public kilometraje: string = "";
  public responsable: string;
  public vehiculo: string = "";
  public tipeInspect: number = 0;

  public si: number = 0;
  public no: number = 0;
  public bueno: number = 0;
  public regular: number = 0;
  public malo: number = 0;
  public observaciones="";

  public mechanics: any = [];
  public subempresas: any = [];
  public spiderMarkers: any = [];
  public plantilla: any = [];
  public plantillaTemp: any = [];

  constructor(private spiderService: SpiderfleetService,
              private activatedRoue: ActivatedRoute,
              private router: Router,
              private shared: SharedService,
              private toastr: ToastrService) {
    if (shared.verifyLoggin()) {
      this.getDevices("");
      this.getMechanical();
      this.getSubempresas();
      this.getPlantilla();
      this.getMechanical();
      this.getSubempresas();
    } else {
      this.router.navigate(['/login']);
    }


  }

  ngOnInit(): void {
  }

  getPlantilla(){

    this.spiderService.getPlantilla()
    .subscribe(plantilla =>{
      this.plantilla = plantilla;
    })

  }



  getMechanical(){
    this.spiderService.getMechanics()
    .subscribe(mechanics =>{
      this.mechanics = mechanics;
    })
  }

  getSubempresas() {
    this.spiderService.getSubempresas()
    .subscribe(data => {

      this.subempresas = data;
    });
  }


  getDevices(compani: string) {
    this.spiderService.getDevicesAdmin(compani)
    .subscribe(data => {

      this.spiderMarkers = data;
    }, error => {
      this.shared.broadcastLoggedStream(false);
      this.shared.clearSharedSession();
      this.router.navigate(['/login']);
    });
  }

  responsibleId: number;

  getResponsible(device: string){
    this.spiderService.getResponsableVehicle(device)
    .subscribe(data => {
      if (data.Id !== 0) {
        this.responsable = data.Name;
        this.responsibleId = data.Id;

       }else {
         this.responsable = "No hay responsable";
         this.responsibleId = 0;
       }
    })
  }

  inspect(){
    const nuevoarr: any = []
    let arrEnd: any = []
    let arrPost: any = []

    this.plantillaTemp.forEach(element => {
      nuevoarr.push(element.Templates)
    });

    for (let index = 0; index < nuevoarr.length; index++) {
      arrEnd = nuevoarr[index];
      for (let index = 0; index < arrEnd.length; index++) {
        arrPost.push({
          Folio: "",
          yes: arrEnd[index].yes,
          no: arrEnd[index].no,
          Good: arrEnd[index].Good,
          Regular: arrEnd[index].Regular,
          Bad: arrEnd[index].Bad,
          Notes: arrEnd[index].Notes,
          idTemplate: arrEnd[index].idTemplate
        })

      }
    }

    let date = new Date();
    const data = {
      listinspectionresult: arrPost,
      node: "",
      Folio: "",
      Date: date.getDate(),
      idMechanic: this.mechanic,
      idType: this.tipeInspect,
      device: this.vehiculo,
      mileage: this.kilometraje + "km",
      idResponsible: this.responsibleId
    }

    if ( this.mechanic != "" && this.tipeInspect !== 0 && this.vehiculo !== "" && this.kilometraje !== "") {
      this.spiderService.postInspection(data)
      .subscribe(data =>{
        if (data['success']) {
          this.toastr.success('Inspección realizada', "Correcto!");
          Swal.fire({
            icon: 'success',
            title: 'Inspección realizada.',
            text: 'Folio de la Inspección: ' + data['folio'],
            confirmButtonColor: '#ff6e40',
            confirmButtonText: 'Confirmar',
            allowOutsideClick: false
          }).then((result) => {
            if (result.value) {
              this.router.navigate(['/mantenimiento/inspecciones']);
            }
          })
        }else{
          this.toastr.error(data['messages'] + "", "Error!");
        }

      })




    }else{
      this.toastr.error('No dejes campos vacios.', "Error!");
    }

  }

  notes(indexPlantilla: number, indexTemplate: number, value: string){}

  generateData(idTemplate: string, value: string, enable: number, indexPlantilla: number, indexTemplate: number){

    this.plantillaTemp = this.plantilla;

    if (enable) {
      enable = 1;
    }else{
      enable = 0;
    }

    this.plantillaTemp[indexPlantilla].Templates[indexTemplate][value] = enable;

    switch (value) {
      case "yes":
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["no"] = 0;
        break;
      case "no":
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["yes"] = 0;
        break;
      case "Good":
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["Regular"] = 0;
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["Bad"] = 0;
        break;
      case "Regular":
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["Good"] = 0;
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["Bad"] = 0;
        break;
      case "Bad":
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["Good"] = 0;
        this.plantillaTemp[indexPlantilla].Templates[indexTemplate]["Regular"] = 0;
        break;
    }
  }





}
