import { jwtDecode } from 'jwt-decode';

export const getRoleFromToken = () => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    const decoded = jwtDecode(token);
    return decoded.role; // Zorg dat "role" in de JWT wordt meegegeven
};

export const getUserIdFromToken = () => {
    const token = localStorage.getItem("token");
    if (!token) return null;

    const decoded = jwtDecode(token);
    return decoded.Id; // Zorg dat "Id" in de JWT wordt meegegeven
};
