import { Component, Injector, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { ChangePasswordDto, UserDto, UserServiceProxy ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';


export const PasswordMatchValidator: ValidatorFn = (formGroup: FormGroup): ValidationErrors | null => {
  const parent = formGroup.parent as FormGroup;
  if (!parent) return null;
  return parent.get('passwordFormControl').value === parent.get('confirmPasswordFormControl').value ?
    null : { 'mismatch': true };
}
@Component({
  animations: [appModuleAnimation()],
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.less']
})
export class ResetPasswordComponent extends AppComponentBase
  implements OnInit {
  userId: number;
  user: UserDto | null;
  userRole: UserDto | null;
  changePswd: ChangePasswordDto | null;
  isOnlyView: boolean = false;
  hidePassword: boolean = true;
  hideConfirmPassword: boolean = true;
  isAdminRole:boolean = false;
  constructor(
    injector: Injector,
    private formBuilder: FormBuilder,
    private _userService: UserServiceProxy,
    private _changePasswordService: ChangePswdServiceProxy,
    private _route: ActivatedRoute,
    private _router: Router,
    private _authService: AppAuthService
  ) {
    super(injector);
  }

  resetPasswordFormGroup: FormGroup = this.formBuilder.group({
    firstNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
    lastNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
    empCodeFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
    passwordMatchGroup: this.formBuilder.group({
      passwordFormControl: ['', [Validators.required, Validators.pattern(new RegExp("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[#?!@$%^&*-]).{8,}$")), Validators.maxLength(32), NoWhitespaceValidator]],
      confirmPasswordFormControl: ['', [Validators.required, PasswordMatchValidator, Validators.maxLength(32), NoWhitespaceValidator]]
    }),
    approvalStatusFormControl: [''],
  });

  matcher = new MyErrorStateMatcher();
  ngOnInit(): void {
    this.setTitle('Reset Password');
    this.user = new UserDto();
    this.changePswd = new ChangePasswordDto();
    let that = this;
    //if(this.isOnlyView) {

       this.resetPasswordFormGroup.get('passwordMatchGroup').get('passwordFormControl').valueChanges.subscribe(() => {
        this.resetPasswordFormGroup.get('passwordMatchGroup').get('confirmPasswordFormControl').updateValueAndValidity();
    });

   // }
   

    this._route.params.subscribe((routeData: Params) => {
      this.userId = routeData['userId'];
      this.isOnlyView = routeData['action'] === 'view' ? true : false;
      that.GetUser(this.userId);
      this.GetLoggedInUser(this.appSession.user.id);
      this.setTitle('Reset Password');
      this.resetPasswordFormGroup.controls['firstNameFormControl'].disable();
      this.resetPasswordFormGroup.controls['lastNameFormControl'].disable();
      this.resetPasswordFormGroup.controls['empCodeFormControl'].disable();
     
     
    })

  }
  Back() {
    this.GetCurrentUser(this.appSession.user.id);


  }

  GetLoggedInUser(userId: number) {
    let url = this._changePasswordService.get(userId);
    abp.ui.setBusy();
    url.pipe(
      finalize(() => {
        abp.ui.clearBusy();
      })
    ).subscribe((successData: UserDto) => {
      this.userRole = successData;
      this.updateRole(this.userRole);
      

    });
  }

  GetCurrentUser(userId: number) {
    let url = this._changePasswordService.get(userId);
    abp.ui.setBusy();
    url.pipe(
      finalize(() => {
        abp.ui.clearBusy();
      })
    ).subscribe((successData: UserDto) => {
      this.userRole = successData;
      if (this.userRole.roleNames.find(x => x.includes('Admin'))) {
        if(this.isOnlyView) {
          this._router.navigate(['../../../password'], { relativeTo: this._route });
        } else {
          this._router.navigate(['../../password'], { relativeTo: this._route });
        }
        
      }
      else {
        if(this.isOnlyView) {
          this._router.navigate(['../../../home'], { relativeTo: this._route });
        } else {
          this._router.navigate(['../../home'], { relativeTo: this._route });
        }
        
      }

    });
  }

  private updateRole(userObj:any):void {
    if(userObj.roleNames.indexOf("SuperAdmin") > -1 || userObj.roleNames.indexOf("Admin") > -1) {
      this.isAdminRole = true;
    } else {
      this.isAdminRole = false;
    }
  }
  GetUser(userId: number) {
    let url = this._changePasswordService.get(userId);
    abp.ui.setBusy();
    url.pipe(
      finalize(() => {
        abp.ui.clearBusy();
      })
    ).subscribe((successData: UserDto) => {
      this.user = successData;
      this.user.password = "";
      this.user.confirmPassword = "";
      console.log(this.user);
     
      if((this.user.passwordStatus === 2 || this.user.passwordStatus === 3) && this.isOnlyView) {

          this.resetPasswordFormGroup.get('passwordMatchGroup').get('passwordFormControl').disable();
          this.resetPasswordFormGroup.get('passwordMatchGroup').get('confirmPasswordFormControl').disable();

      }
    });
  }

  Clear() {
    this.changePswd.newPassword = "";
    this.changePswd.confirmPassword = "";
  }

  Submit(): void {

    abp.ui.setBusy();
    this.changePswd.currentUser = this.appSession.user.id;
    this.changePswd.userId = this.userId
    this.changePswd.userName = this.user.userName
    this.changePswd.passwordStatus = this.user.passwordStatus
    this._changePasswordService.changePassword

      (this.changePswd)
      .pipe(
        finalize(() => {
          abp.ui.clearBusy();
        })
      )
      .subscribe((successData: ChangePasswordDto) => {
        if (successData.status == true)
          abp.notify.success('Reset password successfully.');
        if (this.changePswd.currentUser == this.changePswd.userId) {
          this.logout();
          this._router.navigate(['account/login']);
        }
        else {
          if(this.isOnlyView) {
            this._router.navigate(['../../../password'], { relativeTo: this._route });
          } else {
            this._router.navigate(['../../password'], { relativeTo: this._route });
          }
          
        }

      });
  }
  logout(): void {
    abp.utils.deleteCookie("All");
    localStorage.setItem('plantId', '');
    this._authService.logout();
  }
  markDirty() {
    this.markGroupDirty(this.resetPasswordFormGroup);
    return true;
  }
}
