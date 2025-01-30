import React from "react";
import "./DashboardFrontoffice.css";

const DashboardFrontoffice = () => {
    const navigateToPage = (path) => {
        window.location.href = path;
    };

    return (
        <div className="frontoffice-dashboard-container">
            <h1>Frontoffice Dashboard</h1>
            <div className="frontoffice-dashboard-navigation">
                <button onClick={() => navigateToPage("/FO-verhuuraanvragen")} aria-label="Ga naar Verhuuraanvragen pagina">Verhuuraanvragen</button>
                <button onClick={() => navigateToPage("/voertuiginname")} aria-label="Ga naar Voertuigbeheer pagina">Voertuigbeheer</button>
            </div>
        </div>
    );
};

export default DashboardFrontoffice;




