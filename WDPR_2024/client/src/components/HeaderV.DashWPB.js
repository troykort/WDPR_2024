import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./HeaderV.DashWPB.css";
import NotificationButton from "./notificationbutton";
import logo from "../assets/Logo.png";

const Header = () => {
    const [notificationsVisible, setNotificationsVisible] = useState(false);
    const navigate = useNavigate();
    const toggleNotifications = () => {
        setNotificationsVisible(!notificationsVisible);
    };

    return (
        <div className="header">
            <div className="logo">
                <img src={logo} alt="Logo" />
             
            </div>
            <h1>Wagenparkbeheerder Dashboard</h1>
            <div className="nav-buttons">
                <button onClick={() => navigate("/voertuigoverzicht")}>Voertuigoverzicht</button>
                <button onClick={() => navigate("/medewerkers")}>Medewerkers</button>
                <button onClick={() => navigate("/statistieken")}>Statistieken</button>
                <button onClick={() => navigate("/profiel")}>Profiel</button>
            </div>
            <NotificationButton />
        </div>
    );
};

export default Header;
