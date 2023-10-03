import { Component, Injector } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators, SelectControlValueAccessor } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { ApprovalStatusDto, ChangePswdServiceProxy, CreateUserDto, RoleCheckboxDto, SelectListDto, SelectListServiceProxy, UserDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';

export const PasswordMatchValidator: ValidatorFn = (formGroup: FormGroup): ValidationErrors | null => {
    const parent = formGroup.parent as FormGroup;
    if (!parent) return null;
    return parent.get('passwordFormControl').value === parent.get('confirmPasswordFormControl').value ?
        null : { 'mismatch': true };
}

@Component({
  //selector: 'aadd-edit-customer',
  templateUrl: './add-edit-customer.component.html',
  styleUrls: ['./add-edit-customer.component.css']
})
export class AddEditCustomerComponent extends AppComponentBase {
    userId: number;
    isOnlyView: boolean = false;
    user: UserDto | null;
    userModes: SelectListDto[] | null;
    userDesignations: SelectListDto[] | null;
    userStatuses: SelectListDto[] | null;
    reportingManagers: SelectListDto[] | null;
    plantMasters: SelectListDto[] | null;
    hidePassword: boolean = true;
    hideConfirmPassword: boolean = true;
    isReadOnly: boolean = true;
    isProfileView: boolean = false;
    selectedPlants: number[] | null;
    isdescriptionValid: boolean = true;
    activeInactiveStatusOfUser: boolean = false;
    hideEmail: boolean = false;
    routeEncrypt: any;

    EMAIL_DUMMY: string = 'xxxx@domain.com';
    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _userService: UserServiceProxy,
        private _router: Router,
        private _route: ActivatedRoute,
        private _selectListService: SelectListServiceProxy,
        private formBuilder: FormBuilder
    ) {
        super(injector);
        //this.GetPlantMaster();
    }

    addEditFormGroup: FormGroup = this.formBuilder.group({
        userNameFormControl: ['', [Validators.required,Validators.pattern(new RegExp("^[a-zA-Z0-9_]*$")), Validators.maxLength(256), NoWhitespaceValidator]],
        emailFormControl: ['', [Validators.email, Validators.maxLength(256)]],
        firstNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
        lastNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
        phoneNumberFormControl: ['', [Validators.pattern(new RegExp("^\\d{10}|\d{11}|\d{12}|\d{13}$"))]],
        reportingManagerFormControl: [''],
        plantFormControl: [''],
        modesFormControl: ['', Validators.required],
        designationFormControl: [''],
        isActiveFormControl: [''],
        passwordMatchGroup: this.formBuilder.group({
            passwordFormControl: ['', [Validators.required, Validators.pattern(new RegExp("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[#?!@$%^&*-]).{8,}$")), Validators.maxLength(32), NoWhitespaceValidator]],
            confirmPasswordFormControl: ['', [Validators.required, PasswordMatchValidator, Validators.maxLength(32), NoWhitespaceValidator]]
        }),
        approvalStatusFormControl: [''],
    });

    matcher = new MyErrorStateMatcher();

    ngOnInit() {
        this.setTitle('Add Customer');
        let that = this;
        this.routeEncrypt = null;
        this.addEditFormGroup.get('passwordMatchGroup').get('passwordFormControl').valueChanges.subscribe(() => {
            this.addEditFormGroup.get('passwordMatchGroup').get('confirmPasswordFormControl').updateValueAndValidity();
        });

        this.UserRoles = new RoleCheckboxDto();

        this.user = new UserDto();
        this.user.isActive = true;
        this.GetRoles();
        this.GetModes();

        // this.GetUserDesignations();
        this.GetUserStatus();
        // this.GetReportingManager();
        // this.GetPlantMaster();

        this._route.params.subscribe((routeData: Params) => {
            if (routeData['userId']) {
                this.routeEncrypt = routeData['userId'];
                let paramId = this._changePwdService.decryptPassword(routeData['userId']);
                paramId.subscribe(id => {

                    that.userId = parseInt(id);
                    that.GetUser(that.userId);
                    this.setTitle('Edit User');
                    this.isProfileView = false;
                    this.addEditFormGroup.controls['approvalStatusFormControl'].disable();
                    });

            }
            if (routeData['profileId']) {
                this.routeEncrypt = routeData['profileId'];
                let paramId = this._changePwdService.decryptPassword(routeData['profileId']);
                paramId.subscribe(id => {

                    that.userId = parseInt(id);
                    this.isProfileView = true;
                    that.GetUser(that.userId);
                    that.markGroupDisabled(that.addEditFormGroup);
                    this.setTitle('Edit Profile');
                    this.addEditFormGroup.controls['approvalStatusFormControl'].disable();
                    this.addEditFormGroup.controls['firstNameFormControl'].enable();
                    this.addEditFormGroup.controls['lastNameFormControl'].enable();
                    this.addEditFormGroup.controls['phoneNumberFormControl'].enable();
                    });

            }
            if (routeData['action'] == 'view') {
                that.isOnlyView = true;
                this.setTitle('User Details');
                that.markGroupDisabled(that.addEditFormGroup);
            }
        })
    }

    UserRoles: RoleCheckboxDto;

    allSelected: boolean = false;

    markDirty() {
        this.markGroupDirty(this.addEditFormGroup);
        return true;
    }

    updateAllSelected() {
        this.allSelected = this.UserRoles != null && this.UserRoles.userRoles.every(t => t.isSelected);
    }

    someSelected(): boolean {
        if (this.UserRoles.userRoles == null) {
            return false;
        }
        return this.UserRoles.userRoles.filter(t => t.isSelected).length > 0 && !this.allSelected;
    }

    setAll(selected: boolean) {
        this.allSelected = selected;
        if (this.UserRoles.userRoles == null) {
            return;
        }
        this.UserRoles.userRoles.forEach(t => t.isSelected = selected);
    }

    GetRoles() {
        this._userService.getAllRoles().subscribe((roleData: RoleCheckboxDto) => {
            this.UserRoles = roleData;
        });
    }

    GetReportingManager() {
        this._selectListService.getReportingManagerUser().subscribe((modeSelectList: SelectListDto[]) => {
            this.reportingManagers = modeSelectList;
            if (this.userId) {
                this.reportingManagers = this.reportingManagers.filter(x => x.id != this.userId);
            }
        });
    }
    GetPlantMaster() {
        this._selectListService.getPlantsOnUser().subscribe((modeSelectList: SelectListDto[]) => {
            this.plantMasters = modeSelectList;
        });
    }
    GetPlantByUserId() {
        this._selectListService.getPlantByUserId(this.user.id).subscribe((modeSelectList: SelectListDto[]) => {
            this.selectedPlants = this.plantMasters.map(({ id }) => id);
        });
    }
    GetModes() {
        this._selectListService.getModes().subscribe((modeSelectList: SelectListDto[]) => {
            this.userModes = modeSelectList;
        });
    }

    GetUserDesignations() {
        this._selectListService.getDesignation().subscribe((modeSelectList: SelectListDto[]) => {
            this.userDesignations = modeSelectList;
        });
    }

    GetUserStatus() {
        this._selectListService.getApprovalStatus().subscribe((modeSelectList: SelectListDto[]) => {
            this.userStatuses = modeSelectList;
        });
    }

    GoToAddEditUser(userId: number) {
        if (this.isProfileView) {
            this._router.navigate(['../../../edit-profile', this.routeEncrypt], { relativeTo: this._route });
        } else {
            this._router.navigate(['../../../edit-user', this.routeEncrypt], { relativeTo: this._route });
        }
    }
    GoToViewUser(userId: number) {
        this._router.navigate(['../user', 'view', userId], { relativeTo: this._route });
    }
    GoToUsersListFromView() {
        if (this.isProfileView) {
            this._router.navigate(['../../..//home'], { relativeTo: this._route });
        } else {
            this._router.navigate(['../../../users'], { relativeTo: this._route });
        }
    }
    GoToUsersListFromEdit() {
        if (this.isProfileView) {
            this._router.navigate(['../../home'], { relativeTo: this._route });
        } else {
            this._router.navigate(['../../users'], { relativeTo: this._route });
        }

    }
    GoToUsersListFromAdd() {
        this._router.navigate(['../customer'], { relativeTo: this._route });
    }
    MakeNegativeSelectedToNull(user: UserDto) {
        if (user.designationId == -1) {
            user.designationId = null;
        }
        user.plants = null;
        if (user.reportingManagerId == -1) {
            user.reportingManagerId = null
        }
        if (user.modeId == -1) {
            user.modeId = null;
        }
        return user;
    }


    public randomizeEmail(firstName: string, lastName: string): string {
        var strValues = "abcdefg12345";
        var strEmail = "";
        var strTmp;
        for (var i = 0; i < 5; i++) {
            strTmp = strValues.charAt(Math.round(strValues.length * Math.random()));
            strEmail = strEmail + strTmp;
        }
        strTmp = "";
        strEmail = firstName + "." + lastName + "_" + strEmail + "@";

        strEmail = strEmail + "domain.com"
        return strEmail;

    }
    GetCreateUserDtoFromUserDto(user: UserDto) {
        let createUserDto: CreateUserDto = new CreateUserDto();
        createUserDto.firstName = user.firstName;
        createUserDto.lastName = user.lastName;
        createUserDto.phoneNumber = user.phoneNumber;
        createUserDto.designationId = user.designationId;
        createUserDto.modeId = user.modeId;
        createUserDto.plants = this.selectedPlants;
        createUserDto.reportingManagerId = user.reportingManagerId;
        createUserDto.email = user.email ? user.email : this.randomizeEmail(user.firstName, user.lastName);
        createUserDto.password = user.password;
        createUserDto.confirmPassword = user.confirmPassword;
        createUserDto.createdOn = moment(new Date());
        createUserDto.userName = user.userName;
        createUserDto.isActive = user.isActive;
        return createUserDto;
    }
    AddUser() {
        debugger;
        let userToAdd = this.user;
        let createUserDto: CreateUserDto = this.GetCreateUserDtoFromUserDto(userToAdd);
        if (this.UserRoles && this.UserRoles.userRoles) {
            createUserDto.roleNames = [];
            this.UserRoles.userRoles.filter(x => x.isSelected == true).forEach(role => {
                createUserDto.roleNames.push(role.name);
            });
        }
        abp.ui.setBusy();
        this._userService.create(createUserDto).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: UserDto) => {
            abp.notify.success('User added successfully.');
            this.GoToUsersListFromAdd();
        });
    }
    SaveUser() {
        this.user.roleNames = [];
        if (this.UserRoles && this.UserRoles.userRoles) {
            this.UserRoles.userRoles.filter(x => x.isSelected == true).forEach(role => {
                this.user.roleNames.push(role.name);
            });
        }
        let userToSave = this.user;
        if (!userToSave.email) {
            userToSave.email = this.randomizeEmail(userToSave.firstName, userToSave.lastName);
        }
        this.user.plants = this.selectedPlants;
        abp.ui.setBusy();
        let url = this.isProfileView ? this._userService.updateUserProfile(userToSave) : this._userService.update(userToSave);
        url.pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: UserDto) => {
            let msg = this.isProfileView ? 'User profile updated successfully.' : 'User updated successfully.';
            abp.notify.success(msg);
            this.GoToUsersListFromEdit();
        });
    }

    GetUser(userId: number) {
        let url = this.isProfileView ? this._userService.getUserProfile(userId) : this._userService.get(userId);
        abp.ui.setBusy();
        url.pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: UserDto) => {
            this.user = successData;
            this.user.activeInactiveStatusOfUser = this.user.isActive;
            if (this.user.email.indexOf("domain.com") > -1) {
                this.user.email = null;
            }
            this._selectListService.getPlantsOnUser().subscribe((modeSelectList: SelectListDto[]) => {
                this.plantMasters = modeSelectList;
            });

            this._selectListService.getPlantByUserId(this.user.id).subscribe((modeSelectList: SelectListDto[]) => {
                this.selectedPlants = modeSelectList.map(({ id }) => id);
            });
            this.user.password = "No@PASSWORD";
            this.user.confirmPassword = "No@PASSWORD";
            if (this.user.roleNames && this.user.roleNames.length > 0) {
                for (let x = 0; x < this.UserRoles.userRoles.length; x++) {
                    if (this.user.roleNames
                        .map((x) => x.toLowerCase().trim())
                        .includes(this.UserRoles.userRoles[x].name.toLowerCase().trim())) {
                        this.UserRoles.userRoles[x].isSelected = true;
                    }
                }
            }
            if (this.UserRoles.userRoles.every(role => role.isSelected == true)) {
                this.updateAllSelected();
            }
        });
    }

    ApproveUser() {
        this.isdescriptionValid = true;
        let approvalStatusEntity = new ApprovalStatusDto();
        approvalStatusEntity.id = this.userId;
        approvalStatusEntity.approvalStatusId = 2;
        approvalStatusEntity.description = this.user.approvalStatusDescription;
        abp.message.confirm(
            this.l('Do you want to approve?'),
            (isConfirmed: boolean) => {
                if (isConfirmed) {
                    this._userService.approveOrRejectUser(approvalStatusEntity).pipe(
                        finalize(() => {
                            abp.ui.clearBusy();
                        })
                    ).subscribe(result => {
                        abp.notify.success('User approved successfully.');
                        this.GoToUsersListFromView();
                    })
                }
            }
        );
    }

    RejectUser() {
        if (!this.user.approvalStatusDescription) {
            this.isdescriptionValid = false;
            return true;
        } else if (this.user.approvalStatusDescription) {
            if (this.user.approvalStatusDescription.trim().length == 0) {
                this.isdescriptionValid = false;
                return true;
            }
        }
        this.isdescriptionValid = true;
        let approvalStatusEntity = new ApprovalStatusDto();
        approvalStatusEntity.id = this.userId;
        approvalStatusEntity.approvalStatusId = 3;
        approvalStatusEntity.description = this.user.approvalStatusDescription;
        abp.message.confirm(
            this.l('Do you want to reject?'),
            (isConfirmed: boolean) => {
                if (isConfirmed) {
                    this._userService.approveOrRejectUser(approvalStatusEntity).pipe(
                        finalize(() => {
                            abp.ui.clearBusy();
                        })
                    ).subscribe(result => {
                        abp.notify.success('User rejected successfully.');
                        this.GoToUsersListFromView();
                    })
                }
            }
        );
    }
}