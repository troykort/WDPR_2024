import React, { useState, useEffect } from "react";
import axios from "axios";
import "./NotificationButton.css";
import bellIcon from "../assets/notificationbutton.png";

const NotificationButton = ({ Id }) => {
    const [notifications, setNotifications] = useState([]);
    const [isDropdownVisible, setDropdownVisible] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchNotifications = async () => {
            try {
                setLoading(true);
                setError(null);
                const response = await axios.get(
                    `http://localhost:5000/api/notificaties/user/${Id}`
                );
                setNotifications(response.data);
            } catch (err) {
                setError("Failed to fetch notifications.");
            } finally {
                setLoading(false);
            }
        };

        fetchNotifications();
    }, [Id]);

    const toggleDropdown = () => {
        setDropdownVisible(!isDropdownVisible);
    };

    const markAsRead = () => {
        setNotifications([]);
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
                                    <li key={index}>{notification.titel || "New notification"}</li>
                                ))}
                            </ul>
                            <button className="mark-read-button" onClick={markAsRead}>
                                Mark as Read
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
