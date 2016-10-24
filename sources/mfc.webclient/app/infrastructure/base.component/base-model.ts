export class BaseModel {
    constructor(public id: number, public caption: string){}

    public clone(): BaseModel {
        return new BaseModel(this.id, this.caption);  //Поле caption под вопросом, это поле не обязательно будет у всех моделей
    }

    public reset(): void {
        this.id = 0;
        this.caption = '';
    }
}