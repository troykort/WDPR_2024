import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

function Login() {
    // State for handling input fields
    const [Email, setEmail] = useState('');
    const [Password, setPassword] = useState('');
    const navigate = useNavigate();

    const handleLogin = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Username: Email,
                    Password
                }),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                alert(`Login failed: ${errorMessage}`);
                return;
            }

            const data = await response.json();
            alert(`Login successful. Welcome, ${data.name}`);
            navigate('/dashboard'); // Navigate to dashboard or another page
        } catch (error) {
            alert('An error occurred during login: ' + error.message);
        }
    };

    const handleBack = () => {
        navigate('/'); // Navigate to the home page
    };

    const togglePasswordVisibility = () => {
        const passwordInput = document.getElementById('password');
        const icon = document.querySelector('.password-toggle-icon');
        if (passwordInput.type === 'password') {
            passwordInput.type = 'text';
            icon.textContent = '🙉';
        } else {
            passwordInput.type = 'password';
            icon.textContent = '🙈';
        }
    };

    return (
        <div className="login-page">
            <div>
            </div>

            {/* Login Section */}
            <div className="login-section">
                <div className="login-box">
                    <h1>Login Page</h1>
                    <div className="form-group" style={{ display: 'flex', alignItems: 'center' }}>
                        <label htmlFor="email" style={{ color: '#5E0639', marginRight: '10px' }}>Email Address</label>
                        <input
                            type="email"
                            id="email"
                            value={Email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="Enter your email"
                            style={{ flex: 1 }}
                        />
                    </div>
                    <div className="form-group" style={{ display: 'flex', alignItems: 'center' }}>
                        <label htmlFor="password" style={{ color: '#5E0639', marginRight: '10px' }}>Password</label>
                        <input
                            type="password"
                            id="password"
                            value={Password}
                            onChange={(e) => setPassword(e.target.value)}
                            placeholder="Enter your password"
                            style={{ flex: 1 }}
                        />
                        <span
                            className="password-toggle-icon"
                            onClick={togglePasswordVisibility}
                            style={{ marginLeft: '10px', cursor: 'pointer' }}
                        >
                            🙈
                        </span>
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
            <div>
            </div>
        </div>
    );
}

export default Login;
