import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TraineeService } from 'src/app/shared/services/trainee.service';

@Component({
  selector: 'app-reset-trainee',
  templateUrl: './reset-trainee.component.html',
  styleUrls: ['./reset-trainee.component.css']
})
export class ResetTraineeComponent implements OnInit {

  isLoginError: boolean = false;

  constructor(
    protected service: TraineeService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null) {
      form.resetForm();
    }
    this.service.formData = {
      traineeid: null,
      name: '',
      email: '',
      phone: '',
      address: '',
      isActive: false,
      password: '',
      activationcode: '',
      resetpasswordcode: ''
    };
  }

  onSubmit(form: NgForm) {
    this.service.resetpasswordtrainee(form.value).subscribe((data: any) => {
      this.router.navigate(['/sign-in']);
      this.toastr.success('Khôi phục mật khẩu thành công!', 'Thông báo');
    },
      (error: HttpErrorResponse) => {
        this.isLoginError = true;
        this.toastr.warning('Khôi phục mật khẩu không thành công!', 'Thông báo');
      });
  }

}
