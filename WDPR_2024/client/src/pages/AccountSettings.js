import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './AccountSettings.css';

function AccountSettings() {
    const [originalData, setOriginalData] = useState({
        name: '',
        address: '',
        phone: '',
        email: '',
        password: '', // Added password field
    });

    const [userData, setUserData] = useState({
        name: '',
        address: '',
        phone: '',
        email: '',
        password: '',
    });

    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const token = localStorage.getItem('token');
                const klantId = localStorage.getItem('userId');
                const medewerkerId = localStorage.getItem('medewerkerId')
                const medewerkerIdn = Number(medewerkerId)
                console.log(klantId);
                console.log(medewerkerIdn);


                if (!token || !klantId) throw new Error('User not authenticated.');


                const response = await fetch(`http://localhost:5000/api/klanten/${klantId}`, {
                    method: 'GET',
                    headers: { Authorization: `Bearer ${token}` },
                });

                if (!response.ok) throw new Error('Failed to fetch user data.');

                const data = await response.json();
                console.log(data);
                setOriginalData({
                    name: data.naam,
                    address: data.adres,
                    phone: data.telefoonnummer,
                    email: data.email,
                    password: '****', // Masked password display
                    rol: data.rol,
                    userID: data.userID,
                });

                setUserData({
                    name: data.naam,
                    address: data.adres,
                    phone: data.telefoonnummer,
                    email: data.email,
                    password: '',
                });

                setLoading(false);
            } catch (error) {
                console.error('Error fetching user data:', error.message);
                console.log(error);
                alert('Error fetching user data.');
                //navigate('/');
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
            const token = localStorage.getItem('token');
            const klantId = localStorage.getItem('userId');


            const payload = {
                KlantID: parseInt(klantId), // Ensure it's an integer
                Naam: userData.name || originalData.name,
                Adres: userData.address || originalData.address,
                Telefoonnummer: userData.phone || originalData.phone,
                Email: userData.email || originalData.email,
                Wachtwoord: userData.password || originalData.password,
                Rol: originalData.rol,
                UserID: originalData.userID,
            };

            // Only include password if updated
            if (userData.password) {
                payload.Wachtwoord = userData.password;
            }

            console.log("Payload being sent:", payload); // Debugging step

            const response = await fetch(`http://localhost:5000/api/klanten/${klantId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(payload),
            });

            if (!response.ok) {
                const errorData = await response.text();
                throw new Error(`Failed to save changes: ${errorData}`);
            }

            alert('Wijzigingen succesvol opgeslagen!');
            setOriginalData({ ...userData, password: '****' }); // Reset masked password
        } catch (error) {
            console.error('Error saving changes:', error.message);
            alert(`Error saving changes: ${error.message}`);
        }
    };

    if (loading) return <p>Loading...</p>;

    return (
        <div className="account-settings-page">
            <div className="account-settings-container">
                <h1>Account Instellingen</h1>
                <form>
                    <div className="account-settings-form-group">
                        <label htmlFor="name">Naam:</label>
                        <input
                            type="text"
                            id="name"
                            name="name"
                            value={userData.name}
                            onChange={handleInputChange}
                            placeholder="Voer uw naam in"
                        />
                    </div>
                    <div className="account-settings-form-group">
                        <label htmlFor="address">Adres:</label>
                        <input
                            type="text"
                            id="address"
                            name="address"
                            value={userData.address}
                            onChange={handleInputChange}
                            placeholder="Voer uw adres in"
                        />
                    </div>
                    <div className="account-settings-form-group">
                        <label htmlFor="phone">Telefoonnummer:</label>
                        <input
                            type="tel"
                            id="phone"
                            name="phone"
                            value={userData.phone}
                            onChange={handleInputChange}
                            placeholder="Voer uw telefoonnummer in"
                        />
                    </div>
                    <div className="account-settings-form-group">
                        <label htmlFor="email">E-mailadres:</label>
                        <input
                            type="email"
                            id="email"
                            name="email"
                            value={userData.email}
                            onChange={handleInputChange}
                            placeholder="Voer uw e-mailadres in"
                        />
                    </div>
                    <div className="account-settings-form-group">
                        <label htmlFor="password">Wachtwoord:</label>
                        <input
                            type="password"
                            id="password"
                            name="password"
                            value={userData.password}
                            onChange={handleInputChange}
                            placeholder="Voer uw wachtwoord in"
                        />
                    </div>
                    <div className="account-settings-button-group">
                        <button type="button" onClick={handleSaveChanges} className="account-settings-btn save-btn">
                            Wijzigingen Opslaan
                        </button>
                    </div>
                </form>
            </div>

            <div className="account-settings-read-only-container">
                <h2>Huidige Gegevens</h2>
                <p><strong>Naam:</strong> {originalData.name}</p>
                <p><strong>Adres:</strong> {originalData.address}</p>
                <p><strong>Telefoonnummer:</strong> {originalData.phone}</p>
                <p><strong>E-mailadres:</strong> {originalData.email}</p>
                <p><strong>Wachtwoord:</strong> ****</p>
            </div>
        </div>
    );
}

export default AccountSettings;
