import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
    selector: 'usersfilter-dialog',
    templateUrl: 'usersfilter-dialog.html',
  })
  export class UsersFilterDialog {
  
    constructor(
      public dialogRef: MatDialogRef<UsersFilterDialog>,
      @Inject(MAT_DIALOG_DATA) public data: any) { }
  
    onNoClick(): void {
      this.dialogRef.close();
    }
  
  }