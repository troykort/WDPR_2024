import React, { useState, useEffect } from "react";
import axios from "axios";

import "./Notificatiepagina.css";

const NotificatiePagina = () => {
    const [notificaties, setNotificaties] = useState([]);
    const [selectedNotificatie, setSelectedNotificatie] = useState(null);
    const [modalVisible, setModalVisible] = useState(false);

    useEffect(() => {
        fetchNotificaties();
    }, []);

    const fetchNotificaties = async () => {
        try {
            const token = localStorage.getItem("token");
            const userid = localStorage.getItem('Id');
            const response = await axios.get(`http://localhost:5000/api/notificaties/${userid}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            setNotificaties(response.data);
            console.log("Notificaties fetched:", response.data);
            console.log("")
        } catch (error) {
            console.error("Error fetching notificaties:", error);
        }
    };

    const markAsRead = async (id) => {
        try {
            const token = localStorage.getItem("token");
            await axios.put(`http://localhost:5000/api/notificaties/${id}/gelezen`, {}, {
                headers: { Authorization: `Bearer ${token}` },
            });
            fetchNotificaties(); 
        } catch (error) {
            console.error("Error marking notificatie as read:", error);
        }
    };

    const handleNotificatieClick = (notificatie) => {
        setSelectedNotificatie(notificatie);
        setModalVisible(true);
        if (!notificatie.gelezen) {
            markAsRead(notificatie.notificatieID);
        }
    };

    const handleCloseModal = () => {
        setSelectedNotificatie(null);
        setModalVisible(false);
    };

    return (
        <div className="notificatie-pagina-container">
            <h2>Notificaties</h2>
            <ul className="notificatie-lijst">
                {notificaties.map((notificatie) => (
                    <li
                        key={notificatie.notificatieID}
                        className={`notificatie-item ${notificatie.gelezen ? "gelezen" : "ongelezen"}`}
                        onClick={() => handleNotificatieClick(notificatie)}
                    >
                        <h3>{notificatie.titel}</h3>
                        <p>{notificatie.bericht.substring(0, 50)}...</p>
                        <span>{new Date(notificatie.verzondenOp).toLocaleString()}</span>
                    </li>
                ))}
            </ul>

            {modalVisible && selectedNotificatie && (
                <div className="notificatie-modal">
                    <div className="notificatie-modal-content">
                        <h3>{selectedNotificatie.titel}</h3>
                        <p>{selectedNotificatie.bericht}</p>
                        <p>
                            <strong>Verzonden op:</strong>{" "}
                            {new Date(selectedNotificatie.verzondenOp).toLocaleString()}
                        </p>
                        <button onClick={handleCloseModal}>Sluiten</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default NotificatiePagina;
