import { BaseModel } from './../../models/base.model';

export class ResponseData<TModel extends BaseModel>  {
    constructor(public count: number, public data: TModel[]){}
}