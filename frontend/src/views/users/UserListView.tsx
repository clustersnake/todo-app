import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { apiFetch } from '../../services/api';
import type { User } from '../../types';

export const UserListView = () => {
  const [users, setUsers] = useState<User[]>([]);

  useEffect(() => {
    apiFetch('/Users').then(setUsers).catch(console.error);
  }, []);

  return (
    <div className="min-h-screen bg-gray-50 p-8">
      <div className="max-w-4xl mx-auto">
        <header className="mb-10 text-center">
          <h1 className="text-4xl font-extrabold text-gray-900 mb-2">Gestión de Tareas</h1>
          <p className="text-gray-600">Selecciona un usuario para gestionar sus pendientes</p>
        </header>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {users.map((user) => (
            <Link 
              key={user.id} 
              to={`/user/${user.id}`}
              className="group bg-white p-6 rounded-2xl shadow-sm border border-gray-100 hover:shadow-md hover:border-blue-300 transition-all duration-300 flex flex-col items-center text-center"
            >
              <div className="w-16 h-16 bg-blue-100 text-blue-600 rounded-full flex items-center justify-center text-2xl font-bold mb-4 group-hover:bg-blue-600 group-hover:text-white transition-colors">
                {user.firstName[0]}{user.lastName ? user.lastName[0] : ''}
              </div>
              <h2 className="text-xl font-semibold text-gray-800 mb-1">
                {user.firstName} {user.lastName}
              </h2>
              <p className="text-sm text-gray-500 mb-4">{user.email}</p>
              <span className="text-blue-500 font-medium text-sm group-hover:translate-x-1 transition-transform">
                Ver tareas →
              </span>
            </Link>
          ))}
        </div>

        {users.length === 0 && (
          <div className="text-center py-20">
            <p className="text-gray-400 italic">No hay usuarios registrados todavía.</p>
          </div>
        )}
      </div>
    </div>
  );
};