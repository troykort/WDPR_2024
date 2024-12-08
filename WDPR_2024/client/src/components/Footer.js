import React from 'react';
import { Link } from 'react-router-dom';
import './Footer.css';

const Footer = () => {
    return (
        <footer className="footer">
            <nav className="footer-nav">
                <Link to="/contact" className="footer-button">Privacybeleid</Link>
                <Link to="/about" className="footer-button">Veelgestelde vragen (FAQ)</Link>
                <Link to="/voertuigen" className="footer-button">Algemene voorwaarden</Link>
                <Link to="/abonnementen" className="footer-button">Support</Link>
            </nav>
            <div className="footer-info">
                <p>&copy; 2024 CarAndAll. All rights reserved.</p>
            </div>
        </footer>
    );
};

export default Footer;