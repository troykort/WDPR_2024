import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

function Login() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [showPassword, setShowPassword] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/klanten/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ email, password }),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                alert(`Login failed: ${errorMessage}`);
                return;
            }

            const data = await response.json();
            localStorage.setItem('klantDto', JSON.stringify(data));

            alert(`Login successful. Welcome, ${data.name}`);
            navigate('/dashboard');
        } catch (error) {
            alert('An error occurred during login: ' + error.message);
        }
    };

    const handleBack = () => {
        navigate('/');
    };

    return (
        <div className="login-page">
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
                        <div className="password-container">
                            <input
                                type={showPassword ? 'text' : 'password'}
                                id="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                placeholder="Enter your password"
                            />
                            <button
                                type="button"
                                className="toggle-password-btn"
                                onClick={() => setShowPassword(!showPassword)}
                            >
                                {showPassword ? '🙉' : '🙈'}
                            </button>
                        </div>
                    </div>
                    <div className="button-group">
                        <button onClick={handleBack} className="btn back-btn">Back</button>
                        <button onClick={handleLogin} className="btn login-btn">Login</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Login;
