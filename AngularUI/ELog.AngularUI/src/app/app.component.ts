import { Component, ViewContainerRef, Injector, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { AutoLogoutService } from '@shared/autologout/autologout.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './app.component.html'
})
export class AppComponent extends AppComponentBase implements OnInit, AfterViewInit {

    private viewContainerRef: ViewContainerRef;
    topBar:boolean;

    constructor(
        injector: Injector
        , private autoLogout: AutoLogoutService,
        private activatedRoute: ActivatedRoute
    ) {
                super(injector);
                console.log("App Component Initialize");
                var lastAction = Date.now();
                localStorage.setItem('lastAction',lastAction.toString())
                // this.configure();
                // this.oauthService.tryLoginImplicitFlow();
    }

    ngOnInit(): void {
        this.topBar = true;
console.log("App Component");
        abp.event.on('abp.notifications.received', userNotification => {
            abp.notifications.showUiNotifyForUserNotification(userNotification);

            // Desktop notification
            Push.create('AbpZeroTemplate', {
                body: userNotification.notification.data.message,
                icon: abp.appPath + 'assets/app-logo-small.png',
                timeout: 6000,
                onClick: function () {
                    window.focus();
                    this.close();
                }
            });
        });
        if (location.pathname.match(/Creator/)){
            this.topBar = false;
        }
        // this.activatedRoute.fragment.subscribe((fragment: string) => {
        //     console.log(fragment);// OUTPUT ?productid=1543&color=red 
        //   })

        //   let productid = this.activatedRoute.snapshot.params.productid;
    }
    // private configure() {
    //     this.oauthService.configure(authConfig);
    //     // this.oauthService.tokenValidationHandler = new NullValidationHandler();
    //     this.oauthService.loadDiscoveryDocument(DiscoveryDocumentConfig.url);
    //   }
    

    ngAfterViewInit(): void {
        $.AdminBSB.activateAll();
        $.AdminBSB.activateDemo();
    }

    onResize(event) {
        // exported from $.AdminBSB.activateAll
        $.AdminBSB.leftSideBar.setMenuHeight();
        $.AdminBSB.leftSideBar.checkStatuForResize(false);

        // exported from $.AdminBSB.activateDemo
        $.AdminBSB.demo.setSkinListHeightAndScroll();
        $.AdminBSB.demo.setSettingListHeightAndScroll();
    }
}
