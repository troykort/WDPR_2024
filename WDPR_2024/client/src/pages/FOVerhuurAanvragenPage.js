import React, { useEffect, useState } from "react";
import axios from "axios";

import "./FOVerhuurAanvragenPage.css";

const FrontofficeVerhuurAanvragenPage = () => {
    const [aanvragen, setAanvragen] = useState([]);
    const [selectedAanvraag, setSelectedAanvraag] = useState(null);
    const [opmerkingen, setOpmerkingen] = useState("");
    const [modalVisible, setModalVisible] = useState(false);

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
        } catch (error) {
            console.error("Error fetching verhuuraanvragen:", error);
        }
    };

    const handleOpenModal = (aanvraag) => {
        setSelectedAanvraag(aanvraag);
        setOpmerkingen("");
        setModalVisible(true);
    };

    const handleCloseModal = () => {
        setSelectedAanvraag(null);
        setOpmerkingen("");
        setModalVisible(false);
    };

    const handleUpdateStatus = async () => {
        if (!opmerkingen.trim()) {
            alert("Voer opmerkingen in om de uitgifte te registreren.");
            return;
        }
        try {
            const token = localStorage.getItem("token");
            const status = "Uitgegeven";

            await axios.put(`http://localhost:5000/api/verhuur-aanvragen/${selectedAanvraag.verhuurAanvraagID}/${status}`, {
                opmerkingen,
            }, {
                headers: { Authorization: `Bearer ${token}` },
            });

            alert("Uitgifte succesvol geregistreerd!");
            handleCloseModal();
            fetchAanvragen(); // Refresh de lijst
        } catch (error) {
            console.error("Error updating status:", error);
            alert("Fout bij het registreren van de uitgifte.");
        }
    };

    return (
        <div className="frontoffice-verhuur-aanvragen-page-container">
            <h2>Verhuur Aanvragen</h2>
            <table className="frontoffice-verhuur-aanvragen-table">
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
                                    className="frontoffice-verhuur-aanvragen-button"
                                    onClick={() => handleOpenModal(aanvraag)}
                                >
                                    Registreer Uitgifte
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="frontoffice-verhuur-aanvragen-modal">
                    <div className="frontoffice-verhuur-aanvragen-modal-content">
                        <h3>Uitgifte Registreren</h3>
                        <p>
                            <strong>Klant:</strong> {selectedAanvraag?.klantNaam}
                        </p>
                        <p>
                            <strong>Voertuig:</strong> {selectedAanvraag?.voertuigInfo}
                        </p>
                        <p>
                            <strong>Datum:</strong> {selectedAanvraag?.startDatum} -{" "}
                            {selectedAanvraag?.eindDatum}
                        </p>
                        <textarea
                            className="frontoffice-verhuur-aanvragen-textarea"
                            value={opmerkingen}
                            onChange={(e) => setOpmerkingen(e.target.value)}
                            placeholder="Voer opmerkingen of extra informatie in"
                        ></textarea>
                        <div className="frontoffice-verhuur-aanvragen-modal-actions">
                            <button onClick={handleUpdateStatus}>Registreren</button>
                            <button onClick={handleCloseModal}>Annuleren</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default FrontofficeVerhuurAanvragenPage;
