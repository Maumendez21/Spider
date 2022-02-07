import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PermitsService {

  constructor() { }

  getPermit(permit: string){
    return permit;
  }
}
