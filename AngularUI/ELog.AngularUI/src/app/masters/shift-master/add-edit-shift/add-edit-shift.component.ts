import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto, SmartDateTime } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ActivatedRoute, Router } from '@angular/router';
import { Moment } from 'moment';
import * as moment from 'moment';
import { DatePipe } from '@angular/common';
import { parse, toDate } from 'date-fns';
import { date } from '@rxweb/reactive-form-validators';

//import { shiftinput } from '../shift-master.component';
export class shiftinput
{
    
  
    ShiftCode: string="";
    ShiftDescription: string="";
    sShiftStartTime: any;
    sShiftEndTime: Moment;
}
@Component({
  selector: 'app-add-edit-shift',
  templateUrl: './add-edit-shift.component.html',
  styleUrls: ['./add-edit-shift.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})

export class AddEditShiftComponent implements OnInit {
  validationTypes: string[] | null;
  showInstallationError: boolean | false;
  showExpirationError: boolean | false;
  showProcurmentError: boolean | false;
  ShiftModes: any;
  ShiftCode: string="";
  ShiftDescription: string="";
  sShiftStartTime: Moment;
  sShiftEndTime: Moment;

  myDate = new Date();
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private _router: Router,
    private _route: ActivatedRoute,
    public datepipe: DatePipe
  ) {
    
  }

  ngOnInit() {
    
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    //registration: [null, [Validators.required, NoWhitespaceValidator]],
    ShiftCode: [null, [Validators.required, NoWhitespaceValidator]],
    ShiftDescription: [null, [Validators.required, NoWhitespaceValidator]],
    sShiftStartTime: [null, [Validators.required, NoWhitespaceValidator]],
   
    sShiftEndTime:[null,[Validators.required,NoWhitespaceValidator]]
});
matcher = new MyErrorStateMatcher();
Save() {
  debugger;
  var _shiftinput =  new shiftinput();
  _shiftinput.ShiftCode = this.ShiftCode;
  _shiftinput.ShiftDescription = this.ShiftDescription;
  _shiftinput.sShiftStartTime =this.sShiftStartTime;
  //_shiftinput.sShiftStartTime =  moment(moment(this.sShiftStartTime).format('YYYY-MM-DD HH:mm:ss')).toDate();
  
   _shiftinput.sShiftEndTime =   this.sShiftEndTime;
  try {
    this._apiservice.CreateSiftMaster(this.ShiftCode,this.ShiftDescription,this.sShiftStartTime,this.sShiftEndTime).subscribe((response) => {
      this.ShiftModes = response["result"];
    
      console.log("response",response);
      abp.notify.success("Shift has been  added successfully");
      this._router.navigate(['../shift-master'], { relativeTo: this._route });
      this.Clear();
  });
  } catch (error) {
    abp.notify.error("There is error to add the the shift");
  }
  
};
markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

onDateChangeEvent() {
  this.validationTypes = [];
  this.showExpirationError = false;
  this.showInstallationError = false;
  this.showProcurmentError = false;
  var ShiftStartTime = this.addEditFormGroup.get("sShiftStartTime").value;
  //var installation = this.addEditFormGroup.get("dateOfInstallationFormControl").value;
  var ShiftEndTime = this.addEditFormGroup.get("sShiftEndTime").value;
  if (ShiftStartTime > ShiftEndTime) {
      this.showExpirationError = true;
      this.validationTypes.push("frommustbeless");
  }

  this.sShiftStartTime = ShiftStartTime;
  this.sShiftEndTime = ShiftEndTime;

  return true;
}

  Clear() {
        
    this.addEditFormGroup.reset();
  }
  goBack(): void {
    
    this._router.navigate(['../shift-master'], { relativeTo: this._route });
}
}
