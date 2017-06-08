import { BaseModel } from './../../models/base.model';

export class BaseContext <TModel extends BaseModel> {
    public model: TModel;

    constructor(object: TModel) {
        this.model = object;
    }
}