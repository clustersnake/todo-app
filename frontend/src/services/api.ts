const BASE_URL = 'http://localhost:5091/api';
const API_KEY = 'supersecretkey123!'; 

export const apiFetch = async (endpoint: string, options: RequestInit = {}) => {
  const url = `${BASE_URL}${endpoint}`;

  const defaultHeaders = {
    'X-Api-Key': API_KEY, 
    'Content-Type': 'application/json',
  };

  const response = await fetch(url, {
    ...options,
    headers: {
      ...defaultHeaders,
      ...options.headers,
    },
  });

  if (!response.ok) {
    const errorBody = await response.text();
    throw new Error(errorBody || `Error: ${response.status}`);
  }

  // VALIDACIÓN CLAVE: Si el status es 204 o no hay contenido, no ejecutamos .json()
  if (response.status === 204 || response.headers.get("content-length") === "0") {
    return null; 
  }

  const jsonResponse = await response.json();

  return jsonResponse.data !== undefined ? jsonResponse.data : jsonResponse;
};