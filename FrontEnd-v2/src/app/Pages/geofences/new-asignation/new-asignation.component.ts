import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { SpiderService } from '../../../Services/spider.service';
import { GeofencesService } from '../services/geofences.service';

@Component({
  selector: 'app-new-asignation',
  templateUrl: './new-asignation.component.html',
  styleUrls: ['./new-asignation.component.css']
})
export class NewAsignationComponent implements OnInit {

  selectGeocerca: string = "";
  devicesChecked: any = [];
  geocercas: any;
  dispositivos: Array<any>;

  constructor(
    private spiderService: SpiderService,
    private geofencesService: GeofencesService,
    private router: Router
  ) { 
    this.getGeocercasAndDispositivos();
  }

  dropdownList = [];
  selectedItems = [];
  dropdownSettings = {};
  ngOnInit(){

    this.dropdownSettings = { 
      // singleSelection: false, 
      text:"Selecciona los dispositivos",
      selectAllText:'Select All',
      unSelectAllText:'UnSelect All',
      enableSearchFilter: true,
      classes:"myclass custom-class"
    };            
  }


  onItemSelect(item:any){
      
      
  }
  OnItemDeSelect(item:any){
      
      
  }
  onSelectAll(items: any){
      
  }
  onDeSelectAll(items: any){
      
  }
  getGeocercasAndDispositivos() {


    this.geofencesService.getListGeocercas()
      .subscribe(response => {
        this.geocercas = response;
      });

    this.spiderService.getDevicesGeneralNew("")
      .subscribe(response => {
        
        this.dispositivos = response["ListLastPosition"];

        
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

  list(){
    
  }
  
  crearAsignacion() {


    if (this.getSelectedOptions().length > 0 && this.selectGeocerca != "") {

      const data = {
        IdGeoFence: this.selectGeocerca,
        ListDevice: this.getSelectedOptions()
      }

      this.geofencesService.setNuevaAsignacion(data)
        .subscribe(response => {

          Swal.fire({
            title: 'Asignación correcta!',
            confirmButtonText: 'OK',
            icon: 'success'
          }).then((result) => {
            if (result.isConfirmed) {
              this.router.navigate(['/geofences/list']);
            }
          })
        });
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Es necesario seleccionar una geocerca y los dispositivos'
      })
    }
  }

  ngOnChanges(): void {

  }

}
