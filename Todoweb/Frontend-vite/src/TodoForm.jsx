import {useState, useEffect} from 'react'
import {buildDueDate} from "./utils/buildDueDate.js";
import {splitDateTime} from "./utils/splitDateTime.js";


function TodoForm({onTodoCreated, onTodoUpdated, existingTodo}) {
    const [name, setName] = useState(existingTodo?.name ?? '');
    const [description, setDescription] = useState(existingTodo?.description ?? '');
    const [isComplete, setIsComplete] = useState(existingTodo?.isComplete ?? false);
    const [priority, setPriority] = useState(existingTodo?.todoPriority ?? '0');
    const { date: initialDate, time: initialTime } = splitDateTime(existingTodo?.dueDate) ?? {};
    const [dueDate, setDueDate] = useState(initialDate ?? '');
    const [dueTime, setDueTime] = useState(initialTime ?? '');

    useEffect(() => {
        setName(existingTodo?.name ?? '');
        setDescription(existingTodo?.description ?? '');
        setIsComplete(existingTodo?.isComplete ?? false);
        setPriority(existingTodo?.todoPriority ?? '0');
        setDueDate(initialDate ?? '');
        setDueTime(initialTime ?? '');
    }, [existingTodo]);

    async function handleSubmit(e) {
        e.preventDefault();
        const newTodo = {
            name,
            todoPriority: Number(priority),
            description,
            isComplete,
            dueDate: buildDueDate(dueDate,dueTime)
        };
        const url = existingTodo
            ? `${import.meta.env.VITE_API_URL}/todoitems/${existingTodo.id}`
            : `${import.meta.env.VITE_API_URL}/todoitems`;
        const method = existingTodo ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method,
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(newTodo),
        });

        if (!response.ok) {
            const problem = await response.json();
            console.log(problem);
            return
        }

        const result = await response.json();
        existingTodo ? onTodoUpdated(result) : onTodoCreated(result);
    }

    return (
        <form onSubmit={handleSubmit}>
            <label htmlFor="name-input">Name</label>
            <input id="name-input"
                value={name}
                onChange={e => setName(e.target.value)}
            />
            <label htmlFor="description-input">Description</label>
            <input id = "description-input"
            value={description}
            onChange={e => setDescription(e.target.value)}
            />
            <label htmlFor="completed-input">Is the task completed?</label>
            <input id="completed_input" type="checkbox" checked={isComplete} onChange={() => setIsComplete(!isComplete)} />
            <label htmlFor="priority-input">Task Priority</label>
            <select id="priority-input" value={priority} onChange={e => setPriority(e.target.value)}>
                <option value="0">Low Priority</option>
                <option value="1">Normal Priority</option>
                <option value="2">Urgent Priority</option>
            </select>
            <label htmlFor="date-input">Due Date and Time</label>
            <input id="date-input" type="date" value={dueDate} onChange={e => setDueDate(e.target.value)} />
            <input type="time" value={dueTime} onChange={e => setDueTime(e.target.value)} />
            <button type="submit">{existingTodo ? 'Update' : 'Create'}</button>
        </form>
    )
}

export default TodoForm;