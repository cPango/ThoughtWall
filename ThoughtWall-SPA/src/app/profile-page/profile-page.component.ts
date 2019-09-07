import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css']
})
export class ProfilePageComponent {
  helper = new JwtHelperService();
  decodedToken = {};
  register: any = {};
  login: any = {};
  errorMsg = [];

  private apiUrl = 'http://localhost:5000/api/auth';
  constructor(private http: HttpClient, private router: Router) {
    this.decodedToken = this.helper.decodeToken(localStorage.getItem('token'));
    console.log(this.decodedToken)
  }

  registerUser() {
    this.errorMsg = [];
    return this.http.post(this.apiUrl + '/register', this.register).subscribe(
      res => console.log('success'),
      fail => {
        console.log(fail);
      }
    );
  }

  loginUser() {
    this.errorMsg = [];
    return this.http.post(this.apiUrl + '/login', this.login).subscribe(
      res => {
        localStorage.setItem('token', res['token']),
        this.router.navigate(['/']);
      },
      fail => console.log(fail)
    );
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !!token;
  }

  logout() {
    localStorage.removeItem('token');
  }
}
