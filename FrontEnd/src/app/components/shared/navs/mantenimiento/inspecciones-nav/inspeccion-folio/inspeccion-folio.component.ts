import { Component, OnInit } from '@angular/core';
import { SpiderfleetService } from '../../../../../../services/spiderfleet.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../../../../services/shared.service';
import { ToastrService } from 'ngx-toastr';
import { data } from 'jquery';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-inspeccion-folio',
  templateUrl: './inspeccion-folio.component.html',
  styleUrls: ['./inspeccion-folio.component.css']
})
export class InspeccionFolioComponent implements OnInit {

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
      this.getMechanical();
      this.activatedRoue.params.subscribe(({folio}) => this.cargarInspecction(folio))
    } else {
      this.router.navigate(['/login']);
    }


  }

  public dataRecibe: any = [];

  cargarInspecction(folio: string){
    this.spiderService.getInspecctionFolio(folio).subscribe(data => {
      console.log(data[0]);
      this.dataRecibe = data[0];
      this.fechaInicio = this.dataRecibe.date;
      this.mechanic = this.dataRecibe.idmecanico;
      this.tipeInspect = this.dataRecibe.idType;
      this.vehiculo = this.dataRecibe.namevehicle;
      this.kilometraje = this.dataRecibe.mileage;
      this.responsable = this.dataRecibe.Responsable;
      this.plantilla = this.dataRecibe.results;
    })
  }



  getMechanical(){
    this.spiderService.getMechanics()
    .subscribe(mechanics =>{
      this.mechanics = mechanics;
    })
  }

  responsibleId: number;

  inspect(){


    const nuevoarr: any = []
    let arrEnd: any = []
    let arrPost: any = []

    this.plantilla.forEach(element => {
      nuevoarr.push(element.listinspectionresults)
    });

    for (let index = 0; index < nuevoarr.length; index++) {
      arrEnd = nuevoarr[index];
      for (let index = 0; index < arrEnd.length; index++) {
        arrPost.push({
          idResults: 1,
          Folio: arrEnd[index].folio,
          yes: arrEnd[index].yes,
          no: arrEnd[index].no,
          Good: arrEnd[index].Good,
          Regular: arrEnd[index].Regular,
          Bad: arrEnd[index].Bad,
          Notes: arrEnd[index].Notes,
          idTemplate: arrEnd[index].idtemplate
        })
      }
    }






    const data = {
      Folio: this.dataRecibe.folio,
      Date: this.dataRecibe.date,
      device: this.dataRecibe.device,
      mileage : this.kilometraje,
      results: arrPost
    }



    this.spiderService.putInspeccion(data)
    .subscribe(data =>{
      if (data['success']) {
        this.toastr.success('Inspección realizada', "Correcto!");
        Swal.fire({
          icon: 'success',
          title: 'Inspección Actualizada Correctamente.',
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

  }

  notes(indexPlantilla: number, indexTemplate: number, value: string){}

  generateData(idTemplate: string, value: string, enable: number, indexPlantilla: number, indexTemplate: number){

    this.plantillaTemp = this.plantilla;

    if (enable) {
      enable = 1;
    }else{
      enable = 0;
    }

    this.plantilla[indexPlantilla].listinspectionresults[indexTemplate][value] = enable;

    switch (value) {
      case "yes":
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["no"] = 0;
        break;
      case "no":
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["yes"] = 0;
        break;
      case "Good":
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["Regular"] = 0;
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["Bad"] = 0;
        break;
      case "Regular":
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["Good"] = 0;
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["Bad"] = 0;
        break;
      case "Bad":
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["Good"] = 0;
        this.plantilla[indexPlantilla].listinspectionresults[indexTemplate]["Regular"] = 0;
        break;
    }
  }


  ngOnInit(): void {
  }

}
