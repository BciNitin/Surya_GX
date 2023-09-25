import { Component, Inject, Injector, OnInit, Optional } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { AppComponentBase } from '@shared/app-component-base';
import { SelectListServiceProxy, SelectListDto,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import {MAT_DIALOG_DATA} from '@angular/material/dialog';
@Component({
  templateUrl: './plant-change-dialog.component.html',
  styles: [
    `
      mat-form-field {
        width: 100%;       
          padding-top:15px;
      }
    `
  ]
})
export class PlantChangeDialogComponent extends AppComponentBase implements OnInit {

  saving = false;
  plantId = '';
  currentUserId:number|null;
  plantMasters: SelectListDto[] | null;
  constructor(
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
    injector: Injector,
    private _dialogRef: MatDialogRef<PlantChangeDialogComponent>,
    private _selectListService: SelectListServiceProxy,
  ) {
    super(injector);
    this.currentUserId=this.data.userId;
  }
  ngOnInit(): void {
   this.GetPlants();
  }

  save(result?: any): void {
    this._dialogRef.close(this.plantId);
  }

  close(result?: any): void {
    this._dialogRef.close(null);
  }
  GetPlants(){
    abp.ui.setBusy();
    this._selectListService.getAssociatedPlantByUserId(this.currentUserId).pipe(
      finalize(() => {
          abp.ui.clearBusy();
      })
  ).subscribe((plantSelectList: SelectListDto[]) => {
        this.plantMasters = plantSelectList;
    });
}
}
