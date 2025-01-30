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
    const navigate = useNavigate();

    useEffect(() => {
        const fetchNotifications = async () => {
            try {
                setLoading(true);
                setError(null);
                const token = localStorage.getItem("token");
                const response = await axios.get(
                    `http://localhost:5000/api/notificaties/${Id}`,
                    { headers: { Authorization: `Bearer ${token}` } }
                );
                setNotifications(response.data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchNotifications();
    }, [Id]);

    const toggleDropdown = () => {
        setDropdownVisible(!isDropdownVisible);
    };

    const handleNotificationClick = (notification) => {
       
        if (!notification.gelezen) {
            markAsRead(notification.notificatieID);
        }
      
        navigate(`/notificaties`);
    };

    const markAsRead = async (id) => {
        try {
            const token = localStorage.getItem("token");
            await axios.put(
                `http://localhost:5000/api/notificaties/${id}/gelezen`,
                {},
                { headers: { Authorization: `Bearer ${token}` } }
            );
           
            setNotifications((prev) =>
                prev.map((n) =>
                    n.notificatieID === id ? { ...n, gelezen: true } : n
                )
            );
        } catch (err) {
            console.error("Error marking notification as read:", err);
        }
    };

    const navNotifpagina = () => {
        navigate("/notificaties");
    };

    return (
        <div className="notification-container">
            <button
                className="notification-button"
                onClick={toggleDropdown}
                aria-label="Notifications"
                aria-expanded={isDropdownVisible}
            >
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
                        <p>Error: {error}</p>
                    ) : notifications.length > 0 ? (
                        <>
                            <ul>
                                {notifications.slice(0, 5).map((notification) => (
                                    <li
                                        key={notification.notificatieID}
                                        className={`notification-item ${notification.gelezen ? "gelezen" : "ongelezen"}`}
                                        onClick={() => handleNotificationClick(notification)}
                                    >
                                        <div className="notification-title">
                                            {notification.titel || "New notification"}
                                        </div>
                                        <div className="notification-message">
                                            {notification.bericht.substring(0, 50)}...
                                        </div>
                                        <div className="notification-time">
                                            {new Date(notification.verzondenOp).toLocaleString()}
                                        </div>
                                    </li>
                                ))}
                            </ul>
                            {notifications.length > 5 && (
                                <button className="view-all-button" onClick={navNotifpagina}>
                                    View all notifications
                                </button>
                            )}
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