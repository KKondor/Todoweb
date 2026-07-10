import { useState, useEffect } from 'react'
import { formatDate } from './utils/formatDate.js'
import TodoForm from "./TodoForm.jsx";
import './App.css'


const baseUrl = import.meta.env.VITE_API_URL;
function App() {
    const [items, setItems] = useState([]);
    const [editingTodo, setEditingTodo] = useState(null);
    const [searchName, setSearchName] = useState('');
    const [filterComplete, setFilterComplete] = useState(''); // '', 'true', 'false'
    const [filterPriority, setFilterPriority] = useState(''); // '', '0', '1', '2'
    const priorityLabels = ['Low Priority', 'Normal Priority', 'Urgent Priority'];

    async function getItems() {
        const params = new URLSearchParams();
        if (searchName) params.append("name", searchName);
        if (filterComplete) params.append("isComplete", filterComplete);
        if (filterPriority) params.append("priority", filterPriority);
        const url = `${baseUrl}/todoitems?${params.toString()}`;

        const response = await fetch(url);
        const data = await response.json();
        setItems(data);
    }
    useEffect(() => {
        const timerId = setTimeout(() => {
            getItems();
        }, 500);

        return () => clearTimeout(timerId);
    }, [searchName]);
    useEffect(() => {
        getItems();
    }, [filterComplete, filterPriority]);

    async function handleDelete(id) {
        const response = await fetch(`${import.meta.env.VITE_API_URL}/todoitems/${id}`, {
            method: 'DELETE',
        });

        if (!response.ok) {
            console.log('Delete failed');
            return;
        }

        setItems(items.filter(item => item.id !== id));
    }

    return (
        <div className="App">
            <h1>Todo Items</h1>
            <TodoForm
                existingTodo={editingTodo}
                onTodoCreated={(newTodo) => setItems([...items, newTodo])}
                onTodoUpdated={(updatedTodo) => {
                    setItems(items.map(item => item.id === updatedTodo.id ? updatedTodo : item));
                    setEditingTodo(null);
                }}
            />
            <div>
                <label htmlFor="nameFilter">Search Name</label>
                <input id="nameFilter" value={searchName} onChange={(e) => setSearchName(e.target.value)}/>
                <label htmlFor="isCompleteFilter">Filter Completed</label>
                <select id="isCompleteFilter" value={filterComplete} onChange={(e) => setFilterComplete(e.target.value)}>
                    <option value="">All</option>
                    <option value="true">Completed</option>
                    <option value="false">Not Completed</option>
                </select>
                <label htmlFor="priorityFilter">Filter Priority</label>
                <select id="priorityFilter" value={filterPriority} onChange={(e) =>  setFilterPriority(e.target.value)}>
                    <option value="">All Priorities</option>
                    <option value="0">Low Priority</option>
                    <option value="1">Normal Priority</option>
                    <option value="2">Urgent Priority</option>
                </select>
            </div>
            <ul>
                {items.map((item) => (
                    <li key={item.id}>
                        {item.name} - {item.description} - {item.isComplete ? "Complete" : "Incomplete"} - Priority: {priorityLabels[item.todoPriority]} | Created at: {formatDate(item.createDate)} Due at: {formatDate(item.dueDate)}
                        <button onClick={() => setEditingTodo(item)}>Edit</button>
                        <button onClick={() => handleDelete(item.id)}>Delete</button>
                    </li>
                ))}
            </ul>
        </div>
    )
}

export default App
