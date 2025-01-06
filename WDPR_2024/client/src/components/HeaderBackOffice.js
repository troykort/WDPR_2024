import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import './HeaderBackOffice.css';
import logo from '../assets/Logo.png';

const HeaderBackOffice = () => {
    const navigate = useNavigate();

    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    return (
        <nav className="headerbackoffice">
            <div className="headerbackoffice-left">
                <Link to="/dashboardbackoffice">
                    <img src={logo} alt="Logo" className="headerbackoffice-logo" />
                </Link>
            </div>
            <h1>BackOffice Dashboard</h1>
            <div className="headerbackoffice-right">
                <Link to="/voertuigverhuur" className="headerbackoffice-button">Klantenbeheer</Link>
                <Link to="/voertuigverhuur" className="headerbackoffice-button">Schadebeheer</Link>
                <Link to="/voertuigverhuur" className="headerbackoffice-button">Voertuigbeheer</Link>
                <Link to="/verhuuraanvragenpage" className="headerbackoffice-button">Verhuurbeheer</Link>
                <Link to="/dashboardbackoffice" className="headerbackoffice-button">Home</Link>
                <Link to="/profiel" className="headerbackoffice-button">Profiel</Link>
                <button onClick={handleLogout} className="headerbackoffice-button">Uitloggen</button>
            </div>
        </nav>
    );
};

export default HeaderBackOffice;
