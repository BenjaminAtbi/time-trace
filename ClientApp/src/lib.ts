
export default function addDays(date: Date, days: number) {
    var genDate = new Date(date.valueOf());
    genDate.setDate(date.getDate() + days);
    return genDate;
}
