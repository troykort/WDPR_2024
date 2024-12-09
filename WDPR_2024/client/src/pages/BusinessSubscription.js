import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './BusinessSubscription.css';

const BusinessSubscription = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { formData } = location.state || {};

    const handleSubmit = async (e, subscriptionType) => {
        e.preventDefault();

        try {
            const response = await fetch('http://localhost:5000/api/bedrijven/register', {
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
            navigate('/dashboard'); // Navigate to dashboard or another page
        } catch (error) {
            alert('Er is een fout opgetreden tijdens de registratie: ' + error.message);
        }
    };

    const handleGoBack = () => {
        navigate('/business-register', { state: { formData } });
    };

    return (
        <div className="subscription-page">
            <h2>Zakelijke Abonnementen</h2>
            <p>Bekijk en beheer uw zakelijke abonnementen hier.</p>
            <div className="subscription-boxes">
                <div className="subscription-box prepaid">
                    <h3>Prepaid</h3>
                    <p>Kies voor een prepaid abonnement en betaal vooraf voor uw diensten.</p>
                    
                </div>
                <div className="subscription-box pay-as-you-go">
                    <h3>Pay-as-you-go</h3>
                    <p>Kies voor een pay-as-you-go abonnement en betaal alleen voor wat u gebruikt.</p>
                    
                </div>
            </div>
            <div className="button-container">
                <button type="button" onClick={handleGoBack}>Terug naar Registratie</button>
            </div>
        </div>
    );
};

export default BusinessSubscription;


