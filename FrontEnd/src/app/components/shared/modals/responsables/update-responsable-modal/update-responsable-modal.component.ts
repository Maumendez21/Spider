import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-update-responsable-modal',
  templateUrl: './update-responsable-modal.component.html',
  styleUrls: ['./update-responsable-modal.component.css']
})
export class UpdateResponsableModalComponent implements OnInit {

  @Input() idSend: number;
  @Input() reloadTable: any;

  id: string;
  nameUpdate: string;
  emailUpdate: string;
  phoneUpdate: string;
  areaUpdate: string;

  constructor(private spiderService: SpiderfleetService, private toastr: ToastrService) { }

  ngOnInit(): void {}

  ngOnChanges(): void {
    this.getResponsable(this.idSend);
  }

  getResponsable(id: number) {

    this.spiderService.getResponsable(id)
      .subscribe(response => {
        this.id = response.Id;
        this.nameUpdate = response.Name;
        this.emailUpdate = response.Email;
        this.areaUpdate = response.Area;
        this.phoneUpdate = response.Phone;
      });
  }

  updateResponsable() {

    const data = {
      Id: this.id,
      Name: this.nameUpdate,
      Email: this.emailUpdate,
      Phone: this.phoneUpdate,
      Area: this.areaUpdate
    };

    this.spiderService.setUpdateResponsable(data)
      .subscribe(response => {

        if (response['success']) {
          this.reloadTable();
          this.toastr.success("Exito al actualizar el responsable", "Exito!");
        } else {
          this.toastr.error(response['messages'] , "Error!");
        }
      });

  }

}
