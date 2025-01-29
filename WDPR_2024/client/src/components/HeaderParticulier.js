import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './HeaderParticulier.css';
import logo from '../assets/Logo.png';
import Notification from './notificationbutton.js';

const HeaderParticulier = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    return (
        <nav className="headerparticulier">
            <div className="headerparticulier-left">
                <Link to="/dashboardparticulier">
                    <img src={logo} alt="Logo" className="headerparticulier-logo" />
                </Link>
            </div>
            <h1>Particulier Dashboard</h1>
            <div className="headerparticulier-right">
                <Link to="/voertuigverhuur" className="headerparticulier-button">Voertuigen huren</Link>
                <Link to="/dashboardparticulier" className="headerparticulier-button">Home</Link>
                <Link to="/profiel" className="headerparticulier-button">Profiel</Link>
                <Link to="/rental-history" className="headerparticulier-button">Verhuurgeschiedenis</Link>
                <Notification Id={localStorage.getItem('Id')} />
                <button onClick={handleLogout} className="headerparticulier-button">Uitloggen</button>
            </div>
        </nav>
    );
};

export default HeaderParticulier;

