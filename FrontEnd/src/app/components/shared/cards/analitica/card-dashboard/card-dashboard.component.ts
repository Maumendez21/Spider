import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-card-dashboard',
  templateUrl: './card-dashboard.component.html',
  styleUrls: ['./card-dashboard.component.css']
})
export class CardDashboardComponent implements OnInit {
  @Input() titulo;
  @Input() info;
  @Input() class;
  @Input() icono;

  constructor() { }

  ngOnInit(): void {
  }

}
