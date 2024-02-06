import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ApiServiceService } from '@shared/APIServices/ApiService';
import { ValidationService } from '@shared/ValidationService';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { MyErrorStateMatcher, NoWhitespaceValidator } from '@shared/app-component-base';
import { SelectListDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { Title } from '@angular/platform-browser';

interface grid {

}

@Component({
  selector: 'app-quality-sampling',
  templateUrl: './quality-sampling.component.html',
  styleUrls: ['./quality-sampling.component.css'],
  animations: [appModuleAnimation()],
  providers: [ValidationService]
})
export class QualitySamplingComponent implements OnInit {

  public pageSize = 10;
  public currentPage = 0;
  public totalSize = 0;
  public array: any;
  updateUIselectedOrderType: any;

  plantCode: any;
  lineCode: any;
  packingOrder: any;
  cartonBarcode: any;
  itemBarcode: any;

  plnaCodeList: any;
  lineList: any;
  packingOrderList: any;
  picklistItems: [];
  isSelected: boolean;
  Qty: number = 0;
  checked: boolean

  public dataSource: MatTableDataSource<any> = new MatTableDataSource<grid>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<grid>();
  @ViewChild(MatSort, { static: false }) sort!: MatSort;
  @ViewChild('paginator', { static: true }) paginator: MatPaginator;

  constructor(
    private _apiservice: ApiServiceService,
    private formBuilder: FormBuilder,
    public _appComponent: ValidationService,
    private titleService: Title

  ) { }

  ngOnInit() {
    this.titleService.setTitle('Quality Sampling');
    this.GetPlantCode();
  }
  ngAfterViewInit(): void {

  }

  addEditFormGroup: FormGroup = this.formBuilder.group({
    LineCodeFormControl: [null, [Validators.required, NoWhitespaceValidator]],
    plantCodeFormCControl: [null, [Validators.required, NoWhitespaceValidator]],
    packingOrderFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    CartonBarCodeFormControl: ['', [Validators.required, NoWhitespaceValidator]],
    ItemBarCodeFormControl: '',
  });
  matcher = new MyErrorStateMatcher();

  async GetPlantCode() {
    abp.ui.setBusy();
   await this._apiservice.getPlantCode().subscribe((modeSelectList: SelectListDto[]) => {
      this.plnaCodeList = modeSelectList["result"];
      abp.ui.clearBusy();
    },
    (error) => {
      // Handle HTTP error
      abp.ui.clearBusy();
    }
    );
  };

  async GetLineCode() {
    abp.ui.setBusy();
    await this._apiservice.getqualitySamplingLineWorkCenterNo(this.plantCode).subscribe((response) => {
      this.lineList = response["result"];
      abp.ui.clearBusy();
    },
    (error) => {
      // Handle HTTP error
      abp.ui.clearBusy();
    }
    );
  };


  onChangeLineCode() {
    abp.ui.setBusy();
    this._apiservice.QualitySaplingPackingOrderNo(this.plantCode, this.lineCode).subscribe((response) => {
      this.packingOrderList = response["result"];
      this.updateUIselectedOrderType = this.packingOrder;
      abp.ui.clearBusy();
    },
    (error) => {
      // Handle HTTP error
      abp.ui.clearBusy();
    }
    );
  }

  onScaningCarton(value) {
    abp.ui.setBusy();
    const valid = this.markDirty();
   // if (valid && this.addEditFormGroup.valid) {
      this._apiservice.ScanCartonBarCode(this.plantCode, this.lineCode, value.target.value, this.packingOrder)
        .subscribe((response) => {
          if (response["result"][0].error) {
            abp.notify.error(response["result"][0].error);
            abp.ui.clearBusy();

          }
          else {
            if(!this.checked)
            {
          //  abp.notify.success(response["result"][0].valid);
          abp.ui.clearBusy();

            }
          }
        },
        (error) => {
          // Handle HTTP error
          abp.ui.clearBusy();

        }
        )
    // }
    // else {
    //  // abp.notify.error('Please Enter the required value.');
    // }
  }

  onScaningItem(value) {
    abp.ui.setBusy();
    const valid = this.markDirty();
    if (valid && this.addEditFormGroup.valid) {
      this._apiservice.ScanItemBarCode(this.plantCode, this.lineCode, this.cartonBarcode, this.itemBarcode, this.packingOrder)
        .subscribe((response) => {
          if (response["result"][0].error) {
            abp.notify.error(response["result"][0].error);
            abp.ui.clearBusy();
          }
          else {
            abp.notify.success(response["result"][0].valid);
            this.Qty = response["result"][0].qty;
            abp.ui.clearBusy();
          }

        },
        (error) => {
          // Handle HTTP error
          abp.ui.clearBusy();
        }
        )
    }
    else {
      //abp.notify.error('Please Enter the required value.');
    }
  }

  Save() 
  {
    abp.ui.setBusy();
    this._apiservice.SaveQualitySampling(this.plantCode,this.lineCode,this.packingOrder,this.cartonBarcode)
    .subscribe(result => {
            if(result["result"][0].error) {
              abp.notify.error(result["result"][0].error);
              abp.ui.clearBusy();
            }
            else {
              abp.notify.success(result["result"][0].valid);
              this.Qty = 0;
              this.Clear();
              abp.ui.clearBusy();
            }
        },
        (error) => {
          // Handle HTTP error
          abp.ui.clearBusy();
        }
        );
   }



  markDirty() {
    this._appComponent.markGroupDirty(this.addEditFormGroup);
    return true;
  }

  Clear() {

    this.addEditFormGroup.controls['LineCodeFormControl'].setValue(null);
    this.addEditFormGroup.controls['plantCodeFormCControl'].setValue(null);
    this.addEditFormGroup.controls['packingOrderFormControl'].setValue(null);
    this.addEditFormGroup.controls['CartonBarCodeFormControl'].setValue(null);
    this.addEditFormGroup.controls['ItemBarCodeFormControl'].setValue(null);

  }

  onSelectionChanged(check) {
    this.checked = check.checked;
    this.itemBarcode = '';
    if (this.checked) {
      this._appComponent.markControlDisabled(this.addEditFormGroup.controls['ItemBarCodeFormControl']);
    }
    else {
      this._appComponent.markControlEnable(this.addEditFormGroup.controls['ItemBarCodeFormControl']);
    }
  }

  getQuantity() {
    if(this.packingOrder != undefined)
    {
    this._apiservice.GetQualitySamplingQuantity(this.plantCode, this.lineCode, this.packingOrder)
      .subscribe((response) => {
        this.Qty = response["result"][0].qty;

      })
  }
  }
}
