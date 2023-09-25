import { Component, Injector } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, FormGroupDirective, NgForm, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ErrorStateMatcher, mixinDisabled, ThemePalette } from '@angular/material';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { RxwebValidators } from '@rxweb/reactive-form-validators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase, MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { ApprovalStatusDto, CreatePlantDto, CreateUserDto, PlantDto, PlantServiceProxy, RoleCheckboxDto, SelectListDto, SelectListServiceProxy, UserDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import * as moment from 'moment';
import { finalize } from 'rxjs/operators';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { ClientFormsDtoPagedResultDto, ClientFormsServiceServiceProxy, ClientFormsDto, ChangePasswordDto ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
// import { ActivatedRoute, Router } from '@angular/router';
import { fromEvent } from 'rxjs';


@Component({
    selector: 'app-add-edit-forms',
    templateUrl: './add-edit-forms.component.html',
    styleUrls: ['./add-edit-forms.component.css'],
    animations: [appModuleAnimation()],
})
export class AddEditFormsComponent extends AppComponentBase {
  constructor(injector:Injector, private formBuilder:FormBuilder){
    super(injector);
  }

  contactForm = this.formBuilder.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: [
      '',
      [
        Validators.required,
        Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$'),
      ],
    ],
  });

  get registerFormControl() {
    return this.contactForm.controls;
  }
  
  onChange(){
    this.contactForm.enable();
  }
  onSubmit(){
    console.log(this.contactForm.value)
  }
  OnBlur(event:any){
    // console.log(this.contactForm.value)
    // console.log(event)
    if(event.target.value)
{
  this.contactForm.controls[event.target.id].disable();
 }
//  else {
//   this.contactForm.enable();
// }

}
  ngOnInit(){

  }
  
//     plantId: number;
//     isdescriptionValid: boolean = true;
//     isOnlyView: boolean = false;
//     plant: PlantDto | null;
//     client:ClientFormsDto|null;
//     plantMasters: SelectListDto[] | null;
//     countries: SelectListDto[] | null;
//     states: SelectListDto[] | null;
//     viewForm: any;


//     id: number;
//     ClientId: number=90;
//     FormName: string="form4";
//     FormStartDate: moment.Moment;
//     FormEndDate: moment.Moment;
//     FormJson: string="lkj";
//     IsActive: boolean=false;
//     CreationDate: moment.Moment;
//     ModifiedDate: moment.Moment;

//     constructor(
//         injector: Injector,

//         private _router: Router,
//         private _route: ActivatedRoute,
//         private _clientService:ClientFormsServiceServiceProxy,
//         private _plantService: PlantServiceProxy,
//         private _selectListService: SelectListServiceProxy,
//         private formBuilder: FormBuilder,
//         private _changePwdService: ChangePswdServiceProxy,

//     ) {
//         super(injector);
//     }
//     matcher = new MyErrorStateMatcher();

//     ngOnInit() {
//         this.setTitle('Add Plant');
//         let that = this;
//         this.plant = new PlantDto();
//         this.plant.isActive = true;
//         this.GetPlantMaster();
//         this.GetCountries();
//         this._route.params.subscribe((routeData: Params) => {
//             console.log("abc")
//             if (routeData['plantId']) {
//                 let plantId = this._changePwdService.decryptPassword(routeData['plantId']);
//                 plantId.subscribe(id => {

//                     that.plantId = parseInt(id);
//                     that.GetPlant(that.plantId);
//                     this.setTitle('Edit Plant');
//                     this.addEditFormGroup.controls['approvalStatusFormControl'].disable();

//                 })

//             }
//             if (routeData['action'] == 'view') {
//                 that.isOnlyView = true;
//                 this.setTitle('Plant Details');
//                 that.markGroupDisabled(that.addEditFormGroup);
//             }
//         });

//         this.addEditFormGroup.get('countryFormControl').valueChanges.subscribe(item => {
//             if (item) {
//                 this.GetStates(item);
//             } else {
//                 this.states = [];
//                 this.plant.stateId = null;
//             }


//         });

//         this.addEditFormGroup.get('plantTypeFormControl').valueChanges.subscribe(item => {

//             if (item == 2) {

