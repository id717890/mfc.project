/**
 * @file Busy Config
 * @author yumao<yuzhang.lille@gmail.com>
 */

import {Subscription} from 'rxjs/Subscription';

export class BusyConfig implements IBusyConfig {
    template: string;
    delay: number;
    minDuration: number;
    backdrop: boolean;
    message: string;
    wrapperClass: string;

    constructor(config: IBusyConfig = {}) {
        for (let option in BUSY_CONFIG_DEFAULTS) {
            this[option] = config[option] != null ? config[option] : BUSY_CONFIG_DEFAULTS[option];
        }
    }
}

export interface IBusyConfig {
    template?: string;
    delay?: number;
    minDuration?: number;
    backdrop?: boolean;
    message?: string;
    wrapperClass?: string;
    busy?: Promise<any> | Subscription | Array<Promise<any> | Subscription>
}

export const BUSY_CONFIG_DEFAULTS = {
    delay: 0,
    minDuration: 0,
    backdrop: true,
    message: 'Please wait...',
    wrapperClass: 'ng-busy'
};
