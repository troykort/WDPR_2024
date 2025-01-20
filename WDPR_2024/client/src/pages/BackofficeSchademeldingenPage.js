import React, { useEffect, useState } from "react";
import axios from "axios";

import "./BackofficeSchademeldingenPage.css";

const BackofficeSchademeldingenPage = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [selectedSchademelding, setSelectedSchademelding] = useState(null);
    const [nieuweStatus, setNieuweStatus] = useState("");
    const [opmerkingen, setOpmerkingen] = useState("");
    const [modalVisible, setModalVisible] = useState(false);

   
    useEffect(() => {
        fetchSchademeldingen();
    }, []);

    const fetchSchademeldingen = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await axios.get("http://localhost:5000/api/schademeldingen", {
                headers: { Authorization: `Bearer ${token}` },
            });
            setSchademeldingen(response.data);
        } catch (error) {
            console.error("Error fetching schademeldingen:", error);
        }
    };

    const openModal = (schademelding) => {
        setSelectedSchademelding(schademelding);
        setNieuweStatus("");
        setOpmerkingen("");
        setModalVisible(true);
    };

    const closeModal = () => {
        setSelectedSchademelding(null);
        setNieuweStatus("");
        setOpmerkingen("");
        setModalVisible(false);
    };

    const handleStatusUpdate = async () => {
        if (!selectedSchademelding) return;
        try {
            const token = localStorage.getItem("token");
            await axios.put(
                `http://localhost:5000/api/schademeldingen/${selectedSchademelding.schademeldingID}/status`,
                null,
                {
                    params: {
                        nieuweStatus,
                        opmerkingen,
                    },
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
            alert("Status succesvol bijgewerkt!");
            closeModal();
            fetchSchademeldingen(); 
        } catch (error) {
            console.error("Error updating status:", error);
            alert("Er is een fout opgetreden bij het bijwerken van de status.");
        }
    };

    return (
        <div className="backoffice-schademeldingen-page-container">
            <h2>Backoffice Schademeldingen Beheer</h2>
            <p>Beheer en werk schademeldingen bij van voertuigen. Zie details en voeg opmerkingen toe.</p>
            <table className="backoffice-schademeldingen-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Voertuig</th>
                        <th>Klant</th>
                        <th>Status</th>
                        <th>Datum</th>
                        <th>Acties</th>
                    </tr>
                </thead>
                <tbody>
                    {schademeldingen.map((melding) => (
                        <tr key={melding.schademeldingID}>
                            <td>{melding.schademeldingID}</td>
                            <td>{melding.voertuig?.merk} {melding.voertuig?.type}</td>
                            <td>{melding.klant?.naam || "Onbekend"}</td>
                            <td>{melding.status}</td>
                            <td>{new Date(melding.melddatum).toLocaleDateString()}</td>
                            <td>
                                <button
                                    className="backoffice-schademeldingen-button"
                                    onClick={() => openModal(melding)}
                                >
                                    Bewerken
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="backoffice-schademeldingen-modal">
                    <div className="backoffice-schademeldingen-modal-content">
                        <h3>Schademelding Bijwerken</h3>
                        <p><strong>Voertuig:</strong> {selectedSchademelding?.voertuig?.merk} {selectedSchademelding?.voertuig?.type}</p>
                        <p><strong>Klant:</strong> {selectedSchademelding?.klant?.naam}</p>
                        <p><strong>Huidige Status:</strong> {selectedSchademelding?.status}</p>
                        <textarea
                            placeholder="Nieuwe opmerkingen (optioneel)"
                            value={opmerkingen}
                            onChange={(e) => setOpmerkingen(e.target.value)}
                            className="backoffice-schademeldingen-textarea"
                        ></textarea>
                        <select
                            value={nieuweStatus}
                            onChange={(e) => setNieuweStatus(e.target.value)}
                            className="backoffice-schademeldingen-select"
                        >
                            <option value="">Selecteer nieuwe status</option>
                            <option value="In Behandeling">In Behandeling</option>
                            <option value="Afgehandeld">Afgehandeld</option>
                        </select>
                        <div className="backoffice-schademeldingen-modal-actions">
                            <button onClick={handleStatusUpdate}>Bijwerken</button>
                            <button onClick={closeModal}>Annuleren</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default BackofficeSchademeldingenPage;
