import React, { useEffect, useState } from "react";
import axios from "axios";

import "./BOVerhuurAanvragenPage.css";

const VerhuurAanvragenPage = () => {
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
            setAanvragen(response.data);
            console.log("Fetched aanvragen:", response.data);
        } catch (error) {
            console.error("Error fetching verhuuraanvragen:", error);
        }
    };

    const handleGoedkeuren = async (id) => {
        try {
            const token = localStorage.getItem("token");
            const status = "Goedgekeurd";

            // Log ID for debugging
            console.log("Goedkeuren ID:", id);

            await axios.put(`http://localhost:5000/api/verhuur-aanvragen/${id}/${status}`, 
                { opmerkingen: null },
                {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            fetchAanvragen(); // Refresh the list
            alert("Aanvraag is goedgekeurd!");
        } catch (error) {
            console.error("Error approving aanvraag:", error);
        }
    };

    const handleAfwijzen = async () => {
        if (!afwijsReden.trim()) {
            alert("Voer een reden in voor afwijzing.");
            return;
        }

        try {
            const token = localStorage.getItem("token");
            const status = "Afgewezen";

            console.log("Afwijzen ID:", selectedAanvraag);

            await axios.put(
                `http://localhost:5000/api/verhuur-aanvragen/${selectedAanvraag}/${status}`,
                { opmerkingen: afwijsReden },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            setAfwijsReden("");
            setModalVisible(false);
            fetchAanvragen(); // Refresh the list
            alert("Aanvraag is afgewezen!");
        } catch (error) {
            console.error("Error rejecting aanvraag:", error);
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
        <div className="verhuur-aanvragen-page-container">
            <h2>Verhuur Aanvragen</h2>
            <table className="verhuur-aanvragen-table">
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
                                    className="verhuur-aanvragen-button verhuur-aanvragen-button-approve"
                                    onClick={() => handleGoedkeuren(aanvraag.verhuurAanvraagID)}
                                >
                                    Goedkeuren
                                </button>
                                <button
                                    className="verhuur-aanvragen-button verhuur-aanvragen-button-reject"
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
                <div className="verhuur-aanvragen-modal">
                    <div className="verhuur-aanvragen-modal-content">
                        <h3>Reden voor afwijzing</h3>
                        <textarea
                            className="verhuur-aanvragen-textarea"
                            value={afwijsReden}
                            onChange={(e) => setAfwijsReden(e.target.value)}
                            placeholder="Voer een reden in"
                        ></textarea>
                        <div className="verhuur-aanvragen-modal-actions">
                            <button onClick={handleAfwijzen}>Afwijzen</button>
                            <button onClick={closeAfwijzenModal}>Annuleren</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default VerhuurAanvragenPage;
