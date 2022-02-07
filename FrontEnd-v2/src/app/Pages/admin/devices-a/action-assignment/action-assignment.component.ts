import { Component, OnInit, Input } from '@angular/core';
import { ConfigurationService } from 'src/app/Pages/configuration/services/configuration.service';
import { AdminService } from '../../services/admin.service';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-action-assignment',
  templateUrl: './action-assignment.component.html',
  styleUrls: ['./action-assignment.component.css']
})
export class ActionAssignmentComponent implements OnInit {

    // @Input() reloadTable: any;
    @Input() idSend: string;
    @Input() name: string;
    @Input() node: string;

    @Input() refreshDataTableAsignacion: any
  
  
    public title: string;
    public companies: any = [];

    public subempresa: string;
    public subempresas: any;

    constructor(
      private configurationService: ConfigurationService, 
      private adminService: AdminService
    ) { 
        this.getCompanies();
        //this.getSubempresas();
    }
  
    ngOnInit(): void {
    }

    cleanValues(){
        
      
    }




    getCompanies() {
        this.adminService.getCompanies()
          .subscribe(data => {
            this.companies = data;
          });
    }

    getSubempresas() {
        this.configurationService.getListSubempresas()
          .subscribe(response => {
            this.subempresas = response;
          });
    }
}