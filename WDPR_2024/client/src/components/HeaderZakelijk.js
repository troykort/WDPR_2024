import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './HeaderZakelijk.css';
import logo from '../assets/Logo.png';
import Notification from './notificationbutton.js';

const HeaderZakelijk = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    return (
        <nav className="headerzakelijk">
            <div className="headerzakelijk-left">
                <Link to="/dashboardzakelijk">
                    <img src={logo} alt="Logo" className="headerzakelijk-logo" />
                </Link>
            </div>
            <h1>Zakelijk Dashboard</h1>
            <div className="headerzakelijk-right">
                <Link to="/voertuigverhuurZakelijk" className="headerzakelijk-button">Voertuigen huren</Link>
                <Link to="/profiel" className="headerzakelijk-button">Profiel</Link>
                <Link to="/rental-history" className="headerzakelijk-button">Verhuurgeschiedenis</Link>
                <Link to="/dashboardzakelijk" className="headerzakelijk-button">Home</Link>
                <button onClick={handleLogout} className="headerzakelijk-button">Uitloggen</button>
                <Notification Id={localStorage.getItem('Id')} />
            </div>
        </nav>
    );
};

export default HeaderZakelijk;
