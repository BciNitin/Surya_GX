import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
//import { AddEditFormsComponent } from './add-edit-forms/add-edit-forms.component';
import { ElogPanelComponent } from './elog-panel.component';
import { NewPanelComponent } from './new-panel/new-panel.component';
// import { HellosComponent } from './hellos/hellos.component';


@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: ElogPanelComponent,
                children: [
                 //   { path: 'add-edit-forms', component: AddEditFormsComponent },                   
                    { path: 'createlogs', component: NewPanelComponent },                   
               
                ]
            }
        ])
    ],
    exports: [ 
        RouterModule
    ]
})
export class ElogPanelRoutingModule { }

