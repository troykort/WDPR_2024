import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './HeaderBackOffice.css';
import logo from '../assets/Logo.png';
import NotificationButton from './notificationbutton';

const HeaderBackOffice = () => {
    const navigate = useNavigate();
    const id = localStorage.getItem("medewerkerId");
    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    return (
        <nav className="headerbackoffice">
            <div className="headerbackoffice-left">
                <Link to="/backoffice-dashboard">
                    <img src={logo} alt="Logo" className="headerbackoffice-logo" />
                </Link>
            </div>
            <h1>BackOffice Dashboard</h1>
            <div className="headerbackoffice-right">
                <Link to="/voertuigverhuur" className="headerbackoffice-button">Klantenbeheer</Link>
                <Link to="/voertuigverhuur" className="headerbackoffice-button">Schadebeheer</Link>
                <Link to="/voertuigverhuur" className="headerbackoffice-button">Voertuigbeheer</Link>
                <Link to="/verhuuraanvragenpage" className="headerbackoffice-button">Verhuurbeheer</Link>
                <Link to="/backoffice-dashboard" className="headerbackoffice-button">Home</Link>
                <Link to="/profiel" className="headerbackoffice-button">Profiel</Link>
                <NotificationButton Id={id} />
                <button onClick={handleLogout} className="headerbackoffice-button">Uitloggen</button>
            </div>
        </nav>
    );
};

export default HeaderBackOffice;