//                 this.addEditFormGroup.get('subPlantFormControl').setValidators([Validators.required]);
//                 this.addEditFormGroup.get('subPlantFormControl').updateValueAndValidity();
//             }
//             else {
//                 this.plant.masterPlantId = null;
//                 this.addEditFormGroup.get('subPlantFormControl').clearValidators();
//                 this.addEditFormGroup.get('subPlantFormControl').updateValueAndValidity();
//             }


//         });
//     }



//     addEditForm: FormGroup = this.formBuilder.group({
//         // label:['', ]
//     });


//     addEditFormGroup: FormGroup = this.formBuilder.group({
//         plantIdFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
//         plantNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
//         plantTypeFormControl: ['', [Validators.required]],
//         emailFormControl: ['', [Validators.email, Validators.maxLength(256)]],
//         phoneNumberFormControl: ['', [Validators.pattern(new RegExp("^\\d{10}|\d{11}|\d{12}|\d{13}$"))]],
//         subPlantFormControl: [''],
//         isActiveFormControl: [''],
//         descriptionFormControl: [''],
//         taxRegNoFormControl: [''],
//         licenseFormControl: [''],
//         gs1PrefixFormControl: [''],
//         websiteFormControl: ['', RxwebValidators.url()],
//         postalCodeFormControl: [''],
//         stateFormControl: [''],
//         countryFormControl: [''],
//         cityFormControl: [''],
//         address1FormControl: [''],
//         address2FormControl: [''],
//         approvalStatusFormControl: [''],

//     });


//     markDirty() {
//         this.markGroupDirty(this.addEditFormGroup);
//         return true;
//     }

    
//     GetCreateClientFormsDto(client:ClientFormsDto){
//         let  createClientDto:ClientFormsDto = new ClientFormsDto();
//        // debugger;
//         createClientDto.clientId=this.ClientId;
//         createClientDto.formName=this.FormName;
//         createClientDto.formStartDate=this.FormStartDate;
//         createClientDto.formEndDate=this.FormEndDate;
//         createClientDto.formJson=this.FormJson;
//         createClientDto.isActive=this.IsActive;
//         createClientDto.creationDate=this.CreationDate;
//         createClientDto.modifiedDate=this.ModifiedDate;
//         return createClientDto;
//     }
//     AddClientForms() {
//         let clientFormToAdd = this.client;
//         let createClientFormsDto: ClientFormsDto = this.GetCreateClientFormsDto(clientFormToAdd);
        
//         abp.ui.setBusy();
//         this._clientService.create(createClientFormsDto).pipe(
//             finalize(() => {

//                 abp.ui.clearBusy();
//             })
//         ).subscribe((successData: ClientFormsDto) => {

//             abp.notify.success('Form Created Successfully.');
//             // this.GoToPlantsListFromAdd();
//         });

//     }

//     Submit() {this.AddClientForms();
// console.log(this.viewForm)
//     }

//     GetPlantMaster() {
//         this._selectListService.getAllMasterPlants().subscribe((plantSelectList: SelectListDto[]) => {
//             this.plantMasters = plantSelectList;
//             if (this.plantId) {
//                 this.plantMasters = this.plantMasters.filter(x => x.id != this.plantId);
//             }

//         });
//     }

//     GetCountries() {
//         this._selectListService.getCountries().subscribe((countrySelectList: SelectListDto[]) => {
//             this.countries = countrySelectList;
//         });
//     }

//     GetStates(countryId: number) {
//         this._selectListService.getStates(countryId).subscribe((stateSelectList: SelectListDto[]) => {
//             this.states = stateSelectList;
//         });
//     }




//     GoToAddEditPlant(plantId: any) {
//         this._changePwdService.encryptPassword(plantId).subscribe(
//             data => {

//                 this._router.navigate(['../../../edit-plant', data], { relativeTo: this._route });

//             }
//         );
//         // this._router.navigate(['../../../edit-plant', plantId], { relativeTo: this._route });
//     }
//     GoToViewPlant(plantId: number) {
//         debugger;
//         this._router.navigate(['../add-edit-forms', 'view', plantId], { relativeTo: this._route });
//     }
//     GoToPlantsListFromView() {
//         this._router.navigate(['../../../plants'], { relativeTo: this._route });
//     }
//     GoToPlantsListFromEdit() {
//         this._router.navigate(['../../plants'], { relativeTo: this._route });
//     }
//     GoToPlantsListFromAdd() {
//         this._router.navigate(['../plants'], { relativeTo: this._route });
//     }

