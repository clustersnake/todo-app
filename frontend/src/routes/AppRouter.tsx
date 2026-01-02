import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { UserListView } from '../views/users/UserListView';
import { UserTasksView } from '../views/users/UserTasksView';

export const AppRouter = () => {
  return (
    <BrowserRouter>
      <Routes>
        {/* Ruta principal: Listado de usuarios */}
        <Route path="/" element={<UserListView />} />
        
        {/* Ruta dinámica: Tareas de un usuario específico */}
        <Route path="/user/:userId" element={<UserTasksView />} />
        
        {/* Opcional: Ruta para manejar errores 404 */}
        <Route path="*" element={<h1>404 - Página no encontrada</h1>} />
      </Routes>
    </BrowserRouter>
  );
};