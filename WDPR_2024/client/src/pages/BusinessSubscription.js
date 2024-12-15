import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './BusinessSubscription.css';

const BusinessSubscription = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { formData } = location.state || {};

    const handleSubmit = async (subscriptionType) => {
        try {
            const response = await fetch('http://localhost:5000/api/auth/register/zakelijk', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ ...formData, subscriptionType }),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                alert(`Registratie mislukt: ${errorMessage}`);
                return;
            }

            const data = await response.json();
            alert(`Registratie succesvol. Welkom, ${data.bedrijfsnaam}`);
            navigate('/login'); // Navigate to login page after successful registration
        } catch (error) {
            alert('Er is een fout opgetreden tijdens de registratie: ' + error.message);
        }
    };

    const handleGoBack = () => {
        navigate('/business-register', { state: { formData } });
    };

    return (
        <div className="subscription-page">
            <h2>Abonnementen</h2>
            <p>Kies uw abonnement hier.</p>
            <div className="subscription-boxes">
                <div className="subscription-box prepaid">
                    <h3 style={{ marginBottom: '5px' }}>Prepaid</h3>
                    <p style={{ marginTop: '5px' }}>Kies voor een prepaid abonnement en betaal vooraf voor uw diensten.</p>
                    <div className="button-group">
                        <button type="button" onClick={() => handleSubmit('prepaid')}>Kies Prepaid</button>
                    </div>
                </div>
                <div className="subscription-box pay-as-you-go">
                    <h3 style={{ marginBottom: '5px' }}>Pay-as-you-go</h3>
                    <p style={{ marginTop: '5px' }}>Kies voor een pay-as-you-go abonnement en betaal alleen voor wat u gebruikt.</p>
                    <div className="button-group">
                        <button type="button" onClick={() => handleSubmit('pay-as-you-go')}>Kies Pay-as-you-go</button>
                    </div>
                </div>
            </div>
            <div className="button-container">
                <button type="button" onClick={handleGoBack}>Terug naar bedrijfsregistratie</button>
            </div>
        </div>
    );
};

export default BusinessSubscription;
