import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-detalle2-tab',
  templateUrl: './detalle2-tab.component.html',
  styleUrls: ['./detalle2-tab.component.css']
})
export class Detalle2TabComponent implements OnInit {

  constructor() { }
  @Input() infoDevice2: any = {};

  ngOnInit(): void {
  }

}
