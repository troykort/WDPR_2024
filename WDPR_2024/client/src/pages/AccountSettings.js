import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './AccountSettings.css';

function AccountSettings() {
    const [originalData, setOriginalData] = useState({
        name: '',
        address: '',
        phone: '',
        email: '',
    }); // Originele gegevens
    const [userData, setUserData] = useState({
        name: '',
        address: '',
        phone: '',
        email: '',
    }); // Bewerking door gebruiker
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                // Simuleer een API-call of gebruik een echte API
                const response = await fetch('http://localhost:5000/api/klanten/details', {
                    method: 'GET',
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('token')}`,
                    },
                });

                if (!response.ok) {
                    throw new Error('Kon gebruikersinformatie niet ophalen');
                }

                const data = await response.json();
                setOriginalData(data); // Stel originele gegevens in
                setUserData(data); // Stel invoervelden in met dezelfde gegevens
                setLoading(false);
            } catch (error) {
                console.error('Error fetching user data:', error);
                alert('Fout bij het ophalen van gebruikersinformatie.');
                navigate('/'); // Redirect naar home of login
            }
        };

        fetchUserData();
    }, [navigate]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setUserData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleSaveChanges = async () => {
        try {
            const response = await fetch('http://localhost:5000/api/user/update', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('token')}`,
                },
                body: JSON.stringify(userData),
            });

            if (!response.ok) {
                throw new Error('Fout bij het opslaan van wijzigingen');
            }

            alert('Wijzigingen succesvol opgeslagen!');
            setOriginalData(userData); // Update originele gegevens na succesvolle opslag
        } catch (error) {
            console.error('Error saving changes:', error);
            alert('Fout bij het opslaan van wijzigingen.');
        }
    };

    if (loading) {
        return <p>Gegevens laden...</p>;
    }

    return (
        <div className="account-settings-page">
            <div className="account-settings-container">
                <h1>Account Instellingen</h1>
                <form>
                    <div className="form-group">
                        <label htmlFor="name">Naam:</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={userData.name}
                            onChange={handleInputChange}
                            placeholder="Voer uw naam in"
                        />
                        <span className="info">Huidig: {originalData.name}</span>
                    </div>
                    <div className="form-group">
                        <label htmlFor="address">Adres:</label>
                        <input
                            type="text"
                            id="address"
                            name="address"
                            value={userData.address}
                            onChange={handleInputChange}
                            placeholder="Voer uw adres in"
                        />
                        <span className="info">Huidig: {originalData.address}</span>
                    </div>
                    <div className="form-group">
                        <label htmlFor="phone">Telefoonnummer:</label>
                        <input
                            type="tel"
                            id="phone"
                            name="phone"
                            value={userData.phone}
                            onChange={handleInputChange}
                            placeholder="Voer uw telefoonnummer in"
                        />
                        <span className="info">Huidig: {originalData.phone}</span>
                    </div>
                    <div className="form-group">
                        <label htmlFor="email">E-mailadres:</label>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={userData.email}
                            onChange={handleInputChange}
                            placeholder="Voer uw e-mailadres in"
                        />
                        <span className="info">Huidig: {originalData.email}</span>
                    </div>
                    <div className="button-group">
                        <button type="button" onClick={handleSaveChanges} className="btn save-btn">
                            Wijzigingen Opslaan
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default AccountSettings;
