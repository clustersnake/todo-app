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

  const deleteTask = async (taskId: number | undefined) => {
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
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-2xl mx-auto">
        <header className="flex justify-between items-center mb-8">
          <h1 className="text-3xl font-bold text-gray-900">Mis Tareas</h1>
          <Link to="/" className="text-blue-600 hover:underline">
            ← Volver
          </Link>
        </header>
        <div className="bg-white p-6 rounded-xl shadow-sm mb-8 border border-gray-200">
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
              onChange={(e) =>
                setNewTask({ ...newTask, title: e.target.value })
              }
              required
            />
            <input
              type="date"
              value={newTask.dueDate}
              onChange={(e) =>
                setNewTask({ ...newTask, dueDate: e.target.value })
              }
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
        </div>
        <h2>Lista de Tareas</h2>
        <div className="space-y-4">
          {tasks.length === 0 ? (
            <p>Este usuario no tiene tareas pendientes.</p>
          ) : (
            <div className="task-list">
              {tasks.map((task) => (
                <div
                  key={task.id}
                  className="bg-white p-4 rounded-lg shadow-sm flex items-center justify-between border border-gray-100"
                >
                  <div className="flex items-center gap-4">
                    <input
                      type="checkbox"
                      checked={task.completed}
                      onChange={() => toggleComplete(task)}
                      className="h-5 w-5 text-blue-600"
                    />
                    <div>
                      <p
                        className={`font-medium ${
                          task.completed
                            ? "line-through text-gray-400"
                            : "text-gray-800"
                        }`}
                      >
                        {task.title}
                      </p>
                      <span className="text-xs bg-blue-100 text-blue-700 px-2 py-1 rounded">
                        Prioridad:{" "}
                        {task.priorityId === 3
                          ? "Alta"
                          : task.priorityId === 2
                          ? "Media"
                          : "Baja"}
                      </span>
                    </div>
                  </div>
                  <button
                    onClick={() => deleteTask(task.id)}
                    className="text-red-400 hover:text-red-600 p-2"
                  >
                    Eliminar
                  </button>
                </div>
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
};
