import { Component, Injector, Renderer2, ViewChild, ElementRef } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ErrorStateMatcher, mixinDisabled, ThemePalette } from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RxwebValidators } from '@rxweb/reactive-form-validators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { ApprovalStatusDto, CreatePlantDto, CreateUserDto, PlantDto, PlantServiceProxy, RoleCheckboxDto, SelectListDto, SelectListServiceProxy, UserDto, UserServiceProxy, ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
// import { TopBarComponent } from '@app/layout/topbar.component';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';
declare var jquery: any;
declare var $: any;
@Component({
    selector: 'app-add-edit-forms',
    templateUrl: './add-edit-forms.component.html',
    styleUrls: ['./add-edit-forms.component.css'],
    animations: [appModuleAnimation()],
})
export class AddEditFormsComponent extends AppComponentBase {
    @ViewChild('div', {static: false}) div: ElementRef;
    plantId: number;
        fieldhtml:any = [{
            "single_input_text": "<div class='form-group ui-state-default' id='itemtext'><label for='Inputtext' class=''><p class='lbtx'>Name</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='text' class='form-control' id='Inputtext' aria-describedby='' placeholder='Write here...'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
            "input_email": "<div class='form-group ui-state-default' id='itemEmail '><label for='InputEmail' class=''><p class='lbtx'>Email address</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='email' class='form-control' id='InputEmail' aria-describedby='emailHelp' placeholder='Write here...'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
            "input_phone": "<div class='form-group ui-state-default' id='itemphone'><label for='Inputtel' class=''><p class='lbtx'>Phone</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='tel' class='form-control' id='Inputtel' aria-describedby='emailHelp' placeholder='Write here...'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
            "input_number": "<div class='form-group ui-state-default' id='itemnuber'><label for='Inputnumber' class=''><p class='lbtx'>Number</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='number' class='form-control' id='Inputnumber' aria-describedby='emailHelp' placeholder='Write here...'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
            "input_password": "<div class='form-group ui-state-default' id='itempassword'><label for='Inputpassword' class=''><p class='lbtx'>Password</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='number' class='form-control' id='Inputpassword' aria-describedby='emailHelp' placeholder='Write here...'><small id='errow' class='fommsg-errow'>We'll never share your email with anyone else.</small><small id='succes' class='fommsg-succes'>We'll never share your email with anyone else.</small><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>",
            "multiple_input_text": "",
            "checkbox": "<div class='form-group ui-state-default' id='itemcheckbox'><label for='checkbox' class=''><p class='lbtx'>checkbox</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='checkbox' class='form-control' id='checkbox'><div class='editdelete trans'><span class='edit trans'><i class='fa fa-pencil' aria-hidden='true'></i></span><span class='delete trans'><i class='fa fa-trash-o' aria-hidden='true'></i></span></div></div>"
        }];
        htmlcode: SafeHtml;
    isdescriptionValid: boolean = true;
    isOnlyView: boolean = false;
    plant: PlantDto | null;
    plantMasters: SelectListDto[] | null;
    countries: SelectListDto[] | null;
    states: SelectListDto[] | null;
    constructor(
        injector: Injector,
        private renderer: Renderer2,
        private sanitized: DomSanitizer,
        private _router: Router,
        private _route: ActivatedRoute,
        private _plantService: PlantServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private formBuilder: FormBuilder,
        // private TopBarComponent: TopBarComponent,
        private _changePwdService: ChangePswdServiceProxy,

    ) {
        super(injector);
    }
    matcher = new MyErrorStateMatcher();

