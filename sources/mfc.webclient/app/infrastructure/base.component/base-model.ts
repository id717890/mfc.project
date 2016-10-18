export class BaseModel {
    constructor(public id: number, public caption: string){}

    public clone() :BaseModel {
        return new BaseModel(this.id, this.caption);
    }
}