//     GetCreatePlantDtoFromPlantDto(plant: PlantDto) {
//         let createPlantDto: CreatePlantDto = new CreatePlantDto();
//         createPlantDto.countryId = plant.countryId;
//         createPlantDto.stateId = plant.stateId;
//         createPlantDto.address1 = plant.address1;
//         createPlantDto.address2 = plant.address2;
//         createPlantDto.city = plant.city;
//         createPlantDto.description = plant.description;
//         createPlantDto.email = plant.email;
//         createPlantDto.gS1Prefix = plant.gS1Prefix;
//         createPlantDto.license = plant.license;
//         createPlantDto.phoneNumber = plant.phoneNumber;
//         createPlantDto.plantId = plant.plantId;
//         createPlantDto.plantName = plant.plantName;
//         createPlantDto.postalCode = plant.postalCode;
//         createPlantDto.masterPlantId = plant.masterPlantId;
//         createPlantDto.taxRegistrationNo = plant.taxRegistrationNo;
//         createPlantDto.website = plant.website;
//         createPlantDto.isActive = plant.isActive;
//         createPlantDto.plantTypeId = plant.plantTypeId;

//         return createPlantDto;
//     }

//     AddPlant() {
//         let plantToAdd = this.plant;
//         let createPlantDto: CreatePlantDto = this.GetCreatePlantDtoFromPlantDto(plantToAdd);

//         abp.ui.setBusy();
//         this._plantService.create(createPlantDto).pipe(
//             finalize(() => {

//                 abp.ui.clearBusy();
//             })
//         ).subscribe((successData: PlantDto) => {

//             abp.notify.success('Plant added successfully.');
//             this.GoToPlantsListFromAdd();
//         });

//     }

//     SavePlant() {
//         let plantToSave = this.plant;
//         abp.ui.setBusy();
//         this._plantService.update(plantToSave).pipe(
//             finalize(() => {

//                 abp.ui.clearBusy();
//             })
//         ).subscribe((successData: PlantDto) => {
//             abp.notify.success('Plant updated successfully.');
//             this.GoToPlantsListFromEdit();
//         });
//     }

//     GetPlant(plantId: number) {
//         abp.ui.setBusy();
//         this._plantService.get(plantId).pipe(
//             finalize(() => {
//                 abp.ui.clearBusy();
//             })
//         ).subscribe((successData: PlantDto) => {
//             this.plant = successData;

//         });
//     }

//     ApprovePlant() {
//         this.isdescriptionValid = true;
//         let approvalStatusEntity = new ApprovalStatusDto();
//         approvalStatusEntity.id = this.plantId;
//         approvalStatusEntity.approvalStatusId = 2;
//         approvalStatusEntity.description = this.plant.approvalStatusDescription;
//         abp.message.confirm(
//             this.l('Do you want to approve?'),
//             (isConfirmed: boolean) => {
//                 if (isConfirmed) {
//                     this._plantService.approveOrRejectPlant(approvalStatusEntity).pipe(
//                         finalize(() => {
//                             abp.ui.clearBusy();
//                         })
//                     ).subscribe(result => {

//                         abp.notify.success('Plant approved successfully.');
//                         this.GoToPlantsListFromView();
//                     })
//                 }
//             }
//         );
//     }

//     RejectPlant() {
//         if (!this.plant.approvalStatusDescription) {
//             this.isdescriptionValid = false;
//             return true;
//         } else if (this.plant.approvalStatusDescription) {
//             if (this.plant.approvalStatusDescription.trim().length == 0) {
//                 this.isdescriptionValid = false;
//                 return true;
//             }
//         }
//         this.isdescriptionValid = true;
//         let approvalStatusEntity = new ApprovalStatusDto();
//         approvalStatusEntity.id = this.plantId;
//         approvalStatusEntity.approvalStatusId = 3;
//         approvalStatusEntity.description = this.plant.approvalStatusDescription;
//         abp.message.confirm(
//             this.l('Do you want to reject?'),
//             (isConfirmed: boolean) => {
//                 if (isConfirmed) {
//                     this._plantService.approveOrRejectPlant(approvalStatusEntity).pipe(
//                         finalize(() => {
//                             abp.ui.clearBusy();
//                         })
//                     ).subscribe(result => {

//                         abp.notify.success('Plant rejected successfully.');
//                         this.GoToPlantsListFromView();
//                     })
//                 }
//             }
//         );
//     }
}