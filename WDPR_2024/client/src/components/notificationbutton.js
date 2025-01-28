import React, { useState, useEffect } from "react";
import axios from "axios";
import "./NotificationButton.css";
import bellIcon from "../assets/notificationbutton.png";
import { useNavigate } from "react-router-dom";

const NotificationButton = ({ Id }) => {
    const [notifications, setNotifications] = useState([]);
    const [isDropdownVisible, setDropdownVisible] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const navigator = useNavigate();

    useEffect(() => {
        const fetchNotifications = async () => {
            try {
                setLoading(true);
                setError(null);
                const token = localStorage.getItem("token");
                const response = await axios.get(
                    `http://localhost:5000/api/notificaties/${Id}`
                    , { headers: { Authorization: `Bearer ${token}` } },

                );
                setNotifications(response.data);
                console.log(response.data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchNotifications();
    }, []);

    const toggleDropdown = () => {
        setDropdownVisible(!isDropdownVisible);
    };

    const navNotifpagina = () => {
        navigator("/notificaties");
    };

    return (
        <div className="notification-container">
            <button className="notification-button" onClick={toggleDropdown}>
                <img src={bellIcon} alt="Notifications" className="notification-icon" />
                {notifications.length > 0 && (
                    <span className="notification-badge">{notifications.length}</span>
                )}
            </button>
            {isDropdownVisible && (
                <div className="notification-dropdown">
                    {loading ? (
                        <p>Loading notifications...</p>
                    ) : error ? (
                        <p>{error}</p>
                    ) : notifications.length > 0 ? (
                        <>
                             <ul>
                                        {notifications.map((notification, index) => (
                                            <li key={index}>
                                                <div>{notification.titel || "New notification"}</div>

                                            </li>
                                        ))}
                              </ul>

                             <button className="mark-read-button" onClick={navNotifpagina}>
                               alle notificaties
                            </button>
                        </>
                    ) : (
                        <p>No new notifications</p>
                    )}
                </div>
            )}
        </div>
    );
};

export default NotificationButton;
