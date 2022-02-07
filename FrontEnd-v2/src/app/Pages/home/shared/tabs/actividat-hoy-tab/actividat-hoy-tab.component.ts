import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-actividat-hoy-tab',
  templateUrl: './actividat-hoy-tab.component.html',
  styleUrls: ['./actividat-hoy-tab.component.css']
})
export class ActividatHoyTabComponent implements OnInit {

  @Input() public tipo: number;
  @Input() public infoDevice: any;
  @Input() public logotipo: any;
  @Input() getEstatusName: any;
  @Input() getTipoDispositivo: any;

  constructor() {
  }

  ngOnInit(): void {
  }

}
