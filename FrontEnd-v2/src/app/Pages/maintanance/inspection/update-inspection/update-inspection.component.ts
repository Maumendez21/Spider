import { Component, OnInit } from '@angular/core';
import { MaitananceService } from '../../services/maitanance.service';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-update-inspection',
  templateUrl: './update-inspection.component.html',
  styleUrls: ['./update-inspection.component.css']
})
export class UpdateInspectionComponent implements OnInit {

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

  constructor(
    private maitanaceService: MaitananceService,
    private activatedRoue: ActivatedRoute,
    private router: Router

  ) { 
    this.getMechanical();
    this.activatedRoue.params.subscribe(({id}) => this.cargarInspecction(id))
  }

  getMechanical(){
    this.maitanaceService.getMechanics()
    .subscribe(mechanics =>{
      this.mechanics = mechanics;
    })
  }

  public dataRecibe: any = [];
  responsibleId: number;

  cargarInspecction(folio: string){
    this.maitanaceService.getInspecctionFolio(folio).subscribe(data => {
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



  ngOnInit(): void {
  }

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
      mileage : this.kilometraje.slice(0, -2),
      results: arrPost
    }



    this.maitanaceService.putInspeccion(data)
    .subscribe(data =>{
      if (data['success']) {
        Swal.fire({
          icon: 'success',
          title: 'Inspección Actualizada Correctamente.',
          confirmButtonText: 'Confirmar',
          allowOutsideClick: false
        }).then((result) => {
          if (result.value) {
            this.router.navigate(['/maitanance/inspection']);
          }
        })
      }else{
        Swal.fire('Error!', '' + data['messages'], 'error')
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

}
