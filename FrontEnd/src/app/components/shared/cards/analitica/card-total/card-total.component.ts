import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card-total',
  templateUrl: './card-total.component.html',
  styleUrls: ['./card-total.component.css']
})
export class CardTotalComponent implements OnInit {

  @Input() titulo: string;
  @Input() total: string;
  @Input() classCard: string;

  constructor() { }

  ngOnInit(): void {
  }

}
