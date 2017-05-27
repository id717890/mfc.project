import { Component, ViewContainerRef } from '@angular/core';

@Component({
    selector: 'mfc-dialog',
    template: '<h1>Hello Test</h1>'
})

export class TestComponent {
    constructor(public viewContainerRef: ViewContainerRef) { }
}