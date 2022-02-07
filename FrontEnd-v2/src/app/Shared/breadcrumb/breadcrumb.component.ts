import { Component, OnInit } from '@angular/core';
import { ActivationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { filter, map } from 'rxjs/operators';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css']
})
export class BreadcrumbComponent implements OnInit {

  constructor(
    private router: Router
  ) {
    this.titleSubs$ = this.getTitle()
    .subscribe( ({title}) => {
      this.title = title;
      // document.title = 'Administrador - ' + title;
    } );
  }

  title: string;
  titleSubs$: Subscription;

  ngOnInit(): void {
  }

  getTitle(){
    return this.router.events
    .pipe(
      filter(event => event instanceof ActivationEnd),
      filter((event: ActivationEnd) => event.snapshot.firstChild === null ),
      map((event: ActivationEnd) => event.snapshot.data ),
    );
  }

  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.titleSubs$.unsubscribe();

  }

}