    ngOnInit() {
       

        this.setTitle('Add Plant');
        let that = this;

        this.plant = new PlantDto();
        this.plant.isActive = true;
        this.GetPlantMaster();
        this.GetCountries();
        this._route.params.subscribe((routeData: Params) => {
            if (routeData['plantId']) {
                let plantId = this._changePwdService.decryptPassword(routeData['plantId']);
                plantId.subscribe(id => {

                    that.plantId = parseInt(id);
                    that.GetPlant(that.plantId);
                    this.setTitle('Edit Plant');
                    this.addEditFormGroup.controls['approvalStatusFormControl'].disable();

                })

            }
            if (routeData['action'] == 'view') {
                that.isOnlyView = true;
                this.setTitle('Plant Details');
                that.markGroupDisabled(that.addEditFormGroup);
            }
        });

        this.addEditFormGroup.get('countryFormControl').valueChanges.subscribe(item => {
            if (item) {
                this.GetStates(item);
            } else {
                this.states = [];
                this.plant.stateId = null;
            }


        });

        this.addEditFormGroup.get('plantTypeFormControl').valueChanges.subscribe(item => {

            if (item == 2) {

                this.addEditFormGroup.get('subPlantFormControl').setValidators([Validators.required]);
                this.addEditFormGroup.get('subPlantFormControl').updateValueAndValidity();
            }
            else {
                this.plant.masterPlantId = null;
                this.addEditFormGroup.get('subPlantFormControl').clearValidators();
                this.addEditFormGroup.get('subPlantFormControl').updateValueAndValidity();
            }


        });
        $(document).ready(function () {
            $('.addElement').click(function(){
                $(".siteWrap").addClass("addEl-open")
            })
            $('.closeElement').click(function(){
                $(".siteWrap").removeClass("addEl-open")
            })
             // append form
    //   $('#field-item-text').click(function(){
    //     $("#creator").append(" <div class='form-group comm-form-group ui-state-default'><label for='exampleInputEmail1' class='labels'>  <p class='lbtx'>Email address</p><span class='lbrq'>*</span><span class='lbtt'></span></label><input type='password' class='form-control' id='exampleInputEmail1' aria-describedby='emailHelp'><small id='emailHelp' class='form-text text-muted'>We'll never share your email with anyone else.</small></div>");
    //   })
      

        })
    }
    addEditFormGroup: FormGroup = this.formBuilder.group({
        plantIdFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
        plantNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
        plantTypeFormControl: ['', [Validators.required]],
        emailFormControl: ['', [Validators.email, Validators.maxLength(256)]],
        phoneNumberFormControl: ['', [Validators.pattern(new RegExp("^\\d{10}|\d{11}|\d{12}|\d{13}$"))]],
        subPlantFormControl: [''],
        isActiveFormControl: [''],
        descriptionFormControl: [''],
        taxRegNoFormControl: [''],
        licenseFormControl: [''],
        gs1PrefixFormControl: [''],
        websiteFormControl: ['', RxwebValidators.url()],
        postalCodeFormControl: [''],
        stateFormControl: [''],
        countryFormControl: [''],
        cityFormControl: [''],
        address1FormControl: [''],
        address2FormControl: [''],
        approvalStatusFormControl: [''],

    });


    markDirty() {
        this.markGroupDirty(this.addEditFormGroup);
        return true;
    }

    GetPlantMaster() {
        this._selectListService.getAllMasterPlants().subscribe((plantSelectList: SelectListDto[]) => {
            this.plantMasters = plantSelectList;
            if (this.plantId) {
                this.plantMasters = this.plantMasters.filter(x => x.id != this.plantId);
            }

        });
    }

    GetCountries() {
        this._selectListService.getCountries().subscribe((countrySelectList: SelectListDto[]) => {
            this.countries = countrySelectList;
        });
    }

    GetStates(countryId: number) {
        this._selectListService.getStates(countryId).subscribe((stateSelectList: SelectListDto[]) => {
            this.states = stateSelectList;
        });
    }

    GoToAddEditPlant(plantId: any) {
        this._changePwdService.encryptPassword(plantId).subscribe(
            data => {

                this._router.navigate(['../../../edit-plant', data], { relativeTo: this._route });

            }
        );
        // this._router.navigate(['../../../edit-plant', plantId], { relativeTo: this._route });
    }
    GoToViewPlant(plantId: number) {
        this._router.navigate(['../plant', 'view', plantId], { relativeTo: this._route });
    }
    GoToPlantsListFromView() {
        this._router.navigate(['../../../plants'], { relativeTo: this._route });
    }
    GoToPlantsListFromEdit() {
        this._router.navigate(['../../plants'], { relativeTo: this._route });
    }
    GoToPlantsListFromAdd() {
        this._router.navigate(['../plants'], { relativeTo: this._route });
    }

