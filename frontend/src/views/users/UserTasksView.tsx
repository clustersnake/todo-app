import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { apiFetch } from "../../services/api";
import type { Task } from "../../types";

export const UserTasksView = () => {
  const { userId } = useParams<{ userId: string }>();
  const [tasks, setTasks] = useState<Task[]>([]);
  const [loading, setLoading] = useState(true);

  const [newTask, setNewTask] = useState({
    title: "",
    description: "",
    priorityId: 1, // 'Low' por defecto según nuestro Seed [cite: 2026-01-02]
    dueDate: "",
    tags: "",
  });

  useEffect(() => {
    if (userId) {
      // Asumiendo que tu endpoint es /Tasks/User/{id} según lo que armamos en el backend
      apiFetch(`/Tasks/${userId}`)
        .then((data: Task[]) => {
          // Requisito: Tareas ordenadas por fecha de vencimiento
          const sortedTasks = data.sort(
            (a, b) =>
              new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime()
          );
          setTasks(sortedTasks);
          setLoading(false);
        })
        .catch((err) => {
          console.error("Error cargando tareas:", err);
          setLoading(false);
        });
    }
  }, [userId]);

  const toggleComplete = async (task: Task) => {
    try {
      const updatedTask = { ...task, completed: !task.completed };

      // Llamada al backend para actualizar
      await apiFetch(`/Tasks/${task.id}`, {
        method: "PUT",
        body: JSON.stringify(updatedTask),
      });

      // Actualizamos el estado local para que la UI cambie instantáneamente
      setTasks((prev) => prev.map((t) => (t.id === task.id ? updatedTask : t)));
    } catch (err) {
      alert("No se pudo actualizar la tarea" + err);
    }
  };

  const deleteTask = async (taskId: number) => {
    if (!window.confirm("¿Estás seguro de eliminar esta tarea?")) return;

    try {
      await apiFetch(`/Tasks/${taskId}`, {
        method: "DELETE",
      });

      // Actualizamos el estado local filtrando la tarea eliminada
      setTasks((prev) => prev.filter((t) => t.id !== taskId));
    } catch (err) {
      alert("Error al eliminar la tarea" + err);
    }
  };

  const handleCreateTask = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const taskToSend = {
        ...newTask,
        userId: Number(userId), // Viene de la URL [cite: 2025-12-30]
        dueDate: newTask.dueDate
          ? new Date(newTask.dueDate).toISOString()
          : null,
        completed: false,
      };

      const savedTask = await apiFetch("/Tasks", {
        method: "POST",
        body: JSON.stringify(taskToSend),
      });

      // Añadimos la nueva tarea a la lista y limpiamos el formulario
      setTasks((prev) =>
        [...prev, savedTask].sort(
          (a, b) =>
            new Date(a.dueDate).getTime() - new Date(b.dueDate).getTime()
        )
      );
      setNewTask({
        title: "",
        description: "",
        priorityId: 1,
        dueDate: "",
        tags: "",
      });
    } catch (err) {
      alert("Error al crear la tarea" + err);
    }
  };

  if (loading) return <p>Cargando tareas del usuario...</p>;

  return (
    <div>
      <Link to="/">← Volver a Usuarios</Link>
      <form
        onSubmit={handleCreateTask}
        style={{
          marginBottom: "20px",
          padding: "15px",
          background: "#f4f4f4",
        }}
      >
        <input
          placeholder="Título"
          value={newTask.title}
          onChange={(e) => setNewTask({ ...newTask, title: e.target.value })}
          required
        />
        <input
          type="date"
          value={newTask.dueDate}
          onChange={(e) => setNewTask({ ...newTask, dueDate: e.target.value })}
        />
        <select
          value={newTask.priorityId}
          onChange={(e) =>
            setNewTask({ ...newTask, priorityId: Number(e.target.value) })
          }
        >
          <option value={1}>Baja</option>
          <option value={2}>Media</option>
          <option value={3}>Alta</option>
        </select>
        <button type="submit">Agregar Tarea</button>
      </form>
      <h2>Lista de Tareas</h2>

      {tasks.length === 0 ? (
        <p>Este usuario no tiene tareas pendientes.</p>
      ) : (
        <div className="task-list">
          {tasks.map((task) => (
            <div
              key={task.id}
              className={`task-card ${task.completed ? "completed" : ""}`}
            >
              <h3>{task.title}</h3>
              <p>{task.description}</p>
              <small>
                Vence: {new Date(task.dueDate).toLocaleDateString()}
              </small>
              <div>
                <strong>Prioridad:</strong> {task.priorityId}
              </div>
              <div>
                <strong> Tags:</strong> {task.tags}
              </div>
              <p>Estado: {task.completed ? "✅ Completada" : "⏳ Pendiente"}</p>
              <hr />
              <button onClick={() => toggleComplete(task)}>
                {task.completed ? "Desmarcar" : "Marcar como completada"}
              </button>
              <hr />
              <button onClick={() => deleteTask(task.id!)}>
                Eliminar Tarea
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};
