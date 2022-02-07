import { Component, OnInit } from '@angular/core';
import { SharedService } from '../../Services/shared.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private shared: SharedService) { }

  ngOnInit(): void {
    
  }

}
