import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TraineeService } from '../shared/services/trainee.service';
import { MentorService } from '../shared/services/mentor.service';
import { Trainee } from '../shared/models/trainee.model';
import { Mentor } from '../shared/models/mentor.model';

export interface type {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent implements OnInit {

  trainee: Trainee;
  mentor: Mentor;
  isLoginError = false;
  selectedValue: string;

  types: type[] = [
    { value: 'trainees', viewValue: 'Học viên' },
    { value: 'mentors', viewValue: 'Giảng viên' }
  ];

  constructor(
    private service: TraineeService,
    private services: MentorService,
    private toastr: ToastrService,
    private router: Router,
  ) { }

  ngOnInit() {
  }


  onSubmit(form: NgForm) {
    if (this.selectedValue === 'trainees') {
      this.service.forgettrainee(form.value).subscribe((data: any) => {
        this.router.navigate(['/reset-trainee']);
        this.toastr.success(
          'Mã xác nhận thay đổi mật khẩu đã được gửi đến email của bạn, vui lòng xác nhận email!',
          'Thông báo');
      },
        (error: HttpErrorResponse) => {
          this.isLoginError = true;
          this.toastr.warning('Email không tồn tại!', 'Thông báo');
        });
    } else {
      this.services.forgetmetor(form.value).subscribe((data: any) => {
        this.router.navigate(['/reset-mentor']);
        this.toastr.success(
          'Mã xác nhận thay đổi mật khẩu đã được gửi đến email của bạn, vui lòng xác nhận email!', 'Thông báo');
      },
        (error: HttpErrorResponse) => {
          this.isLoginError = true;
          this.toastr.warning('Email không tồn tại!', 'Thông báo');
        });
    }


  }

}
