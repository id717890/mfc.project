export class AppSettings {
    public static get API_ENDPOINT(): string { return "http://localhost:4664/api/"; }
    //Кол-во элементов на странице
    public static get DEFAULT_PAGE_SIZE(): number { return 25; }

    //Настройки компонента MyDatePickerModule
    public static get DEFAULT_DATE_PICKER_OPTION(): any {
        return {
            todayBtnTxt: 'Today',
            dateFormat: 'dd.mm.yyyy',
            firstDayOfWeek: 'mo',
            inline: false,
        }
    }
}