import { Injector, ElementRef } from '@angular/core';
import { AppConsts } from '@shared/AppConsts';
import { LocalizationService } from '@abp/localization/localization.service';
import { PermissionCheckerService } from '@abp/auth/permission-checker.service';
import { FeatureCheckerService } from '@abp/features/feature-checker.service';
import { NotifyService } from '@abp/notify/notify.service';
import { SettingService } from '@abp/settings/setting.service';
import { MessageService } from '@abp/message/message.service';
import { AbpMultiTenancyService } from '@abp/multi-tenancy/abp-multi-tenancy.service';
import { AppSessionService } from '@shared/session/app-session.service';
import { AbstractControl, FormArray, FormControl, FormGroup, FormGroupDirective, NgForm, ValidatorFn } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material';
import { Title } from '@angular/platform-browser';
import * as moment from 'moment';
import { Router } from '@angular/router';

export function NoWhitespaceValidator(control: FormControl) {
    try {
        const isWhitespace = (control.value || '').trim().length === 0;
        const isValid = !isWhitespace;
        return isValid ? null : { 'whitespace': true };
    }
    catch (e) {
        return null;
    }

}

export function NoZeroValidator(control: FormControl) {
  const isZero = control.value == 0;
  const isValid = !isZero;
  return isValid ? null : { 'zero': true };
}

/** Error when invalid control is dirty, touched, or submitted. */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
export abstract class AppComponentBase {
    localizationSourceName = AppConsts.localization.defaultLocalizationSourceName;
    localization: LocalizationService;
    feature: FeatureCheckerService;
    notify: NotifyService;
    setting: SettingService;
    message: MessageService;
    multiTenancy: AbpMultiTenancyService;
    appSession: AppSessionService;
    elementRef: ElementRef;
    titleService: Title;
    router:Router;
    DecimalPattern:RegExp=new RegExp("^[+-]?[0-9]{1,9}(?:\.[0-9]{1,15})?$");
    MAXRESULTCOUNT:number=2147483647;
    todayDate:moment.Moment=moment(new Date());
    constructor(injector: Injector) {
        this.localization = injector.get(LocalizationService);
        this.feature = injector.get(FeatureCheckerService);
        this.notify = injector.get(NotifyService);
        this.setting = injector.get(SettingService);
        this.message = injector.get(MessageService);
        this.multiTenancy = injector.get(AbpMultiTenancyService);
        this.appSession = injector.get(AppSessionService);
        this.elementRef = injector.get(ElementRef);
        this.titleService=injector.get(Title);
        this.router=injector.get(Router);
    }

    public setTitle(newTitle: string) {
      this.titleService.setTitle(newTitle);
    }

    l(key: string, ...args: any[]): string {
        let localizedText = this.localization.localize(key, this.localizationSourceName);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args.unshift(localizedText);
        return abp.utils.formatString.apply(this, args);
    }



    markGroupDirty(formGroup: FormGroup) {
      Object.keys(formGroup.controls).forEach(key => {
        
        if(formGroup.get(key)  instanceof FormGroup){
          this.markGroupDirty(formGroup.get(key)  as FormGroup);
        }
        if(formGroup.get(key)  instanceof FormArray){
          this.markArrayDirty(formGroup.get(key)  as FormArray);
        }
        if(formGroup.get(key)  instanceof FormControl){
          this.markControlDirty(formGroup.get(key)  as FormControl);
        }
      });
      }
      markArrayDirty(formArray: FormArray) {
      formArray.controls.forEach(control => {
        if(control instanceof FormGroup){
          this.markGroupDirty(control as FormGroup);
        }
        if(control instanceof FormArray){
          this.markArrayDirty(control as FormArray);
        }
        if(control instanceof FormControl){
          this.markControlDirty(control as FormControl);
        }
       
       
       });
      }
    markControlDirty(formControl: FormControl) {
         formControl.markAsDirty();
    }

    markGroupDisabled(formGroup: FormGroup) {
      Object.keys(formGroup.controls).forEach(key => {
        
        if(formGroup.get(key)  instanceof FormGroup){
          this.markGroupDisabled(formGroup.get(key)  as FormGroup);
        }
        if(formGroup.get(key)  instanceof FormArray){
          this.markArrayDisabled(formGroup.get(key)  as FormArray);
        }
        if(formGroup.get(key)  instanceof FormControl){
          this.markControlDisabled(formGroup.get(key)  as FormControl);
        }
      });
      }
      markArrayDisabled(formArray: FormArray) {
      formArray.controls.forEach(control => {
        if(control instanceof FormGroup){
          this.markGroupDisabled(control as FormGroup);
        }
        if(control instanceof FormArray){
          this.markArrayDisabled(control as FormArray);
        }
        if(control instanceof FormControl){
          this.markControlDisabled(control as FormControl);
        }
       
       
       });
      }
      markControlDisabled(formControl: AbstractControl) {
           formControl.disable();
    }
      isPermissionGranted(permissionName?: string) {
        if (permissionName != null && this.isGranted(permissionName)) {
            return true;
        } else { return false; }
    }
    isFromDateValid(fromDate: moment.Moment, toDate: moment.Moment) {
        if (fromDate <= toDate) {
            return true;
        }
        return false;
    }
    isToDateValid(fromDate: moment.Moment, toDate: moment.Moment) {
        if (toDate >= fromDate) {
            return true;
        }
        return false;
    } 
    isGranted(permissionName: string): boolean {           
        if (permissionName != null && this.appSession.isPermissionGranted(permissionName) && this.appSession.isSubmoduleActive(permissionName)) {
          return true;
        } else { return false;}
    }    

    autocompleteStringValidator(validOptions): ValidatorFn {
      if (validOptions != undefined) {

          return (control: AbstractControl): { [key: string]: any } | null => {

              for (var i = 0; i < validOptions.length; i++) {
                  if (validOptions[i].value == control.value) {
                      return null  /* valid option selected */
                  }
              }
              return { 'invalidAutocompleteString': { value: control.value } }
          }
      }
  }
    
  GoToDashboard() {
    this.router.navigateByUrl('/');
}
}