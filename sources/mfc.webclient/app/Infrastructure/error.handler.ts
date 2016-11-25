import { ErrorHandler } from "@angular/core";
import { forwardRef } from "@angular/core";
import { Inject } from "@angular/core";
import { Injectable } from "@angular/core";

import { ErrorLogService } from "./error.service";
import { Modal } from 'angular2-modal/plugins/bootstrap';

export interface LoggingErrorHandlerOptions {
    rethrowError: boolean;
    unwrapError: boolean;
}

export var LOGGING_ERROR_HANDLER_OPTIONS: LoggingErrorHandlerOptions = {
    rethrowError: false,
    unwrapError: false
};

@Injectable()
export class LoggingErrorHandler implements ErrorHandler {

    private errorLogService: ErrorLogService;
    private options: LoggingErrorHandlerOptions;

    // I initialize the service.
    // --
    // CAUTION: The core implementation of the ErrorHandler class accepts a boolean
    // parameter, `rethrowError`; however, this is not part of the interface for the
    // class. In our version, we are supporting that same concept; but, we are doing it
    // through an Options object (which is being defaulted in the providers).
    constructor(
        errorLogService: ErrorLogService,
        @Inject(LOGGING_ERROR_HANDLER_OPTIONS) options: LoggingErrorHandlerOptions,
        private modal: Modal
    ) {

        this.errorLogService = errorLogService;
        this.options = options;
    }

    // PUBLIC METHODS.

    // I handle the given error.
    public handleError(error: any): void {
        // Log to the console.
        try {
            console.group("ErrorHandler");
            console.error(error.message);
            console.error(error.stack);
            console.groupEnd();
        } catch (handlingError) {
            console.group("ErrorHandler");
            console.warn("Error when trying to output error.");
            console.error(handlingError);
            console.groupEnd();
        }

        /* Вызываем модальное окно ошибки */
        try {
            this.errorToModal(error);
        }
        catch (handlingError) {
            console.log(handlingError);
        }

        // Send to the error-logging service.
        try {
            this.options.unwrapError
                ? this.errorLogService.logError(this.findOriginalError(error))
                : this.errorLogService.logError(error)
                ;
        } catch (loggingError) {
            console.group("ErrorHandler");
            console.warn("Error when trying to log error to", this.errorLogService);
            console.error(loggingError);
            console.groupEnd();
        }

        if (this.options.rethrowError) {
            throw (error);
        }
    }

    // PRIVATE METHODS.

    // I attempt to find the underlying error in the given Wrapped error.
    private findOriginalError(error: any): any {
        while (error && error.originalError) {
            error = error.originalError;
        }
        return (error);
    }

    // Вызов модального окна для уведомления пользователя об ошибке
    private errorToModal(error: any) {
        this.modal.alert()
            .isBlocking(false)
            .showClose(false)
            .keyboard(27)
            .title('Произошла ошибка!')
            .body(`
                <label>Ошибка:</label>
                <p>`+ error.message + `</p>
                <label>Stack</label>
                <div class="panel panel-default">
                    `+ error.stack + `
                </div>
                `
            )
            .bodyClass('modal-body-error')
            .okBtn('Закрыть')
            .open();
    }
}

// I am the collection of providers used for this service at the module level.
// Notice that we are overriding the CORE ErrorHandler with our own class definition.
// --
// CAUTION: These are at the BOTTOM of the file so that we don't have to worry about
// creating futureRef() and hoisting behavior.
export var LOGGING_ERROR_HANDLER_PROVIDERS = [
    {
        provide: LOGGING_ERROR_HANDLER_OPTIONS,
        useValue: LOGGING_ERROR_HANDLER_OPTIONS
    },
    {
        provide: ErrorHandler,
        useClass: LoggingErrorHandler
    }
];