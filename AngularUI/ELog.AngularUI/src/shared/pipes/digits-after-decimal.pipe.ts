import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'digitsafterdecimal'}) 

export class DigitsAfterDecimalPipe implements PipeTransform {

transform(input: number , noOfDigits:any): string{ //string type
    if(input) {
        return input.toFixed(parseInt(noOfDigits))
    }
    return "0.00";
} 
}