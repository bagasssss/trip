import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import * as jQuery from 'jquery';

@Directive({ selector: '[localize]' })
export class LocalizeDirective implements OnInit {
    @Input() localize:string;

    constructor(private element: ElementRef, private translate: TranslateService) {
    }

    ngOnInit() {
        this.translate.get(this.localize).toPromise().then(translation => {
            this.setTranslation(translation);
        });
    }

    private setTranslation(translation) {
        var jElement = jQuery(this.element.nativeElement);
        var isInputOrTextArea = jElement.is("input") || jElement.is("textarea");
        var isSubmit = jElement.is("input[type=submit]");

        if (isInputOrTextArea && !isSubmit) {
            jElement.attr("placeholder", translation);
        } else if (isSubmit) {
            jElement.val(translation);
        } else {
            jElement.html(translation);
        }
    }
}