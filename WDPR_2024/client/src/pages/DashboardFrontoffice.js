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
                <button onClick={() => navigateToPage("/verhuuraanvragen-page")}>Verhuuraanvragen</button>
                <button onClick={() => navigateToPage("/voertuigbeheer-page")}>Voertuigbeheer</button>
            </div>
        </div>
    );
};

export default DashboardFrontoffice;
