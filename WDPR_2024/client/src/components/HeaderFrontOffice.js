import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './HeaderFrontOffice.css';
import logo from '../assets/Logo.png';

const HeaderFrontOffice = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    return (
        <nav className="headerfrontoffice">
            <div className="headerfrontoffice-left">
                <Link to="frontoffice-dashboard">
                    <img src={logo} alt="Logo" className="headerfrontoffice-logo" />
                </Link>
            </div>
            <h1>FrontOffice Dashboard</h1>
            <div className="headerfrontoffice-right">
                <Link to="/voertuigverhuur" className="headerfrontoffice-button">Klantenbeheer</Link>
                <Link to="/voertuigverhuur" className="headerfrontoffice-button">Voertuigbeheer</Link>
                <Link to="/frontoffice-verhuuraanvragen" className="headerfrontoffice-button">Verhuuraanvragen</Link>
                <Link to="/frontoffice-dashboard" className="headerfrontoffice-button">Home</Link>
                <Link to="/profiel" className="headerfrontoffice-button">Profiel</Link>
                <button onClick={handleLogout} className="headerfrontoffice-button">Uitloggen</button>
            </div>
        </nav>
    );
};

export default HeaderFrontOffice;
