import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-ranking-tables',
  templateUrl: './ranking-tables.component.html',
  styleUrls: ['./ranking-tables.component.css']
})
export class RankingTablesComponent implements OnInit {

  constructor() { }

  @Input() rankingsBest: any[];
  @Input() rankingsLow: any[];

  ngOnInit(): void {
  }

}
