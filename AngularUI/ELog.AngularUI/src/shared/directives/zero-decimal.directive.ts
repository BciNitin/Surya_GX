import {

    Directive,
    ElementRef,
    Injectable,
    HostListener,
    Input,
    SimpleChanges,
    OnChanges,
    OnInit,
    Renderer2
} from '@angular/core';

@Directive({
    selector: '[applyDecimal]'
})
@Injectable()
export class ZeroDecimalDirective implements OnInit {

    @Input('applyDecimal') applyDecimal: any;
    @Input('leastCount') leastCount: string;
    @Input('fieldName') fieldName: string;

    constructor(private element: ElementRef, private renderer: Renderer2) {

    }

    ngOnInit() {

        this.onfocusout({});

    }

    @HostListener('focusout', ['$event']) onfocusout($event) {

        if (this.element.nativeElement.value !== undefined && this.element.nativeElement.value !== '' && this.element.nativeElement.value !== null) {
            if (!Number.isNaN(parseFloat(this.element.nativeElement.value))) {

                this.applyDecimal[this.fieldName] = parseFloat(this.element.nativeElement.value);
                let zeroDecimalStr = this.element.nativeElement.value.toString();
                zeroDecimalStr = parseFloat(this.element.nativeElement.value).toFixed(parseInt(this.leastCount));
                //this.element.nativeElement.value = zeroDecimalStr;
                // this.renderer.setValue(this.element.nativeElement, zeroDecimalStr);
                this.renderer.setProperty(this.element.nativeElement, 'value', zeroDecimalStr);

            } else {
                this.renderer.setProperty(this.element.nativeElement, 'value', null);
            }


        }
    }

}
