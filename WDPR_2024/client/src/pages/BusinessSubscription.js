import React, { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './BusinessSubscription.css';

const BusinessSubscription = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { formData } = location.state || {};
    const [maandelijkseAbonnementskosten] = useState({ type: '', monthlyCost: 5000, discount: 20 });
    const [abonnementType, setAbonnementType] = useState({ type: '', days: 0, cost: 0 });
    const [selectedPrepaid, setSelectedPrepaid] = useState(false);

    const handleSubmit = async (type) => {
        if (type === 'prepaid' && !selectedPrepaid) {
            alert('Selecteer een van de prepaid opties.');
            return;
        } 
        const updatedFormData = {
            ...formData,
            abonnementType: type,
            aantalHuurdagenPerJaar: type === 'prepaid' ? abonnementType.days : undefined,
            kostenPerJaar: type === 'prepaid' ? abonnementType.cost : undefined,
            overgebruikKostenPerDag: type === 'prepaid' ? 70 : undefined,
            maandelijkseAbonnementskosten: type === 'pay-as-you-go' ? maandelijkseAbonnementskosten.monthlyCost : undefined,
            kortingOpVoertuig: type === 'pay-as-you-go' ? maandelijkseAbonnementskosten.discount : undefined,
        };
        console.log(updatedFormData)
        try {
            const response = await fetch('http://localhost:5000/api/auth/register/zakelijk', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updatedFormData),
            });

            if (!response.ok) {
                const errorMessage = await response.text();
                alert(`Registratie mislukt: ${errorMessage}`);
                return;
            }

            const data = await response.json();
            alert(`Registratie succesvol. Welkom, ${data.bedrijfsnaam}`);
            navigate('/login'); 
        } catch (error) {
            alert('Er is een fout opgetreden tijdens de registratie: ' + error.message);
        }
    };

    const handleGoBack = () => {
        navigate('/business-register', { state: { formData } });
    };

    const handleSubscriptionChange = (type, days, cost) => {
        setAbonnementType({ type, days, cost });
        setSelectedPrepaid(true);
    };


    const handlePayAsYouGoSubmit = () => {
        
        handleSubmit('pay-as-you-go');
    };

    const textStyle = { marginBottom: '10px' };

    return (
        <div className="subscription-page">
            <h2>Abonnementen</h2>
            <p>Kies uw abonnement hier.</p>
            <div className="subscription-boxes">
                <div className="subscription-box prepaid">
                    <h3 style={{ marginBottom: '5px' }}>Prepaid</h3>
                    <p style={{ ...textStyle, marginTop: '5px' }}>Betaal vooraf voor een bepaald aantal huurdagen per jaar.</p>
                    <p style={textStyle}><strong>Kosten</strong></p>
                    <div style={textStyle}>
                        <label>
                            <input type="radio" name="subscription" value="30-dagen" onChange={() => handleSubscriptionChange('prepaid', 30, 1500)} />
                            30 dagen/jaar: &euro;1.500
                        </label>
                    </div>
                    <div style={textStyle}>
                        <label>
                            <input type="radio" name="subscription" value="60-dagen" onChange={() => handleSubscriptionChange('prepaid', 60, 2800)} />
                            60 dagen/jaar: &euro;2.800
                        </label>
                    </div>
                    <div style={textStyle}>
                        <label>
                            <input type="radio" name="subscription" value="100-dagen" onChange={() => handleSubscriptionChange('prepaid', 100, 4500)} />
                            100 dagen/jaar: &euro;4.500
                        </label>
                    </div>
                    <p style={textStyle}><strong>Overgebruik</strong></p>
                    <div style={textStyle}>&#10004; Extra dagen worden gefactureerd tegen een standaardtarief tegenover &euro;70 per dag.</div>
                    <p style={textStyle}><strong>Voordelen</strong></p>
                    <div style={textStyle}>&#10004; Kostenbesparing bij regelmatig gebruik.</div>
                    <div style={textStyle}>&#10004; Ideaal voor bedrijven met voorspelbare huurbehoeften.</div>
                    <div className="button-group">
                        <button type="button" onClick={() => handleSubmit('prepaid')}>Kies Prepaid</button>
                    </div>
                </div>
                <div className="subscription-box pay-as-you-go">
                    <h3 style={{ marginBottom: '5px' }}>Pay-as-you-go</h3>
                    <p style={{ ...textStyle, marginTop: '5px' }}>Betaal een vast maandelijks bedrag voor toegang tot de dienst, met een korting op huurprijzen per voertuig.</p>
                    <p style={textStyle}><strong>Kosten</strong></p>
                    <div style={textStyle}>
                        <label>
                            &#10004; Maandelijkse kosten: &euro;5000
                        </label>
                    </div>
                    <div style={textStyle}>
                        <label>
                            &#10004; Korting op voertuighuur: 20%
                        </label>
                    </div>
                    <p style={textStyle}><strong>Voordelen</strong></p>
                    <div style={textStyle}>
                        &#10004; Flexibiliteit: geen vooraf bepaalde huurhoeveelheid.
                    </div>
                    <div style={textStyle}>
                        &#10004; Geschikt voor bedrijven met onregelmatige behoeften.
                    </div>
                    <div className="button-group">
                        <button type="button" onClick={() => handlePayAsYouGoSubmit()}>Kies Pay-as-you-go</button>
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
