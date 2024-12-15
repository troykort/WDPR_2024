import React from 'react';
import { useNavigate } from 'react-router-dom';
import './DashboardWPB.css';

const DashboardWPB = () => {
    const navigate = useNavigate();

    const handleGoToHome = () => {
        navigate('/');
    };

    const handleGoToMedewerkers = () => {
        navigate('/medewerkers');
    };

    const handleGoToVoertuigoverzicht = () => {
        navigate('/voertuigoverzicht');
    };

    return (
        <div className="dashboardwpb-container">
            <h1>Dashboard WPB</h1>
            <p className="dashboard-description">
                Het dashboard voor wagenparkbeheer is een centraal punt waar wagenparkbeheerders eenvoudig en efficient het gehele wagenpark kunnen beheren. Dit dashboard biedt real-time informatie en functionaliteiten om processen makkelijker te laten verlopen.
            </p>
            <div className="button-wrapper">
                <button onClick={handleGoToHome} className="btn home-btn">
                    Terug naar Home
                </button>
                <button onClick={handleGoToMedewerkers} className="btn home-btn">
                    Medewerkers
                </button>
                <button onClick={handleGoToVoertuigoverzicht} className="btn home-btn">
                    Voertuigoverzicht
                </button>
            </div>
        </div>
    );
};

export default DashboardWPB;

