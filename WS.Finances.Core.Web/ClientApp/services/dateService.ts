import * as moment from "moment";

export class DateService {
    public static getMonthName(monthNumber : number, shortMonthFormat?: boolean) {
        return moment([2000, monthNumber - 1]).format(shortMonthFormat ? "MMM" : "MMMM");
    }
}
