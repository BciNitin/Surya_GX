import { Component, OnInit } from '@angular/core';
import { NotificationDto, NotificationServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-notifications-center',
  templateUrl: './notifications-center.component.html',
  styleUrls: ['./notifications-center.component.css']
})
export class NotificationsCenterComponent implements OnInit {

  constructor(private _notification:NotificationServiceServiceProxy) {
   
   }

  ngOnInit() {
    this.getAllNotification();
  }

  getAllNotification() {
      
    this._notification.getAll( null , null,null,null,null,null, null,null,null,null,null,0,10)
        .pipe(
            finalize(() => {
          
                abp.ui.clearBusy();
            })
        )
        .subscribe((data) => {
          console.log(data);
            //if (result.items.length > 0) {
                // console.log(result.items);
              //  this.MenuList.push(result.items);
                //console.log(this.plants)                
           // }
        });
}

}
