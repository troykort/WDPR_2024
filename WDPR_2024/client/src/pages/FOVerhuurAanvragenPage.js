import React, { useEffect, useState } from "react";
import axios from "axios";
import "./FOVerhuurAanvragenPage.css";

const FrontofficeVerhuurAanvragenPage = () => {
    const [aanvragen, setAanvragen] = useState([]);
    const [selectedAanvraag, setSelectedAanvraag] = useState(null);
    const [opmerkingen, setOpmerkingen] = useState("");
    const [modalVisible, setModalVisible] = useState(false);
    const [isLoading, setIsLoading] = useState(false);

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
        if (opmerkingen.trim()) {
            if (!window.confirm("Weet je zeker dat je de modal wilt sluiten? De ingevoerde opmerkingen gaan verloren.")) {
                return;
            }
        }
        setSelectedAanvraag(null);
        setOpmerkingen("");
        setModalVisible(false);
    };

    const handleUpdateStatus = async () => {
        if (!opmerkingen.trim()) {
            alert("Voer opmerkingen in om de uitgifte te registreren.");
            return;
        }
        if (opmerkingen.length > 500) {
            alert("Opmerkingen mogen niet langer zijn dan 500 tekens.");
            return;
        }
        try {
            setIsLoading(true);
            const token = localStorage.getItem("token");
            const status = "Uitgegeven";
            const userID = localStorage.getItem('Id')
            await axios.put(
                `http://localhost:5000/api/verhuur-aanvragen/${selectedAanvraag.verhuurAanvraagID}/${status}`,
                { userID, opmerkingen },
                { headers: { Authorization: `Bearer ${token}` } }
            );

            alert("Uitgifte succesvol geregistreerd!");
            handleCloseModal();
            fetchAanvragen(); // Refresh de lijst
        } catch (error) {
            console.error("Error updating status:", error);
            const errorMessage = error.response?.data?.message || "Er is iets misgegaan. Probeer opnieuw.";
            alert(errorMessage);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className="frontoffice-verhuur-aanvragen-page-container">
            <h2>Verhuur Aanvragen</h2>
            <table className="frontoffice-verhuur-aanvragen-table" role="table">
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
                                    className="frontoffice-verhuur-aanvragen-button"
                                    onClick={() => handleOpenModal(aanvraag)}
                                    aria-label={`Registreer uitgifte voor aanvraag ${aanvraag.verhuurAanvraagID}`}
                                >
                                    Registreer Uitgifte
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="frontoffice-verhuur-aanvragen-modal" role="dialog" aria-modal="true">
                    <div className="frontoffice-verhuur-aanvragen-modal-content">
                        <h3>Uitgifte Registreren</h3>
                        <p>
                            <strong>Klant:</strong> {selectedAanvraag?.klantNaam}
                        </p>
                        <p>
                            <strong>Voertuig:</strong> {selectedAanvraag?.voertuigInfo}
                        </p>
                        <p>
                            <strong>Datum:</strong> {selectedAanvraag?.startDatum} - {selectedAanvraag?.eindDatum}
                        </p>
                        <label htmlFor="opmerkingen" className="sr-only">Opmerkingen</label>
                        <textarea
                            id="opmerkingen"
                            className="frontoffice-verhuur-aanvragen-textarea"
                            value={opmerkingen}
                            onChange={(e) => setOpmerkingen(e.target.value)}
                            placeholder="Voer opmerkingen of extra informatie in"
                        ></textarea>
                        <div className="frontoffice-verhuur-aanvragen-modal-actions">
                            <button onClick={handleUpdateStatus} disabled={isLoading}>
                                {isLoading ? "Bezig met registreren..." : "Registreren"}
                            </button>
                            <button onClick={handleCloseModal}>Annuleren</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default FrontofficeVerhuurAanvragenPage;




