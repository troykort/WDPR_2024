import React from 'react';
import { Navigate } from 'react-router-dom';
import { getRoleFromToken } from '../utils/authHelpers';

const ProtectedRoute = ({ allowedRoles, children }) => {
    const role = getRoleFromToken();

    if (!role) {
        // Gebruiker is niet ingelogd, stuur door naar de loginpagina
        return <Navigate to="/login" />;
    }

    if (!allowedRoles.includes(role)) {
        // Gebruiker heeft geen toegang, stuur door naar een foutpagina
        return <Navigate to="/unauthorized" />;
    }

    // Gebruiker heeft toegang
    return children;
};

export default ProtectedRoute;
