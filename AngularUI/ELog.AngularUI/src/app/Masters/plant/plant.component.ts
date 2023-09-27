import { Component, ElementRef, Injector, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { PagedListingComponentBase, PagedRequestDto } from 'shared/paged-listing-component-base';
import { PlantServiceProxy, PlantDto, SelectListServiceProxy, ChangePswdServiceProxy,ElogSuryaApiServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { relative } from 'path';
import { FormControl } from '@angular/forms';
import * as moment from 'moment';
import { fromEvent } from 'rxjs';

class PagedPlantsRequestDto extends PagedRequestDto {
    keyword: string;
    isActive: boolean | null;
}

@Component({
    templateUrl: './plant.component.html',
    animations: [appModuleAnimation()],
    providers: [ElogSuryaApiServiceServiceProxy]
    //styleUrls: ['./plant.component.less']
})
export class PlantComponent extends PagedListingComponentBase<PlantDto> {
  plants :any;
  code ='';
  type ='';
  description = '';
  address = '';


    @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;

    constructor(
        injector: Injector,
        private _changePwdService: ChangePswdServiceProxy,private _plantService: ElogSuryaApiServiceServiceProxy,
        private _selectListService: SelectListServiceProxy,
        private _dialog: MatDialog,
        private _router: Router,
        private _route: ActivatedRoute
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.setTitle('Plant Master');
        fromEvent(this.searchTextBox.nativeElement, 'keyup').pipe(

            // get value
            map((event: any) => {
                return event.target.value;
            })
            // Time in milliseconds between key events
            , debounceTime(1000)

            // If previous query is diffent from current
            , distinctUntilChanged()

            // subscription for response
        ).subscribe((text: string) => {
            this.refresh();
        });
        super.ngOnInit();
    }

  
    GetSortBy(sortBy: number, orderBy: number) {
        let order = '';
        if (orderBy && orderBy != -1) {
            if (orderBy == 1) {
                order = 'asc'
            } else if (orderBy == 2) {
                order = 'desc';
            }
        }
        if (sortBy == 1) {
            return `UserListDto.UserName ${order}`;
        } else if (sortBy == 2) {
            return `UserListDto.CreationTime ${order}`;
        } else if (sortBy == 3) {
            return `UserListDto.ApprovalStatusId ${order}`
        }
        else {
            return '';
        }

    }

    GoToViewUser(user: any) {
        this._changePwdService.encryptPassword(user.id).subscribe(
            data => {

                this._router.navigate(['../user', 'view', data], { relativeTo: this._route });

            }
        );

        // this._router.navigate(['../user', 'view', user.id], { relativeTo: this._route });
    }
   
    addUser() {
        this._router.navigate(['../add-user'], { relativeTo: this._route });
    }
    protected list(

     ): void {
      debugger;
     abp.ui.setBusy();
     
     this._plantService.getPlantMaster().pipe(
     finalize(() => {
     abp.ui.clearBusy();
     })
     ).subscribe({
      next: dt => {
     debugger;
     if (dt == null) {
      return;
      }
     this.plants = dt;
      
      },
      error: error => {
     console.error('There was an error!', error);
     }
     });
     
     }
    
    public onScroll() {  
        this.pageNumber = this.pageNumber + 1;
        this.getDataPage(this.pageNumber);
      } 
      onSelect($event,user: PlantDto): void
      {
         if($event.target.classList.contains('deleteUser'))
          {
              this.delete(user);
          }
          else
          {
              this.GoToViewUser(user);
          }
      }
    protected delete(user: PlantDto): void {
        // abp.message.confirm(
        //     this.l('Are you sure you want to delete this user?', user.userName),
        //     (result: boolean) => {
        //         if (result) {
        //             abp.ui.setBusy();
        //             this._userService.delete(user.id).pipe(
        //         finalize(() => {
                    
        //             abp.ui.clearBusy();
        //         })
        //     ).subscribe(() => {
        //                 abp.notify.success(this.l('User deleted successfully.'));
        //                 this.users = [];
        //                 this.pageNumber=1;
        //                 this.refresh();
        //             });
        //         }
        //     }
        // );
    }

    
}