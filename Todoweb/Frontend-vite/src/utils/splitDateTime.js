export function splitDateTime(dateString) {
    if (!dateString) {
        const date=null;
        const time=null;
        return {date, time};
    }

    const originalDate = new Date(dateString);
    const date = String(originalDate.getFullYear())
        + '-' + (String(originalDate.getMonth() + 1)).padStart(2, '0') + '-' + String(originalDate.getDate()).padStart(2, '0');
    const time = String(originalDate.getHours()).padStart(2, '0') + ':' + String(originalDate.getMinutes()).padStart(2, '0');
    return {date, time};
}