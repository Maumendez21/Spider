import { Component, OnInit } from '@angular/core';
import { SharedService } from 'src/app/Services/shared.service';

@Component({
  selector: 'app-options-map',
  templateUrl: './options-map.component.html',
  styleUrls: ['./options-map.component.css']
})
export class OptionsMapComponent implements OnInit {

  constructor(
    private shared: SharedService
  ) {
    this.getFilters();
  }


  public statusCluster: boolean = true;
  public statusMapColor: boolean = true;
  public statusSatelite: boolean = true;
  public statustrafficLayer: boolean = true;

  ngOnInit(): void {
  }

  getFilters() {

    this.shared.clusterDinamicoStream$.subscribe(data => {
      this.statusCluster = data;

    });

    this.shared.mapaDinamicoStream$.subscribe(data => {
      this.statusMapColor = data;
    });
    this.shared.mapaSateliteStream$.subscribe(data => {
      this.statusSatelite = data;
    });

    this.shared.trafficLayer$.subscribe(data => {
      this.statustrafficLayer = data;
    });

  }

  changeViewCluster(e){
    this.shared.clusterDinamicoStream(e);
  }

  changeMapColor(e){
    this.shared.mapaDinamicoStream(e);
  }

  changeSatelit(e){
    this.shared.mapaSateliteStream(e);
  }
}
