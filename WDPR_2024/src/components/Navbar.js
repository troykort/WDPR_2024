import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';
import logo from '../assets/Logo.png';

const NavBar = () => {
    return (
        <nav className="navbar">
            <div className="navbar-left">
                <Link to="/">
                    <img src={logo} alt="Logo" className="navbar-logo" />
                </Link>
            </div>
            <div className="navbar-right">
                <button className="navbar-button">Contact</button>
                <button className="navbar-button">Over ons</button>
                <button className="navbar-button">Voertuigen huren</button>
                <button className="navbar-button">Abonnementen</button>
                <button className="navbar-button">Profiel</button>
            </div>
        </nav>
    );
};

export default NavBar;