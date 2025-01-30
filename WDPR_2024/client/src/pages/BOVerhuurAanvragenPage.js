import React, { useEffect, useState } from "react";
import axios from "axios";
import "./BOVerhuurAanvragenPage.css";

const BOVerhuurAanvragenPage = () => {
    const [aanvragen, setAanvragen] = useState([]);
    const [afwijsReden, setAfwijsReden] = useState("");
    const [modalVisible, setModalVisible] = useState(false);
    const [selectedAanvraag, setSelectedAanvraag] = useState(null);
    const [error, setError] = useState("");

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
            setError("Er is een fout opgetreden bij het ophalen van de verhuuraanvragen.");
        }
    };

    const handleGoedkeuren = async (id) => {
        try {
            const token = localStorage.getItem("token");
            const status = "Goedgekeurd";
            const opmerkingen = "Aanvraag is goedgekeurd";
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

            fetchAanvragen(); // Refresh de lijst
            alert("Aanvraag is goedgekeurd!");
        } catch (error) {
            console.error("Error approving aanvraag:", error);
            setError("Er is een fout opgetreden bij het goedkeuren van de aanvraag.");
        }
    };

    const handleAfwijzen = async () => {
        if (!afwijsReden.trim()) {
            setError("Voer een reden in voor afwijzing.");
            return;
        }

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
            setError("Er is een fout opgetreden bij het afwijzen van de aanvraag.");
        }
    };

    const openAfwijzenModal = (id) => {
        setSelectedAanvraag(id);
        setModalVisible(true);
        setError(""); // Reset error message when opening the modal
    };

    const closeAfwijzenModal = () => {
        setAfwijsReden("");
        setModalVisible(false);
    };

    return (
        <div className="bo-verhuur-aanvragen-page-container" role="main">
            <h2>Backoffice Verhuur Aanvragen</h2>
            {error && <div role="alert" className="error-message">{error}</div>}
            <table className="bo-verhuur-aanvragen-table" aria-label="Tabel met verhuuraanvragen">
                <thead>
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">Naam Huurder</th>
                        <th scope="col">Voertuig</th>
                        <th scope="col">Datum</th>
                        <th scope="col">Status</th>
                        <th scope="col">Acties</th>
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
                                    aria-label={`Goedkeuren aanvraag ${aanvraag.verhuurAanvraagID}`}
                                >
                                    Goedkeuren
                                </button>
                                <button
                                    className="bo-verhuur-aanvragen-button bo-verhuur-aanvragen-button-reject"
                                    onClick={() => openAfwijzenModal(aanvraag.verhuurAanvraagID)}
                                    aria-label={`Afwijzen aanvraag ${aanvraag.verhuurAanvraagID}`}
                                >
                                    Afwijzen
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="bo-verhuur-aanvragen-modal" role="dialog" aria-labelledby="modal-heading">
                    <div className="bo-verhuur-aanvragen-modal-content">
                        <h3 id="modal-heading">Reden voor afwijzing</h3>
                        <textarea
                            className="bo-verhuur-aanvragen-textarea"
                            value={afwijsReden}
                            onChange={(e) => setAfwijsReden(e.target.value)}
                            placeholder="Voer een reden in"
                            aria-label="Reden voor afwijzing"
                        ></textarea>
                        {error && <div role="alert" className="error-message">{error}</div>}
                        <div className="bo-verhuur-aanvragen-modal-actions">
                            <button onClick={handleAfwijzen} aria-label="Bevestig afwijzing">
                                Afwijzen
                            </button>
                            <button onClick={closeAfwijzenModal} aria-label="Annuleer afwijzing">
                                Annuleren
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default BOVerhuurAanvragenPage;