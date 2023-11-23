import { Component, Injector, AfterViewInit, OnInit } from "@angular/core";
import { AppComponentBase } from "@shared/app-component-base";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import { MatDialog } from "@angular/material";
import { WeighingMachineStampingDueOnListComponent } from "./weighing-machine-stamping-due-on-list.component";
//import { MaterialIStatusServiceProxy, MaterialStatusDto, StandardWeightServiceProxy, StandardWeightStampingDueListDto, WeighingMachineServiceProxy, WeighingMachineStampingDueOnListDto } from "@shared/service-proxies/service-proxies";
import { StandardweightduedatedilogComponent } from "./standardweightduedatedilog/standardweightduedatedilog.component";
//import { MaterialstatusdilogComponent } from "./materialstatusdilog/materialstatusdilog.component";
import { isThisMonth } from "date-fns/esm";

@Component({
    templateUrl: "./home.component.html",
    animations: [appModuleAnimation()],
})
export class HomeComponent
    extends AppComponentBase
    implements AfterViewInit, OnInit {
    passwordStatus: string;
    // DueOnWeighingMachineList: WeighingMachineStampingDueOnListDto[] | null;
    // DueOnStandardWeightBox:StandardWeightStampingDueListDto[]|null;
    // MaterialStatusDto:MaterialStatusDto[]|null;
    constructor(injector: Injector, private _dialog: MatDialog, 
        // private _weighingMachineService: WeighingMachineServiceProxy,
        // private _standardWeightAppService:StandardWeightServiceProxy,
        // private _materialIStatusAppService:MaterialIStatusServiceProxy
        ) {
        super(injector);
    }

    ngAfterViewInit(): void { }

    ngOnInit(): void {
        this.setTitle('Home');    
        this.CheckPasswordStatus();
     /*   this.GetDueOnWeighingMachineList(); 
        this.GetStandardWeightStampingDueListDtoList();    
        this.GetMaterialStatusList(); */ 
        
    }

    CheckPasswordStatus() {
        this.passwordStatus = localStorage.getItem('PasswordStatus');
        if (this.passwordStatus == "2") {
            this.message.info((this.l('Reset Password')));
        }

        if(this.appSession.getShownPasswordResetDaysLeft() < 5) {
            this.message.info((this.l('Reset Password')));
            console.log("reset password")

        }

    }
    // openWMStampingDialogComponent(): void {
    //     const dialogRef = this._dialog.open(WeighingMachineStampingDueOnListComponent, {   
    //       disableClose: true,  
    //       data: { DueOnWeighingMachineList: this.DueOnWeighingMachineList}          
    //     });
    // }
    // openMaterialStatusDialogComponent(): void {
    //     const dialogRef = this._dialog.open(MaterialstatusdilogComponent, {   
    //       disableClose: true,  
    //       data: { MaterialStatusDto: this.MaterialStatusDto}          
    //     });
    // }
    // openStandardWeightBoxStampingDialogComponent(): void {
    //     const dialogRef = this._dialog.open(StandardweightduedatedilogComponent, {      
    //       disableClose: true,  
    //       data: { DueOnStandardWeightBoxList: this.DueOnStandardWeightBox}          
    //     });
    // }
    // GetDueOnWeighingMachineList() {
    //     this._weighingMachineService.getStampingDueOnWMList()
    //     .subscribe((weighinMachineSelectList: WeighingMachineStampingDueOnListDto[]) => {
    //         this.DueOnWeighingMachineList = weighinMachineSelectList;
    //         if(weighinMachineSelectList.length>0){
    //             this.openWMStampingDialogComponent();
    //         }         
    //     });
    // }
    // GetMaterialStatusList() {
    //     this._materialIStatusAppService.getAllMaterilStatusList()
    //     .subscribe((MaterialStatusDto: MaterialStatusDto[]) => {
    //         this.MaterialStatusDto = MaterialStatusDto;
    //         if(MaterialStatusDto.length>0){
    //             this.openMaterialStatusDialogComponent();
    //         }         
    //     });
    // }
    // GetStandardWeightStampingDueListDtoList() {
    //     this._standardWeightAppService.getStandardWeightStampingDueList()
    //     .subscribe((DueOnStandardWeightBox: StandardWeightStampingDueListDto[]) => {
    //         this.DueOnStandardWeightBox = DueOnStandardWeightBox;
    //         if(DueOnStandardWeightBox.length>0){
    //             this.openStandardWeightBoxStampingDialogComponent();
    //         }         
    //     });
    // }
}
