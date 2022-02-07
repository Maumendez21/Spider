import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-create-responsable-modal',
  templateUrl: './create-responsable-modal.component.html',
  styleUrls: ['./create-responsable-modal.component.css']
})
export class CreateResponsableModalComponent implements OnInit {

  @Input() reloadTable: any;

  name: string;
  email: string;
  phone: string = "";
  area: string = "";

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService) { }

  ngOnInit(): void {}

  createResponsable() {

    const data = {
      Name: this.name,
      Email: this.email,
      Phone: this.phone,
      Area: this.area
    };

    this.spiderService.setNuevoResponsable(data)
    .subscribe(response => {
      if (response['success']) {
        
        this.toastr.success("Exito al registrar el responsable", "Exito!");
        this.reloadTable();

        this.name = "";
        this.email = "";
        this.phone = "";
        this.area = "";

      } else {
        this.toastr.error(response['messages'], "Error!");
      }
    });
    

    
  }

}
