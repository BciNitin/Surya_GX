import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { NoWhitespaceValidator } from '@shared/app-component-base';
interface ShiftMasterEdit {
  id:string,
  shift_code: string,
  shift_description: string,
  shift_startTime: string,
  shift_endTime: string,
}
@Component({
  selector: 'app-add-edit-shift',
  templateUrl: './add-edit-shift.component.html',
  styleUrls: ['./add-edit-shift.component.css'],
  animations: [appModuleAnimation()],
})
export class AddEditShiftComponent implements OnInit {
  public dataSource: MatTableDataSource<any> = new MatTableDataSource<ShiftMasterEdit>();
  public dataSourcePagination: MatTableDataSource<any> = new MatTableDataSource<ShiftMasterEdit>();
  constructor(
    private formBuilder: FormBuilder
  ) { }
  addEditFormGroup: FormGroup = this.formBuilder.group({
    shiftcodeFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
    shiftdespNameFormControl: ['', [Validators.required, Validators.maxLength(64), NoWhitespaceValidator]],
  })
  ngOnInit() {
  }

}
