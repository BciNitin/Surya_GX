import { Component, Inject, Injector, OnInit, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { StandardWeightStampingDueListDto ,ChangePswdServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  animations: [appModuleAnimation()],
  templateUrl: './standardweightduedatedilog.component.html',
  styleUrls: ['./standardweightduedatedilog.component.less']
})
export class StandardweightduedatedilogComponent extends AppComponentBase implements OnInit {
  DueOnStandardWeightListList: StandardWeightStampingDueListDto[] | null;
  constructor( injector: Injector, private _dialogRef: MatDialogRef<StandardweightduedatedilogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: any,
   ) { super(injector);
  this.DueOnStandardWeightListList=data.DueOnStandardWeightBoxList;}

  ngOnInit() {  
  }
 
close(result?: any): void {
  this._dialogRef.close(null);
}
}
