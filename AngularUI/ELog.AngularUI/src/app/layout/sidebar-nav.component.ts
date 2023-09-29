import { Component, Injector, ViewEncapsulation } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { MenuItem } from "@shared/layout/menu-item";
import { AppSessionService } from "../../shared/session/app-session.service";
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  finalize,
  map,
} from "rxjs/operators";
import {
  SelectListServiceProxy,
  SelectListDto,
  ClientFormsDtoPagedResultDto,
  ClientFormsServiceServiceProxy,
  ClientFormsDto,
  ChangePasswordDto,
  ChangePswdServiceProxy,
  ElogApiServiceServiceProxy,
} from "@shared/service-proxies/service-proxies";
import { CDK_TABLE_TEMPLATE } from "@angular/cdk/table";
import { da } from "date-fns/locale";

@Component({
  templateUrl: "./sidebar-nav.component.html",
  selector: "sidebar-nav",
  encapsulation: ViewEncapsulation.None,
  styleUrls: ["./sidebar-nav.component.less"],
})
export class SideBarNavComponent extends AppComponentBase {
  appSession: AppSessionService;
  nameLowwer: any;
  formName: any;
  FormNames: any = [];
  MenuId:any = [];
  MenuList = [];
  getMenu =[];
  valuesOnly:any;
  clientFormJson: any;
  DBName: any;
  menuItems: MenuItem[] = [
    new MenuItem(this.l("Home"), "", "home", "/app/home"),
    // new MenuItem(
    //   this.l("Users Management"),
    //   "",
    //   "manage_accounts",
    //   "/app/users"
    // ), 
    // new MenuItem(
    //   this.l("Role Management"),
    //   "",
    //   "people",
    //   "/app/roles"
    // ),
    // new MenuItem(
    //   this.l("Password Management"),
    //   "",
    //   "password",
    //   "/app/password"
    // ),
    new MenuItem(
      this.l("Creator"),
      "ElogCreator.Add",
      "summarize",
      "/app/Creator"
    ),
    // new MenuItem(this.l('Reports'), '', 'summarize', '/app/logData'),
    new MenuItem(
      this.l("Saved Forms"),
      "ElogCreator.View",
      "summarize",
      "/app/log-forms-list"
    ),
    //  new MenuItem(this.l('Dummy Forms'), 'DummyShow.View', '', '/app/dummy-show'),
    ///  new MenuItem(this.l('Notifications'), '', 'notifications', '/app/notifications'),
    //     new MenuItem(this.l('Transactions'), '', 'sync_alt', '/app/transactions'),
  ];
  result: any;
  nameLowwer2: any;
   
  MenuType: any[];

  constructor(
    injector: Injector,
    private _clientFormsService: ClientFormsServiceServiceProxy,
    private _selectListService: SelectListServiceProxy,
    private _elogservice:ElogApiServiceServiceProxy
  ) {
    super(injector);
    this.appSession = injector.get(AppSessionService);
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit() {
    this.MenuList = [];
    this.MenuType = [];
    // this.getMenuType();
    // this.getAllMenu();
  }

  showMenuItem(menuItem): boolean {
    if (menuItem.permissionName) {
      return this.isGranted(menuItem.permissionName);
    }

    return true;
  }

// getMenuType(){
//   this._elogservice.fetchTableWiseData("Menu","Name",10,null,null).pipe(
//     finalize(() => {
//       abp.ui.clearBusy();
//     })
//   ).subscribe({
//     next: data => {
//       const newD = JSON.parse(data);
//       this.valuesOnly = Object.values(newD); 
//       this.getMenu.push(this.valuesOnly);
// console.log(this.getMenu);
//     }
// });
// }
  // getAllMenu() {
  //   this._clientFormsService
  //     .getAll(null, null, null, true, null, null, null,null,null, 0, 200)
  //     .pipe(
  //       finalize(() => {
  //         abp.ui.clearBusy();
  //       })
  //     )
  //     .subscribe((result: ClientFormsDtoPagedResultDto) => {
  //       if (result.items.length > 0) {
  //         this.MenuList.push(result.items);
  //         for (var i = 0; i < result.items.length; i++) {
  //           this.clientFormJson = JSON.parse(result.items[i].formJson);
  //           var FormNamewithString = this.clientFormJson.form_name.replaceAll("_"," ");
  //           this.FormNames.push(FormNamewithString);
            
  //         }
  //       }

       

  //     });
  // }
  toggle(id) {
    $("#" + id);
    if ($("#" + id).css("display") == "none") {
      $("#" + id).show();
    } else {
      $("#" + id).hide();
    }
    $("#" + id)
      .siblings("a")
      .toggleClass("toggled");
  }

  isPermissionGranted(permissionName?: string) {
    if (permissionName != null && this.isGranted(permissionName)) {
      return true;
    } else {
      return false;
    }
  }
  hideMenuBar() {
    $("#menuOverlayDiv").css("display", "none");
    $("body").removeClass("overlay-open");
  }
}
