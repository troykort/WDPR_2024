﻿import React, { useState } from 'react';
import { useNavigate, Outlet, useLocation } from 'react-router-dom';
import './BusinessRegister.css';

const BusinessRegister = () => {
const [formData, setFormData] = useState({
bedrijfsnaam: '',
        adres: '',
        kvkNummer: '',
        emailDomein: '',
        contactEmail: '',
        password: '',
        confirmPassword: ''
    });

const [error, setError] = useState('');

const navigate = useNavigate();
const location = useLocation();

const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
        ...formData,
            [name]: value
        });
setError(''); // Clear error on input change
    };
    
const handleGoToSubscriptions = () => {
    //if (!formData.emailDomein.startsWith('@'))
    //{
    //    setError('Het e-maildomein moet beginnen met een @.');
    //    return;
    //}

    navigate('/business-register/subscriptions', { state: { formData } });
};

const handleGoToRegister = () => {
    navigate('/register', { state: { formData } });
};

const togglePasswordVisibility = () => {
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const icon = document.querySelector('.password-toggle-icon');
    if (passwordInput.type === 'password')
    {
        passwordInput.type = 'text';
        confirmPasswordInput.type = 'text';
        icon.textContent = '🙉';
    }
    else
    {
        passwordInput.type = 'password';
        confirmPasswordInput.type = 'password';
        icon.textContent = '🙈';
    }
};

return (

    < div className = "register-page" >
            {
    location.pathname === '/business-register' && (
                < form >
                    < h2 > Bedrijfsregistratie </ h2 >
                    < p > Registreren als abbonnementsbeheerder voor bedrijven.</ p >
                    < div >
                        < label htmlFor = "bedrijfsnaam" > Bedrijfsnaam </ label >
                        < input
                            type = "text"
                            id = "bedrijfsnaam"
                            name = "bedrijfsnaam"
                            value ={ formData.bedrijfsnaam}
    onChange ={ handleChange}
    placeholder = "Voer uw bedrijfsnaam in"
                            required
                        />
                    </ div >
                    < div >
                        < label htmlFor = "adres" > Adres </ label >
                        < input
                            type = "text"
                            id = "adres"
                            name = "adres"
                            value ={ formData.adres}
    onChange ={ handleChange}
    placeholder = "Voer uw adres in"
                            required
                        />
                    </ div >
                    < div >
                        < label htmlFor = "kvkNummer" > KVK - nummer </ label >
                        < input
                            type = "text"
                            id = "kvkNummer"
                            name = "kvkNummer"
                            value ={ formData.kvkNummer}
    onChange ={ handleChange}
    placeholder = "Voer uw kvknummer in"
                            required
                        />
                    </ div >
                    < div >
                        < label htmlFor = "emailDomein" > E-maildomein </ label >
                        < input
                            type = "text"
                            id = "emailDomein"
                            name = "emailDomein"
                            value ={ formData.emailDomein}
    onChange ={ handleChange}
    placeholder = "Voer uw e-maildomein in (alles na de @)"
                            required
                        />
                    </ div >
                    { error && < p className = "error-message" >{ error}</ p >}
                    < div >
                        < label htmlFor = "contactEmail" > Contact e-mailadres </ label >
                        < input
                            type = "email"
                            id = "contactEmail"
                            name = "contactEmail"
                            value ={ formData.contactEmail}
    onChange ={ handleChange}
    placeholder = "Voer uw contact e-mailadres in"
                            required
                        />
                    </ div >
                    < div >
                        < label htmlFor = "password" > Wachtwoord </ label >
                        < input
                            type = "password"
                            id = "password"
                            name = "password"
                            value ={ formData.password}
    onChange ={ handleChange}
    placeholder = "Voer uw wachtwoord in"
                            required
                        />
                        < span
                            className = "password-toggle-icon"
                            onClick ={ togglePasswordVisibility}
                        >
                            🙈
                        </ span >
                    </ div >
                    < div >
                        < label htmlFor = "confirmPassword" > Herhaal Wachtwoord </ label >
                        < input
                            type = "password"
                            id = "confirmPassword"
                            name = "confirmPassword"
                            value ={ formData.confirmPassword}
    onChange ={ handleChange}
    placeholder = "Herhaal uw wachtwoord"
                            required
                        />
                    </ div >
                    < div className = "button-group" >
                        < button type = "button" onClick ={ handleGoToRegister}> Terug </ button >
                        < button type = "button" onClick ={ handleGoToSubscriptions}> Ga door </ button >
                    </ div >
                </ form >
            )}
            < Outlet />
        </ div >
    );
};

export default BusinessRegister;
