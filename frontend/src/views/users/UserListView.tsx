import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { apiFetch } from "../../services/api";
import type { User } from "../../types";

export const UserListView = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Usamos el wrapper que creamos
    apiFetch("/Users")
      .then((data) => {
        setUsers(data);
        setLoading(false);
        console.info({ data });
      })
      .catch((err) => console.error("Fallo al cargar usuarios:", err));
  }, []);

  return (
    <div>
      <h1>Listado de Usuarios</h1>
      {loading ? <p>Cargando usuarios...</p> : null}
      <ul>
        {users.map((user) => (
          <li key={user.id}>
            {user.firstName} {user.lastName}
            <Link to={`/user/${user.id}`} style={{ marginLeft: "10px" }}>
              Ver Tareas
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
};
