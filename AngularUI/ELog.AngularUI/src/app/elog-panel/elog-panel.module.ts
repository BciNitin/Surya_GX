import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { AddEditFormsComponent } from './add-edit-forms/add-edit-forms.component';

import { ElogPanelRoutingModule } from './elog-panel-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
// import { HellosComponent } from './hellos/hellos.component';
import { AddEditFormsComponent } from './add-edit-forms/add-edit-forms.component';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';

//import { NewPanelComponent } from './new-panel/new-panel.component';



@NgModule({
  imports: [
    CommonModule,
    ElogPanelRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    
  ],
  declarations: [
    AddEditFormsComponent,
   
    // HellosComponent,
   // AddEditFormsComponent,
  //  NewPanelComponent
  ],


})
export class ElogPanelModule { }

