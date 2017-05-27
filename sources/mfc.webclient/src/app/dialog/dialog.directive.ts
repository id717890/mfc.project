import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
    selector: '[mfc-dialog]'
})

export class DialogDirective {
    constructor(public viewContainerRef: ViewContainerRef) { }
}