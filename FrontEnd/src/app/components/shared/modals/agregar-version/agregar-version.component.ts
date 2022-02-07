import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from 'src/app/services/shared.service';
import { SpiderfleetService } from 'src/app/services/spiderfleet.service';

@Component({
  selector: 'app-agregar-version',
  templateUrl: './agregar-version.component.html',
  styleUrls: ['./agregar-version.component.css']
})
export class AgregarVersionComponent implements OnInit {

  version: string;

  constructor(private spiderService: SpiderfleetService, private shared: SharedService, private router: Router, private toastr: ToastrService) {
    if (this.shared.verifyLoggin()) {

    } else {
      this.router.navigate(['/login']);
    }
  }

  ngOnInit(): void {
  }

  agregarVersion(){

    if (this.version !== null) {

      const data = {
        Description: this.version
      };

      this.spiderService.setNewVersion(data)
      .subscribe(response => {
        if (response['success']) {
          this.toastr.success("Exito al agregar la nueva versión", "Exito!");
        }else{
          this.toastr.error(response['messages'] , "Error!");
        }

      });


    } else {
      this.toastr.warning("Es necesario llenar todos los campos", "Campos vacios!");
    }

  }

}
