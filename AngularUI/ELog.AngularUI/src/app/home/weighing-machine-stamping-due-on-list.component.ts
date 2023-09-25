import { Component, Inject, Injector, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { WeighingMachineServiceProxy, WeighingMachineStampingDueOnListDto ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  animations: [appModuleAnimation()],
  templateUrl: './weighing-machine-stamping-due-on-list.component.html',
  styleUrls: ['./weighing-machine-stamping-due-on-list.component.less']
})
export class WeighingMachineStampingDueOnListComponent extends AppComponentBase implements OnInit {
  DueOnWeighingMachineList: WeighingMachineStampingDueOnListDto[] | null;
  constructor( injector: Injector, private _dialogRef: MatDialogRef<WeighingMachineStampingDueOnListComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
   ) { super(injector);
  this.DueOnWeighingMachineList=data.DueOnWeighingMachineList;}

  ngOnInit() {  
  }
 
close(result?: any): void {
  this._dialogRef.close(null);
}
}
