export class DateService {
    public ConvertDateToString(value: Date = null): string {
        if (value === null) value = new Date();
        let output = (value.getDate().toString().length == 1 ? "0" + value.getDate() : value.getDate()) + '.'
            + (((value.getMonth() + 1)).toString().length == 1 ? "0" + (value.getMonth() + 1) : (value.getMonth() + 1)) + '.'
            + value.getFullYear();
        return output;
    }

    public ConvertStringToDate(value: string=null){
        if (value===null) value=this.ConvertDateToString();
        return new Date(value);
    }
}