import { Router } from '@angular/router';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { Injectable, NgZone } from '@angular/core';
import { el } from 'date-fns/locale';
import { UtilsService } from 'abp-ng2-module/dist/src/utils/utils.service';

const MINUTES_UNITL_AUTO_LOGOUT = 15 // in Minutes
const CHECK_INTERVAL = 1000 // in ms
const STORE_KEY = 'lastAction';

@Injectable()
export class AutoLogoutService {

  constructor(
    private auth: AppAuthService,
    private router: Router,
      private ngZone: NgZone,
      private _utilsService: UtilsService
  ) {
    abp.ui.setBusy();
    this.check();
    this.initListener();
    this.initInterval();
    abp.ui.clearBusy();
  }

  get lastAction() {
      var lastActionVal = localStorage.getItem(STORE_KEY)
      if(lastActionVal == null)
      {
          return Date.now();
      }
      else
      {
        return parseInt(lastActionVal);
      }
  }
  set lastAction(value) {
    localStorage.setItem(STORE_KEY, value.toString());
  }

  initListener() {
    this.ngZone.runOutsideAngular(() => {
      document.body.addEventListener('click', () => this.reset());
      document.body.addEventListener('mouseover', () => this.reset());
      //document.body.addEventListener('mousedown', () => this.reset());
      //document.body.addEventListener('mouseup', () => this.reset());
    });
  }

  initInterval() {
    this.ngZone.runOutsideAngular(() => {
      setInterval(() => {
        this.check();
      }, CHECK_INTERVAL);
    })
  }

  reset() {
    this.lastAction = Date.now();
  }

  check() {
    const now = Date.now();
    const timeleft = this.lastAction + MINUTES_UNITL_AUTO_LOGOUT * 60 * 1000;
    const diff = timeleft - now;
    const isTimeout = diff < 0;

    this.ngZone.run(() => {
      if (isTimeout) {
        console.log(`User has been inactive for ${MINUTES_UNITL_AUTO_LOGOUT}.`);
        localStorage.setItem(STORE_KEY, null);
          this.auth.logout();
          this._utilsService.deleteCookie;
        this.router.navigate(['login']);
      }
    });
  }
}