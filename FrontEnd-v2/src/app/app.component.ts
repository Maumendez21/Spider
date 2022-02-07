import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { SharedService } from './Services/shared.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'sdas';

  company: string;
  constructor(
    private titleService: Title,
    private shared: SharedService
  ){
    this.company = localStorage.getItem("company");
    this.compatyTitleBrowser();
  }

  public setTitle(newTitle: string) {
    this.titleService.setTitle(newTitle);
  }

  ngOnInit(): void {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    this.shared.broadcastPermisosStream('MAP100');
  }

  compatyTitleBrowser() {
    switch(this.company) {
      case "0":
       this.setTitle("Fleet Managment");
        break;
      case "1":
       this.setTitle("Kernel Logistics");
        break;
      case "2":
       this.setTitle("Agua y Drenaje de Nuevo León");
        break;
      default:
       this.setTitle("Fleet Managment");
        break;
    }
  }
}
