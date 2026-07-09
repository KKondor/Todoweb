import { useState, useEffect } from 'react'
import { formatDate } from './utils/formatDate.js'
import './App.css'


const baseUrl = import.meta.env.VITE_API_URL;
function App() {
    const [items, setItems] = useState([]);

    useEffect(() => {
        async function getItems() {
            const response = await fetch(baseUrl.concat("todoitems/"));
            const data = await response.json();
            setItems(data);
        }
        getItems();
    }, []);
    return (
        <div className="App">
            <h1>Todo Items</h1>
            <ul>
                {items.map((item) => (
                    <li key={item.id}>
                        {item.name} - {item.description} - {item.isComplete ? "Complete" : "Incomplete"} Created at: {formatDate(item.createDate)} Due at: {formatDate(item.dueDate)}
                        </li>
                ))}
            </ul>
        </div>
    )
}

export default App
