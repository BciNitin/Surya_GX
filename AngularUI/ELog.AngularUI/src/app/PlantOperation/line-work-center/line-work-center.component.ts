import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators,ValidatorFn, AbstractControl } from '@angular/forms';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ActivatedRoute, Router } from '@angular/router';
import { AppComponent } from '@app/app.component';
import { finalize, debounceTime, distinctUntilChanged, map, filter, tap, switchMap } from 'rxjs/operators';
import { error } from 'console';
import { Title } from '@angular/platform-browser';

export class linework
{
     binMapping: any;
     barcode: string="";
     lineBarCode: string="";
     plantCode: string="";
    
}
@Component({
  selector: 'app-line-work-center',
  templateUrl: './line-work-center.component.html',
  styleUrls: ['./line-work-center.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})

export class LineWorkCenterComponent implements OnInit {
  resultValue: any;
  
  barcode: string="";
  lineBarCode: string="";
  plantCode: string="";
  
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent : ValidationService,
    private _router: Router,
    private _route: ActivatedRoute,
    private titleService: Title

    

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Line/Work Center and Bin Mapping');
  }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    //registration: [null, [Validators.required, NoWhitespaceValidator]],
    barcode: [null, [Validators.required, NoWhitespaceValidator]],
    lineBarCode:[null,[Validators.required,NoWhitespaceValidator]]
});
matcher = new MyErrorStateMatcher();
 Save() {
  var _linework =  new linework();
  _linework.barcode = this.barcode;
 _linework.lineBarCode = this.lineBarCode;
  this._apiservice.SaveLineWork(this.barcode,this.lineBarCode).subscribe(result => {
    
           if(result["result"][0]['valid'])
           {
             abp.notify.success(result["result"][0]['valid']);
           }
           else
           {
            abp.notify.error(result["result"][0]['error']);
           }
           this.Clear();
      });
  
}
Clear() {
        
  this.addEditFormGroup.reset();
}
markDirty() {
  this._appComponent.markGroupDirty(this.addEditFormGroup);
   return true;
}

}
