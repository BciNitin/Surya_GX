import { Component, ElementRef, Injector, OnInit, ViewChild, ɵɵqueryRefresh } from '@angular/core';
import { MatDialog } from '@angular/material';
import { ActivatedRoute, Router } from '@angular/router';
import { ElogSuryaApiServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, finalize, map } from 'rxjs/operators';

@Component({
  selector: 'app-material',
  templateUrl: './material.component.html',
  styleUrls: ['./material.component.css']
})
export class MaterialComponent implements OnInit {

  @ViewChild('searchTextBox', { static: true }) searchTextBox: ElementRef;
  constructor(
    injector: Injector,
    private _materialService:ElogSuryaApiServiceServiceProxy,
    private _dialog: MatDialog,
    private _router: Router,
    private _route: ActivatedRoute
  ) {
    //super(injector);
   }

  // ngOnInit() {
  // }
  ngOnInit() {
  }
}
