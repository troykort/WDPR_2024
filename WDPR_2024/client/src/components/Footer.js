import React from 'react';
import { Link } from 'react-router-dom';
import './Footer.css';

const Footer = () => {
    return (
        <footer className="footer">
            <nav className="footer-nav">
                <Link to="/privacybeleid" className="footer-button">Privacybeleid</Link>
                <Link to="/faq" className="footer-button">Veelgestelde vragen (FAQ)</Link>
                <Link to="/algemene-voorwaarden" className="footer-button">Algemene voorwaarden</Link>
                <Link to="/support" className="footer-button">Support</Link>
            </nav>
            <div className="footer-info">
                <p>&copy; 2024 CarAndAll. All rights reserved.</p>
            </div>
        </footer>
    );
};

export default Footer;


