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
                <Link to="/register" className="navbar-button">Sign Up</Link>
                <Link to="/login" className="navbar-button">Login</Link>
            </div>
        </nav>
    );
};

export default NavBar;