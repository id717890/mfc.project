export class CustomerType {
    constructor(public id: number, public caption: string){}
    public clone() :CustomerType {
        return new CustomerType(this.id, this.caption);
    }
}