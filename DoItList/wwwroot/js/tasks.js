// tasks.js - manejo de autenticación y CRUD de tareas

const apiBase = '/api';

// Guardar y obtener token
function setToken(token) { localStorage.setItem('jwt', token); }
function getToken() { return localStorage.getItem('jwt'); }
function clearToken() { localStorage.removeItem('jwt'); }

// Cabeceras con JWT
function authHeaders() {
    const token = getToken();
    return { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` };
}

// Login
async function login(email, password) {
    const res = await fetch(`${apiBase}/auth/login`, {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    if (!res.ok) throw 'Login failed';
    const data = await res.json(); setToken(data.token); return data;
}

// Register
async function register(name, email, password) {
    const res = await fetch(`${apiBase}/auth/register`, {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, email, password })
    });
    if (!res.ok) throw 'Registration failed';
    return await res.json();
}

// Obtener tareas
async function fetchTasks(userId) {
    const res = await fetch(`${apiBase}/tasks?userId=${userId}`, { headers: authHeaders() });
    if (!res.ok) throw 'Could not load tasks';
    return await res.json();
}

// Crear tarea
async function createTask(task) {
    const res = await fetch(`${apiBase}/tasks`, {
        method: 'POST', headers: authHeaders(), body: JSON.stringify(task)
    });
    if (!res.ok) throw 'Create task failed';
    return await res.json();
}

// Actualizar tarea
async function updateTask(id, task) {
    const res = await fetch(`${apiBase}/tasks/${id}`, {
        method: 'PUT', headers: authHeaders(), body: JSON.stringify(task)
    });
    if (!res.ok) throw 'Update failed';
    return await res.json();
}

// Eliminar tarea
async function deleteTask(id) {
    const res = await fetch(`${apiBase}/tasks/${id}`, {
        method: 'DELETE', headers: authHeaders()
    });
    if (!res.ok) throw 'Delete failed';
}

// Logout
function logout() { clearToken(); window.location = 'login.html'; }
