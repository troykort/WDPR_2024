import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

function Login() {
    // State for handling input fields
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = () => {
        alert(`Email: ${email}, Password: ${password}`);
    };

    const handleBack = () => {
        alert('Navigating Back...');
    };

    return (
        <div className="login-page">
            {/* Top Section with Logo */}
            <div className="top-bar">
                <img src="/images/logo.png" alt="logo" className="logo-img" />
            </div>

            {/* Login Section */}
            <div className="login-section">
                <div className="login-box">
                    <h1>Login Page</h1>
                    <div className="form-group">
                        <label htmlFor="email">Email Address</label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Enter your email"
                        />
                    </div>
                    <div className="form-group">
                        <label htmlFor="password">Password</label>
                        <input
                            type="password"
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Enter your password"
                        />
                    </div>
                    <div className="button-group">
                        <button onClick={handleBack} className="btn back-btn">
                            Back
                        </button>
                        <button onClick={handleLogin} className="btn login-btn">
                            Login
                        </button>
                    </div>
                </div>
            </div>

            {/* Footer */}
            <div className="footer">
                <button className="footer-btn">About Us</button>
                <button className="footer-btn">Privacy</button>
                <button className="footer-btn">FAQ</button>
            </div>
        </div>
    );
}

export default Login;