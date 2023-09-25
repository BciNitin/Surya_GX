import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";

@Component({
    selector: 'rolesfilter-dialog',
    templateUrl: 'rolesfilter-dialog.html',
})
export class RolesFilterDialog {
    constructor(
        public dialogRef: MatDialogRef<RolesFilterDialog>,
        @Inject(MAT_DIALOG_DATA) public data: any) { }

    onNoClick(): void {
        this.dialogRef.close();
    }
}