export function formatDate(dateString) {
    if (dateString === null) {
        return "N/A";
    }
    const date = new Date(dateString);
    return String(date.getFullYear())
        + '-' + (String(date.getMonth() + 1)).padStart(2, '0') + '-' + String(date.getDate()).padStart(2, '0')
        + ' ' + String(date.getHours()).padStart(2, '0') + ':' + String(date.getMinutes()).padStart(2, '0');
}
