import { useState, useEffect } from 'react'
import { buildDueDate } from "./utils/buildDueDate.js";
import { splitDateTime } from "./utils/splitDateTime.js";
import './TodoForm.css';

function TodoForm({ onTodoCreated, onTodoUpdated, existingTodo }) {
    const [name, setName] = useState(existingTodo?.name ?? '');
    const [description, setDescription] = useState(existingTodo?.description ?? '');
    const [isComplete, setIsComplete] = useState(existingTodo?.isComplete ?? false);
    const [priority, setPriority] = useState(existingTodo?.todoPriority ?? '0');
    const [error, setError] = useState(null);
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
            dueDate: buildDueDate(dueDate, dueTime)
        };

        const url = existingTodo
            ? `${import.meta.env.VITE_API_URL}/todoitems/${existingTodo.id}`
            : `${import.meta.env.VITE_API_URL}/todoitems`;

        const method = existingTodo ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newTodo),
        });

        if (!response.ok) {
            const problem = await response.json();
            const firstErrorKey = Object.keys(problem.errors ?? {})[0];
            const firstErrorMessage = firstErrorKey
                ? problem.errors[firstErrorKey][0]
                : problem.title ?? "An unknown error occurred.";

            setError(firstErrorMessage);
            return;
        }

        const result = await response.json();
        existingTodo ? onTodoUpdated(result) : onTodoCreated(result);
    }

    return (

        <form className="todo-form" onSubmit={handleSubmit}>
            {error && (
                <div className="error-box">
                    {error}
                </div>
            )}
            <h2 className="form-title">{existingTodo ? "Edit Todo" : "Create Todo"}</h2>

            <div className="form-group">
                <label>Name</label>
                <input
                    value={name}
                    onChange={e => setName(e.target.value)}
                    placeholder="Task name..."
                />
            </div>

            <div className="form-group">
                <label>Description</label>
                <input
                    value={description}
                    onChange={e => setDescription(e.target.value)}
                    placeholder="Short description..."
                />
            </div>

            <div className="form-group checkbox-group">
                <label>Completed?</label>
                <input
                    type="checkbox"
                    checked={isComplete}
                    onChange={() => setIsComplete(!isComplete)}
                />
            </div>

            <div className="form-group">
                <label>Priority</label>
                <select value={priority} onChange={e => setPriority(e.target.value)}>
                    <option value="0">Low Priority</option>
                    <option value="1">Normal Priority</option>
                    <option value="2">Urgent Priority</option>
                </select>
            </div>

            <div className="form-group">
                <label>Due Date</label>
                <input
                    type="date"
                    value={dueDate}
                    onChange={e => setDueDate(e.target.value)}
                />
            </div>

            <div className="form-group">
                <label>Due Time</label>
                <input
                    type="time"
                    value={dueTime}
                    onChange={e => setDueTime(e.target.value)}
                />
            </div>

            <button className="submit-btn" type="submit">
                {existingTodo ? "Update" : "Create"}
            </button>
        </form>
    );
}

export default TodoForm;