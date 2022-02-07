import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle-tab',
  templateUrl: './detalle-tab.component.html',
  styleUrls: ['./detalle-tab.component.css']
})
export class DetalleTabComponent implements OnInit {

  constructor() { }

  @Input() public infoDevice: any;

  ngOnInit(): void {
  }

}
