import React, { useEffect, useState } from 'react';
import './App.css';
import { createTask, deleteTask, getTasks, updateTask, type GetTask, type Pagination } from './actions/task';
import { getStates, type GetState } from './actions/state';

export default function App() {
  const [tasks, setTasks] = useState<GetTask[]>([]);
  const [states, setStates] = useState<GetState[]>([]);
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [dueDate, setDueDate] = useState('');
  const [stateId, setStateId] = useState(0);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [pagination, setPagination] = useState<Pagination | null>(null);

  const [showFormModal, setShowFormModal] = useState(false);

    async function fetchTasks(pageNumber = 1) {
      const response = await getTasks(pageNumber);
      
      console.log('Tasks fetched:', response);
      if (response.isSuccess && response.data) {
          const fetchedTasks = response.data.tasks
          const paginationData = response.data.metadata;
          console.log('Pagination data:', paginationData);
          setPagination(paginationData);
          setTasks(fetchedTasks);
      } else {
          console.error('Error fetching tasks:', response.message);
      }
    }

  useEffect(() => {
    async function exec() {
      await fetchTasks();
    }
    exec();
  }, []);

  useEffect(() => {
    async function fetchStates() {
      const response = await getStates();
      if (response.isSuccess && response.data) {
        setStates(response.data);
      } else {
        console.error('Error fetching states:', response.message);
      }
    }
    fetchStates();
  }, []);

  const addTask = async () => {
    if (!title.trim()) return;
    
    if (editingId) {
      await updateTask({
        Id: editingId,
        Title: title,
        Description: description,
        StateId: stateId,
        DueDate: new Date(dueDate),
      });
      setTasks(tasks.map(t => 
        t.Id === editingId ? { ...t, Title: title, Description: description } : t
      ));
      setEditingId(null);
    } else {
      await createTask({
        Title: title,
        Description: description,
        StateId: stateId,
        DueDate: new Date(dueDate)
      });
    }
    fetchTasks();
    setShowFormModal(false);
    setTitle('');
    setDescription('');
    setDueDate('');
    setStateId(0);
  };

  const removeTask = async (id: number) => {
    await deleteTask(id);
    setTasks(tasks.filter(t => t.Id !== id));
  };

  const editTask = (task: GetTask) => {
    setTitle(task.Title);
    setDescription(task.Description);
    setDueDate(task.DueDate.toString().split('T')[0]);
    setStateId(task.State.Id);
    setEditingId(task.Id);
    setShowFormModal(true);
  };

  return (
    <div className="container">
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <h1>Gestor de Tareas</h1>
        <button style={{ color: '#333' }} onClick={() => setShowFormModal(true)}>Agregar Tarea</button>
      </div>
      {
        showFormModal && 
          <div className="modal-backdrop">
            <div className="form-section">
              <input
                type="text"
                placeholder="Título de la tarea"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                style={{ display: 'block', width: '100%', padding: '10px', marginBottom: '10px', borderRadius: '4px', border: '1px solid #ddd' }}
              />
              <textarea
                placeholder="Descripción"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                style={{ display: 'block', resize: 'none', width: '100%', padding: '10px', marginBottom: '10px', borderRadius: '4px', border: '1px solid #ddd', minHeight: '80px' }}
              />
              <input
                type="date"
                placeholder="Fecha de vencimiento"
                value={dueDate}
                onChange={(e) => setDueDate(e.target.value)}
                style={{ display: 'block', width: '100%', padding: '10px', marginBottom: '10px', borderRadius: '4px', border: '1px solid #ddd' }}
              />
              <select value={stateId} onChange={(e) => setStateId(Number(e.target.value))}>
                <option value="0" hidden>Seleccione un estado</option>
                {states.map((state: GetState) => (
                  <option key={state.Id} value={state.Id}>{state.Name}</option>
                ))}
              </select>
              <section className='button-container'>
                <button 
                  onClick={addTask}
                  style={{ padding: '10px 20px', backgroundColor: '#007bff', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}
                >
                  {editingId ? 'Actualizar' : 'Crear Tarea'}
                </button>
                <button className="button button-secondary" onClick={() => setShowFormModal(false)}>Cerrar</button>
              </section>
            </div>
          </div>
      }

      <table style={{ width: '100%', borderCollapse: 'collapse', border: '1px solid #ddd' }}>
        <thead>
          <tr style={{ backgroundColor: '#007bff', color: 'white' }}>
            <th style={{ padding: '10px', textAlign: 'left' }}>Título</th>
            <th style={{ padding: '10px', textAlign: 'left' }}>Descripción</th>
            <th style={{ padding: '10px', textAlign: 'center' }}>Estado</th>
            <th style={{ padding: '10px', textAlign: 'center' }}>Acciones</th>
          </tr>
        </thead>
        <tbody>
          {tasks.map(task => (
            <tr key={task.Id} style={{ borderBottom: '1px solid #ddd' }}>
              <td style={{ padding: '10px' }}>{task.Title}</td>
              <td style={{ padding: '10px' }}>{task.Description}</td>
              <td style={{ padding: '10px', textAlign: 'left' }}>
                {task.State.Name}
              </td>
              <td style={{ padding: '10px', textAlign: 'center' }}>
                <button onClick={() => editTask(task)} style={{ marginRight: '5px', padding: '5px 10px', backgroundColor: '#17a2b8', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Editar</button>
                <button onClick={() => removeTask(task.Id)} style={{ padding: '5px 10px', backgroundColor: '#dc3545', color: 'white', border: 'none', borderRadius: '4px', cursor: 'pointer' }}>Eliminar</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {pagination && (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '10px', marginTop: '20px' }}>
          {
 //           pagination.hasPreviousPage &&
              <button 
                onClick={() => fetchTasks(pagination.CurrentPage - 1)}
                disabled={pagination.CurrentPage === 1}
                style={{ padding: '8px 16px', backgroundColor: pagination.CurrentPage === 1 ? '#ccc' : '#007bff', color: 'white', border: 'none', borderRadius: '4px', cursor: pagination.CurrentPage === 1 ? 'not-allowed' : 'pointer' }}
              >
                Anterior
              </button>
          }
          {
//            pagination.hasNextPage || pagination.hasPreviousPage ? (
              <span style={{ fontSize: '16px', fontWeight: 'bold' }}>
                Página {pagination.CurrentPage} de {pagination.TotalPages}
              </span>
 //           ) : null
          }
          {
//            pagination.hasNextPage &&
              <button 
                onClick={() => fetchTasks(pagination.CurrentPage + 1)}
                disabled={pagination.CurrentPage === pagination.TotalPages}
                style={{ padding: '8px 16px', backgroundColor: pagination.CurrentPage === pagination.TotalPages ? '#ccc' : '#007bff', color: 'white', border: 'none', borderRadius: '4px', cursor: pagination.CurrentPage === pagination.TotalPages ? 'not-allowed' : 'pointer' }}
              >
                Siguiente
              </button>
          }
        </div>
      )}
      {tasks.length === 0 && <p style={{ textAlign: 'center', color: '#999', marginTop: '20px' }}>No hay tareas. ¡Crea una nueva!</p>}
    </div>
  );
}