import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-details-travel',
  templateUrl: './details-travel.component.html',
  styleUrls: ['./details-travel.component.css']
})
export class DetailsTravelComponent implements OnInit {

  constructor() { }

  @Input() nombre: string;
  @Input() responsible: string;
  @Input() fuel: string;
  @Input() time: string;
  @Input() distance: string;

  @Input() aceleration: string;
  @Input() braking: string;
  @Input() speed: string;

  @Input() startDate : string;
  @Input() endDate: string;

  @Input() startingPoint: string;
  @Input() finalPoint: string;

  ngOnInit(): void {
  }

}
