import React from 'react';
import { Link } from 'react-router-dom';
import './HeaderABO.css';
import logo from '../assets/Logo.png';
import { useNavigate } from "react-router-dom";


const Header = () => {
    const navigate = useNavigate();
    
};

const handleLogout = () => {
    localStorage.removeItem('token');
    navigate('/');
};


const HeaderABO = () => {
    return (
        <nav className="headerabo">
            <div className="headerabo-left">
                <Link to="/dashboardabo">
                    <img src={logo} alt="Logo" className="headerabo-logo" />
                </Link>
            </div>
            <h1>Abbonnementbeheerder Dashboard</h1>
            <div className="headerabo-right">
                <Link to="/abonnement" className="headerabo-button">Abonnement</Link>
                <Link to="/medewerkers" className="headerabo-button">Medewerkers</Link>
                <Link to="/profiel" className="headerabo-button">Profiel</Link>
                <button onClick={handleLogout} className="headerbackoffice-button">Uitloggen</button>
            </div>
        </nav>
    );
};

export default HeaderABO;