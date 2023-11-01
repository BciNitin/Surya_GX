import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, ViewChildren, OnInit, NgModule, ViewChild, QueryList, AfterViewInit } from '@angular/core';
import { MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ValidationService } from '@shared/ValidationService';
import { NoWhitespaceValidator, MyErrorStateMatcher } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
export class Tracking {

  QRBarcode: string = "";
  
}
@Component({
  selector: 'app-warranty-tracking',
  templateUrl: './warranty-tracking.component.html',
  styleUrls: ['./warranty-tracking.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class WarrantyTrackingComponent implements OnInit {
  QRBarcode: string = "";
  
  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService
  ) { }

  ngOnInit() {
  }
  public dataSource: MatTableDataSource<any> = new MatTableDataSource<Tracking>();
  addEditFormGroup: FormGroup = this.formBuilder.group({


    QRBarcodeBarcodeFormControl:['', [Validators.required, NoWhitespaceValidator]],
    SerialNoFormControl:['', [Validators.required, NoWhitespaceValidator]]

  });
  GrtNonbarcodedGrid() {
    
      
      var _tracking = new Tracking();
      _tracking.QRBarcode = this.QRBarcode;
      
      this._apiservice.GetWarrantyTrackingDtls(this.QRBarcode).subscribe((response) => {
        
        if (response["result"][0]['error']) {
          abp.notify.error(response["result"][0]['error']);
          

        }
        else {
         
          this.dataSource.filteredData=response["result"];
        }

      })
   
  }
}
