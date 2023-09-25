import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonChecksService {

  constructor() { }

  commonCheck(event) {
    debugger;
    var k;
    k = event.charCode;  //         k = event.keyCode;  (Both can be used)
     return (k == 92 || k == 95 || (k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 45 && k <= 57));
    }
}
