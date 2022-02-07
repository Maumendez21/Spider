import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {

  @Input() color: string;
  @Input() info: string;
  @Input() icon: string;
  @Input() footer: string;

  constructor() { }

  ngOnInit(): void {
  }

}
