import { Injectable } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class RegisterService {
  private baseUrl = environment.apiUrl + '/auth';
  constructor(private http: HttpClient) { }

  createForm(): FormGroup {
    return new FormGroup({
      username: new FormControl(null, [Validators.required, Validators.minLength(3), Validators.maxLength(12)]),
      password: new FormControl(null, [Validators.required, Validators.minLength(6), Validators.maxLength(20)]),
      passwordMatch: new FormControl(null, [Validators.required, Validators.minLength(6), Validators.maxLength(20)]),
    });
  }

  post(form: FormGroup) {
    return this.http.post(this.baseUrl + '/register', form)
  }
}
