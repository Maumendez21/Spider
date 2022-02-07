import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { MobilityService } from '../services/mobility.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-point-vinculation',
  templateUrl: './point-vinculation.component.html',
  styleUrls: ['./point-vinculation.component.css']
})
export class PointVinculationComponent implements OnInit {

  selectPunto: string = "";
  devicesChecked: any = [];
  puntos: any;
  dispositivos: any;


  constructor(
    private mobilityService: MobilityService,
    private router: Router
  ) { 
    this.getPuntosInteresAndDispositivos();
  }

  ngOnInit(): void {
    this.dropdownSettings = { 
      // singleSelection: false, 
      text:"Selecciona los dispositivos",
      selectAllText:'Select All',
      unSelectAllText:'UnSelect All',
      enableSearchFilter: true,
      classes:"myclass custom-class"
    }; 
  }

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};

  onItemSelect(item:any){
    console.log(item);
    console.log(this.selectedItems);
  }
  OnItemDeSelect(item:any){
      console.log(item);
      console.log(this.selectedItems);
  }
  onSelectAll(items: any){
      console.log(items);
  }
  onDeSelectAll(items: any){
      console.log(items);
  }



  getPuntosInteresAndDispositivos() {

    this.mobilityService.getListPuntosInteres()
      .subscribe(response => {

        this.puntos = response;
      });

    this.mobilityService.getDevicesGeneral("Flota", "")
      .subscribe(response => {
        this.dispositivos = response;

        this.dispositivos.forEach(element => {   
          this.dropdownList.push({
            id: element.dispositivo,
            itemName: element.nombre
          })
        });
      });
  }

  getSelectedOptions() {
    return this.selectedItems.map(opt => opt.id);
  }

  crearAsignacion() {

    

    if (this.getSelectedOptions().length > 0 && this.selectPunto != "") {

      const data = {
        IdPointInterest: this.selectPunto,
        ListDevice: this.getSelectedOptions()
      }

      
      console.log(data);
      


      this.mobilityService.setVincularPuntoDispositivos(data)
        .subscribe(response => {
          console.log(response);
          if (response['success']) {

            Swal.fire({
              position: 'top-end',
              title: 'Asignación correcta!',
              confirmButtonText: 'OK',
              icon: 'success'
            }).then((result) => {
              if (result.isConfirmed) {
                this.router.navigate(['/mobility/points-interest']);
              }
            })
          }else {

            Swal.fire('Error!', '' + response['messages'] , 'error', )
          }
        });
    } else {
      // this.toastr.warning('Es necesario seleccionar un punto de interes y los dispositivos', 'Faltan campos por seleccionar');
      Swal.fire('Error!', 'Es necesario seleccionar un punto de interes y los dispositivos', 'error', )
    }
  }



}
