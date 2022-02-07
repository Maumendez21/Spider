import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from '../../environments/environment';




export class ServiceClass {

  constructor(private http: HttpClient){

  }

  public url = environment.API_URL;

  get token() {
    return localStorage.getItem("token")
  }

  getQuery(query:string) {
    const url = `${this.url}${ query }`;

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });

    return this.http.get(url, { headers });
  }

  postQuery(query:string, data:any = null) {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });

    const url = `${this.url}${ query }`;

    return this.http.post(url, data, { headers });
  }

  putQuery(query:string, data:any = null) {

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });
    const url = `${this.url}${ query }`;

    return this.http.put(url, data, { headers });
  }

  deleteQuery(query:string) {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem("token")}`
    });

    const url = `${this.url}${ query }`;

    return this.http.delete(url, { headers });
  }
}
