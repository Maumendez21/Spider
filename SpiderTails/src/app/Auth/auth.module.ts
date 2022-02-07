import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { RecoveryPasswordComponent } from './recovery-password/recovery-password.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    LoginComponent,
    RecoveryPasswordComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    RouterModule,
    // JwtModule.forRoot({
    //   config: {
    //     // ...
    //     tokenGetter: () => {
    //       return localStorage.getItem("token");
    //     },
    //   },
    // })
  ],
  // providers: [{ provide: JWT_OPTIONS, useValue: JWT_OPTIONS }, JwtHelperService]
})
export class AuthModule { }
