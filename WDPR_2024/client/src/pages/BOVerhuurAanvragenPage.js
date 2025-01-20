import React, { useEffect, useState } from "react";
import axios from "axios";

import "./BOVerhuurAanvragenPage.css";

const BOVerhuurAanvragenPage = () => {
    const [aanvragen, setAanvragen] = useState([]);
    const [afwijsReden, setAfwijsReden] = useState("");
    const [modalVisible, setModalVisible] = useState(false);
    const [selectedAanvraag, setSelectedAanvraag] = useState(null);

    useEffect(() => {
        fetchAanvragen();
    }, []);

    const fetchAanvragen = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await axios.get("http://localhost:5000/api/verhuur-aanvragen", {
                headers: { Authorization: `Bearer ${token}` },
            });
            setAanvragen(response.data || []);
        } catch (error) {
            console.error("Error fetching verhuuraanvragen:", error);
            alert("Er is een fout opgetreden bij het ophalen van de verhuuraanvragen.");
        }
    };

    const handleGoedkeuren = async (id) => {
        try {
            const token = localStorage.getItem("token");
            const status = "Goedgekeurd";
            const opmerkingen = "aanvraag is goedgekeurd"
            const userID = localStorage.getItem("Id");
            await axios.put(
                `http://localhost:5000/api/verhuur-aanvragen/${id}/${status}`,
                { userID, opmerkingen },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );

            fetchAanvragen(); 
            alert("Aanvraag is goedgekeurd!");
        } catch (error) {
            console.error("Error approving aanvraag:", error);
            alert("Er is een fout opgetreden bij het goedkeuren van de aanvraag.");
        }
    };

    const handleAfwijzen = async () => {
        if (!afwijsReden.trim()) {
            alert("Voer een reden in voor afwijzing.");
            return;
        }
        console.log("Afwijsreden:", afwijsReden);
        try {
            const token = localStorage.getItem("token");
            const status = "Afgewezen";
            const userID = localStorage.getItem("Id");
            await axios.put(
                `http://localhost:5000/api/verhuur-aanvragen/${selectedAanvraag}/${status}`,
                { userID, opmerkingen: afwijsReden },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                       "Content-Type": "application/json",
                    },
                }
            );

            


            setAfwijsReden("");
            setModalVisible(false);
            fetchAanvragen(); // Refresh de lijst
            alert("Aanvraag is afgewezen!");
        } catch (error) {
            console.error("Error rejecting aanvraag:", error);
            alert("Er is een fout opgetreden bij het afwijzen van de aanvraag.");
        }
    };

    const openAfwijzenModal = (id) => {
        setSelectedAanvraag(id);
        setModalVisible(true);
    };

    const closeAfwijzenModal = () => {
        setAfwijsReden("");
        setModalVisible(false);
    };

    return (
        <div className="bo-verhuur-aanvragen-page-container">
            <h2>Backoffice Verhuur Aanvragen</h2>
            <table className="bo-verhuur-aanvragen-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Naam Huurder</th>
                        <th>Voertuig</th>
                        <th>Datum</th>
                        <th>Status</th>
                        <th>Acties</th>
                    </tr>
                </thead>
                <tbody>
                    {aanvragen.map((aanvraag) => (
                        <tr key={aanvraag.verhuurAanvraagID}>
                            <td>{aanvraag.verhuurAanvraagID}</td>
                            <td>{aanvraag.klantNaam || "Onbekend"}</td>
                            <td>{aanvraag.voertuigInfo || "Onbekend voertuig"}</td>
                            <td>{`${aanvraag.startDatum} - ${aanvraag.eindDatum}` || "Geen datum beschikbaar"}</td>
                            <td>{aanvraag.status}</td>
                            <td>
                                <button
                                    className="bo-verhuur-aanvragen-button bo-verhuur-aanvragen-button-approve"
                                    onClick={() => handleGoedkeuren(aanvraag.verhuurAanvraagID)}
                                >
                                    Goedkeuren
                                </button>
                                <button
                                    className="bo-verhuur-aanvragen-button bo-verhuur-aanvragen-button-reject"
                                    onClick={() => openAfwijzenModal(aanvraag.verhuurAanvraagID)}
                                >
                                    Afwijzen
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="bo-verhuur-aanvragen-modal">
                    <div className="bo-verhuur-aanvragen-modal-content">
                        <h3>Reden voor afwijzing</h3>
                        <textarea
                            className="bo-verhuur-aanvragen-textarea"
                            value={afwijsReden}
                            onChange={(e) => setAfwijsReden(e.target.value)}
                            placeholder="Voer een reden in"
                        ></textarea>
                        <div className="bo-verhuur-aanvragen-modal-actions">
                            <button onClick={handleAfwijzen}>Afwijzen</button>
                            <button onClick={closeAfwijzenModal}>Annuleren</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default BOVerhuurAanvragenPage;
