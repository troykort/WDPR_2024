﻿import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Register.css';

const Register = () => {
    const [formData, setFormData] = useState({
        naam: '',
        adres: '',
        telefoonnummer: '',
        email: '',
        password: '',
        confirmPassword: ''
    });

    const navigate = useNavigate();

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData({
            ...formData,
            [name]: value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (formData.password !== formData.confirmPassword) {
            alert('Wachtwoorden komen niet overeen');
            return;
        }

        try {
            const response = await fetch('http://localhost:5000/api/auth/register/particulier', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Naam: formData.naam,
                    Adres: formData.adres,
                    Telefoonnummer: formData.telefoonnummer,
                    Email: formData.email,
                    Password: formData.password
                }),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                alert(`Registratie mislukt: ${errorMessage}`);
                return;
            }

            const data = await response.json();
            alert(`Registratie succesvol.`);
            navigate('/login');
        } catch (error) {
            alert('Er is een fout opgetreden tijdens de registratie: ' + error.message);
            console.log(error);
        }
    };

    const handleGoBack = () => {
        navigate('/');
    };

    const togglePasswordVisibility = () => {
        const passwordInput = document.getElementById('password');
        const confirmPasswordInput = document.getElementById('confirmPassword');
        const icon = document.querySelector('.password-toggle-icon');
        if (passwordInput.type === 'password') {
            passwordInput.type = 'text';
            confirmPasswordInput.type = 'text';
            icon.textContent = '🙉';
        } else {
            passwordInput.type = 'password';
            confirmPasswordInput.type = 'password';
            icon.textContent = '🙈';
        }
    };

    return (
        <div className="register-page">
            <form onSubmit={handleSubmit} aria-labelledby="register-heading">
                <h2 id="register-heading">Registreer</h2>
                <p>Registreren voor particuliere & zakelijke klanten.</p>
                <div>
                    <label htmlFor="naam">Naam</label>
                    <input
                        type="text"
                        id="naam"
                        name="naam"
                        value={formData.naam}
                        onChange={handleChange}
                        placeholder="Voer uw naam in"
                        required
                    />
                </div>
                <div>
                    <label htmlFor="adres">Adres</label>
                    <input
                        type="text"
                        id="adres"
                        name="adres"
                        value={formData.adres}
                        onChange={handleChange}
                        placeholder="Voer uw adres in"
                        required
                    />
                </div>
                <div>
                    <label htmlFor="telefoonnummer">Telefoonnummer</label>
                    <input
                        type="tel"
                        id="telefoonnummer"
                        name="telefoonnummer"
                        value={formData.telefoonnummer}
                        onChange={handleChange}
                        placeholder="Voer uw telefoonnummer in"
                        required
                    />
                </div>
                <div>
                    <label htmlFor="email">E-mailadres</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        placeholder="Voer uw e-mailadres in"
                        required
                    />
                </div>
                <div>
                    <label htmlFor="password">Wachtwoord</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        placeholder="Voer uw wachtwoord in"
                        required
                    />
                    <span
                        className="password-toggle-icon"
                        onClick={togglePasswordVisibility}
                        aria-label="Toggle password visibility"
                        role="button"
                        tabIndex="0"
                        onKeyPress={(e) => { if (e.key === 'Enter') togglePasswordVisibility(); }}
                    >
                        🙈
                    </span>
                </div>
                <div>
                    <label htmlFor="confirmPassword">Herhaal Wachtwoord</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        placeholder="Herhaal uw wachtwoord"
                        required
                    />
                </div>
                <div className="button-group">
                    <button type="button" onClick={handleGoBack}>Terug</button>
                    <button type="submit">Registreer</button>
                </div>
                <Link to="/business-register" className="zakelijkregistreren-button" style={{ color: '#5E0639' }}>Bedrijfsabbonnement afsluiten</Link>
            </form>
        </div>
    );
};

export default Register;


