export function buildDueDate(date,time){
    if (!date && !time) return null;
    if (!date && time) return null;
    return `${date}T${time || '00:00'}`;
}