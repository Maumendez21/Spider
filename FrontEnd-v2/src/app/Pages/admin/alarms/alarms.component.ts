import { Component, OnInit } from '@angular/core';
import { ConfigurationService } from '../../configuration/services/configuration.service';

@Component({
  selector: 'app-alarms',
  templateUrl: './alarms.component.html',
  styleUrls: ['./alarms.component.css']
})
export class AlarmsComponent implements OnInit {

  alarmas: any[];
 

  constructor() {

   }

  ngOnInit(): void {
  }

 
}
