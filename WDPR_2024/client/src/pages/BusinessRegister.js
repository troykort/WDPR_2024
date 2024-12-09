import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './BusinessRegister.css';

const BusinessRegister = () => {
const [formData, setFormData] = useState({
bedrijfsnaam: '',
        adres: '',
        kvknummer: '',
        emailDomein: '',
        contactEmail: '',
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
if (formData.password !== formData.confirmPassword)
{
    alert('Wachtwoorden komen niet overeen');
    return;
}

try
{
    const response = await fetch('http://localhost:5000/api/bedrijven/register', {
    method: 'POST',
                headers:
        {
            'Content-Type': 'application/json',
                },
                body: JSON.stringify({
bedrijfsnaam: formData.bedrijfsnaam,
                    adres: formData.adres,
                    kvknummer: formData.kvknummer,
                    emailDomein: formData.emailDomein,
                    contactEmail: formData.contactEmail,
                    password: formData.password
                }),
            });

if (!response.ok)
{
    const errorMessage = await response.text();
    alert(`Registratie mislukt: ${ errorMessage}`);
    return;
}

const data = await response.json();
alert(`Registratie succesvol.Welkom, ${ data.bedrijfsnaam}`);
navigate('/dashboard'); // Navigate to dashboard or another page
        } catch (error) {
    alert('Er is een fout opgetreden tijdens de registratie: ' + error.message);
}
    };

const handleGoBack = () => {
    navigate('/');
};

return (

    < div className = "register-page" >

        < form onSubmit ={ handleSubmit}>

            < h2 > Zakelijk Registreren </ h2 >

            < p > Registreren voor zakelijke klanten.</p>
                <div>
                    <label htmlFor="bedrijfsnaam">Bedrijfsnaam</label>
                    <input
                        type="text"
                        id="bedrijfsnaam"
                        name="bedrijfsnaam"
                        value={formData.bedrijfsnaam}
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
                    < label htmlFor = "kvknummer" > KVK-nummer </ label >
                    < input
                        type = "text"
                        id = "kvknummer"
                        name = "kvknummer"
                        value ={ formData.kvknummer}
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
placeholder = "Voer uw e-maildomein in"
                        required
                    />
                </ div >
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
                    < button type = "button" onClick ={ handleGoBack}> Terug </ button >
                    < button type = "submit" > Registreer </ button >
                </ div >
            </ form >
        </ div >
    );
};

export default BusinessRegister;