    GetCreatePlantDtoFromPlantDto(plant: PlantDto) {
        let createPlantDto: CreatePlantDto = new CreatePlantDto();
        createPlantDto.countryId = plant.countryId;
        createPlantDto.stateId = plant.stateId;
        createPlantDto.address1 = plant.address1;
        createPlantDto.address2 = plant.address2;
        createPlantDto.city = plant.city;
        createPlantDto.description = plant.description;
        createPlantDto.email = plant.email;
        createPlantDto.gS1Prefix = plant.gS1Prefix;
        createPlantDto.license = plant.license;
        createPlantDto.phoneNumber = plant.phoneNumber;
        createPlantDto.plantId = plant.plantId;
        createPlantDto.plantName = plant.plantName;
        createPlantDto.postalCode = plant.postalCode;
        createPlantDto.masterPlantId = plant.masterPlantId;
        createPlantDto.taxRegistrationNo = plant.taxRegistrationNo;
        createPlantDto.website = plant.website;
        createPlantDto.isActive = plant.isActive;
        createPlantDto.plantTypeId = plant.plantTypeId;

        return createPlantDto;
    }
    AddPlant() {
        let plantToAdd = this.plant;
        let createPlantDto: CreatePlantDto = this.GetCreatePlantDtoFromPlantDto(plantToAdd);

        abp.ui.setBusy();
        this._plantService.create(createPlantDto).pipe(
            finalize(() => {

                abp.ui.clearBusy();
            })
        ).subscribe((successData: PlantDto) => {

            abp.notify.success('Plant added successfully.');
            this.GoToPlantsListFromAdd();
        });

    }
    SavePlant() {


        let plantToSave = this.plant;
        abp.ui.setBusy();
        this._plantService.update(plantToSave).pipe(
            finalize(() => {

                abp.ui.clearBusy();
            })
        ).subscribe((successData: PlantDto) => {
            abp.notify.success('Plant updated successfully.');
            this.GoToPlantsListFromEdit();
        });
    }

    GetPlant(plantId: number) {
        abp.ui.setBusy();
        this._plantService.get(plantId).pipe(
            finalize(() => {
                abp.ui.clearBusy();
            })
        ).subscribe((successData: PlantDto) => {
            this.plant = successData;




        });


    }
    ApprovePlant() {
        this.isdescriptionValid = true;
        let approvalStatusEntity = new ApprovalStatusDto();
        approvalStatusEntity.id = this.plantId;
        approvalStatusEntity.approvalStatusId = 2;
        approvalStatusEntity.description = this.plant.approvalStatusDescription;
        abp.message.confirm(
            this.l('Do you want to approve?'),
            (isConfirmed: boolean) => {
                if (isConfirmed) {
                    this._plantService.approveOrRejectPlant(approvalStatusEntity).pipe(
                        finalize(() => {
                            abp.ui.clearBusy();
                        })
                    ).subscribe(result => {

                        abp.notify.success('Plant approved successfully.');
                        this.GoToPlantsListFromView();
                    })
                }
            }
        );
    }

    RejectPlant() {
        if (!this.plant.approvalStatusDescription) {
            this.isdescriptionValid = false;
            return true;
        } else if (this.plant.approvalStatusDescription) {
            if (this.plant.approvalStatusDescription.trim().length == 0) {
                this.isdescriptionValid = false;
                return true;
            }
        }
        this.isdescriptionValid = true;
        let approvalStatusEntity = new ApprovalStatusDto();
        approvalStatusEntity.id = this.plantId;
        approvalStatusEntity.approvalStatusId = 3;
        approvalStatusEntity.description = this.plant.approvalStatusDescription;
        abp.message.confirm(
            this.l('Do you want to reject?'),
            (isConfirmed: boolean) => {
                if (isConfirmed) {
                    this._plantService.approveOrRejectPlant(approvalStatusEntity).pipe(
                        finalize(() => {
                            abp.ui.clearBusy();
                        })
                    ).subscribe(result => {

                        abp.notify.success('Plant rejected successfully.');
                        this.GoToPlantsListFromView();
                    })
                }
            }
        );
    }

    //
    showFields(event) {
// for(let i = 0; i <= this.fieldhtml.length; i++){
//     console.log(this.fieldhtml)

//   }
// var k = JSON.stringify(this.fieldhtml);
// var l = JSON.parse(k);
// console.log(l)
// console.log(k)
  const element: HTMLDivElement = this.renderer.createElement('div');
  for (let value of Object.values(this.fieldhtml)) {
    console.log(value[event]);
   // this.htmlcode = this.sanitized.bypassSecurityTrustHtml(value[event]);
    //this.renderer.appendChild(this.div.nativeElement, this.htmlcode)
   element.innerHTML = value[event];
   }
   this.renderer.appendChild(this.div.nativeElement, element)
    }   
}