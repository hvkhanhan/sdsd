import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../shared/services/auth.service';

@Component({
  selector: 'app-login-admin',
  templateUrl: './login-admin.component.html',
  styleUrls: ['./login-admin.component.css']
})
export class LoginAdminComponent implements OnInit {

  formfroup: FormGroup;
  checked: boolean = false;

  constructor(
    private authenservice: AuthService,
    private toastr: ToastrService,
    private router: Router,
    private formbuilder: FormBuilder,

  ) { }

  ngOnInit() {
    this.formfroup = this.formbuilder.group({
      email: [null, [Validators.required]],
      password: [null, null],
      checked: [false, null],
    });
    this.checkLogin();
  }
  

  onSubmit() {
    //   this.authenservice.loginAdmin(this.formfroup.controls.email.value, this.formfroup.controls.password.value)
    //     .subscribe((data: any) => {
    //       this.router.navigate(['/admin']);
    //       this.toastr.success('Logged in successfully!','Notification');
    //     if(this.formfroup.controls.check.value == true){
    //       localStorage.setItem('email',JSON.stringify(this.formfroup.controls.email.value));
    //       localStorage.setItem('password',JSON.stringify(this.formfroup.controls.password.value));
    //     }
    //   },
    //   (error : HttpErrorResponse)=>{
    //     this.toastr.warning('Login unsuccessful!','Notification');
    //   });

    console.log(this.formfroup.controls.email.value, this.formfroup.controls.password.value);
    if (this.formfroup.controls.email.value === "hvkhanhan@gmail.com" && this.formfroup.controls.password.value === "meomeo123") {   
      //this.router.navigate(['/admin']); 
      this.toastr.success('Đăng nhập thành công!', 'Thông báo');
      if (this.formfroup.controls.checked.value === true) {
        localStorage.setItem('email', JSON.stringify(this.formfroup.controls.email.value));
        localStorage.setItem('password', JSON.stringify(this.formfroup.controls.password.value));

        localStorage.setItem('roles', 'Admin');
        this.router.navigate(['/admin']);
      }
    }
    else {
      this.toastr.warning('Đăng nhập thất bại!', 'Thông báo');
    }   
  }

  checkLogin() {
    if (localStorage.getItem('email') != null && localStorage.getItem('password') != null) {
      this.router.navigate(['/admin']);
    }
  }
}
