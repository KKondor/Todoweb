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
        <div className="app-container">
            <h1 className="title">Todo Items</h1>

            <TodoForm
                existingTodo={editingTodo}
                onTodoCreated={(newTodo) => setItems([...items, newTodo])}
                onTodoUpdated={(updatedTodo) => {
                    setItems(items.map(item => item.id === updatedTodo.id ? updatedTodo : item));
                    setEditingTodo(null);
                }}
            />

            {/* Filters */}
            <div className="filters">
                <div className="filter-group">
                    <label>Search Name</label>
                    <input
                        value={searchName}
                        onChange={(e) => setSearchName(e.target.value)}
                        placeholder="Search by name..."
                    />
                </div>

                <div className="filter-group">
                    <label>Completed</label>
                    <select value={filterComplete} onChange={(e) => setFilterComplete(e.target.value)}>
                        <option value="">All</option>
                        <option value="true">Completed</option>
                        <option value="false">Not Completed</option>
                    </select>
                </div>

                <div className="filter-group">
                    <label>Priority</label>
                    <select value={filterPriority} onChange={(e) => setFilterPriority(e.target.value)}>
                        <option value="">All</option>
                        <option value="0">Low</option>
                        <option value="1">Normal</option>
                        <option value="2">Urgent</option>
                    </select>
                </div>
            </div>

            {/* Todo List */}
            <div className="todo-list">
                {items.map(item => (
                    <div key={item.id} className="todo-card">
                        <div className="todo-header">
                            <h3>{item.name}</h3>
                            <span className={`priority p-${item.todoPriority}`}>
                                {priorityLabels[item.todoPriority]}
                            </span>
                        </div>

                        <p className="description">{item.description}</p>

                        <p className="meta">
                            <strong>Status:</strong> {item.isComplete ? "Complete" : "Incomplete"}
                            <br />
                            <strong>Created:</strong> {formatDate(item.createDate)}
                            <br />
                            <strong>Due:</strong> {formatDate(item.dueDate)}
                        </p>

                        <div className="actions">
                            <button className="edit-btn" onClick={() => setEditingTodo(item)}>Edit</button>
                            <button className="delete-btn" onClick={() => handleDelete(item.id)}>Delete</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default App
