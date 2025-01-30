import React, { useEffect, useState } from "react";
import axios from "axios";
import "./BackofficeSchademeldingenPage.css";

const BackofficeSchademeldingenPage = () => {
    const [schademeldingen, setSchademeldingen] = useState([]);
    const [selectedSchademelding, setSelectedSchademelding] = useState(null);
    const [nieuweStatus, setNieuweStatus] = useState("");
    const [opmerkingen, setOpmerkingen] = useState("");
    const [modalVisible, setModalVisible] = useState(false);
    const [photoPopupVisible, setPhotoPopupVisible] = useState(false);
    const [selectedPhoto, setSelectedPhoto] = useState(null);

    useEffect(() => {
        fetchSchademeldingen();
    }, []);

    const fetchSchademeldingen = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await axios.get("http://localhost:5000/api/schademeldingen", {
                headers: { Authorization: `Bearer ${token}` },
            });
            if (Array.isArray(response.data)) {
                setSchademeldingen(response.data);
            } else {
                setSchademeldingen([]);
                console.error("Unexpected response format:", response.data);
            }
        } catch (error) {
            console.error("Error fetching schademeldingen:", error);
            setSchademeldingen([]);
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

    const openPhotoPopup = (photoPath) => {
        setSelectedPhoto(photoPath);
        setPhotoPopupVisible(true);
    };

    const closePhotoPopup = () => {
        setSelectedPhoto(null);
        setPhotoPopupVisible(false);
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
            <h1>Backoffice Schademeldingen Beheer</h1>
            <p>Beheer en werk schademeldingen bij van voertuigen. Zie details en voeg opmerkingen toe.</p>
            <table className="backoffice-schademeldingen-table" aria-label="Lijst van schademeldingen">
                <thead>
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">Voertuig</th>
                        <th scope="col">Klant</th>
                        <th scope="col">Status</th>
                        <th scope="col">Datum</th>
                        <th scope="col">Foto</th>
                        <th scope="col">Acties</th>
                    </tr>
                </thead>
                <tbody>
                    {schademeldingen.map((melding) => (
                        <tr key={melding.schademeldingID}>
                            <td>{melding.schademeldingID}</td>
                            <td>{melding.voertuigMerk} {melding.voertuigType}</td>
                            <td>{melding.klantNaam || "Onbekend"}</td>
                            <td>{melding.status}</td>
                            <td>{new Date(melding.melddatum).toLocaleDateString()}</td>
                            <td>
                                {melding.fotoPath && (
                                    <>
                                        <button
                                            className="view-photo-button"
                                            onClick={() => openPhotoPopup(melding.fotoPath)}
                                            aria-label={`Bekijk foto van schademelding ${melding.schademeldingID}`}
                                        >
                                            View Photo
                                        </button>
                                        <img
                                            src={melding.fotoPath}
                                            alt={`Schade foto van ${melding.voertuigMerk} ${melding.voertuigType}`}
                                            className="schade-foto"
                                        />
                                    </>
                                )}
                            </td>
                            <td>
                                <button
                                    className="backoffice-schademeldingen-button"
                                    onClick={() => openModal(melding)}
                                    aria-label={`Bewerk schademelding ${melding.schademeldingID}`}
                                >
                                    Bewerken
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="backoffice-schademeldingen-modal" role="dialog" aria-labelledby="modal-title" aria-modal="true">
                    <div className="backoffice-schademeldingen-modal-content">
                        <h3 id="modal-title">Schademelding Bijwerken</h3>
                        <p><strong>Voertuig:</strong> {selectedSchademelding?.voertuigMerk} {selectedSchademelding?.voertuigType}</p>
                        <p><strong>Klant:</strong> {selectedSchademelding?.klantNaam}</p>
                        <p><strong>Huidige Status:</strong> {selectedSchademelding?.status}</p>
                        <textarea
                            placeholder="Nieuwe opmerkingen (optioneel)"
                            value={opmerkingen}
                            onChange={(e) => setOpmerkingen(e.target.value)}
                            className="backoffice-schademeldingen-textarea"
                            aria-label="Opmerkingen invoeren"
                        ></textarea>
                        <select
                            value={nieuweStatus}
                            onChange={(e) => setNieuweStatus(e.target.value)}
                            className="backoffice-schademeldingen-select"
                            aria-label="Selecteer nieuwe status"
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

            {photoPopupVisible && (
                <div className="photo-popup" onClick={closePhotoPopup} role="dialog" aria-labelledby="photo-popup-title" aria-modal="true">
                    <div className="photo-popup-content">
                        <h3 id="photo-popup-title">Schade Foto</h3>
                        <img src={selectedPhoto} alt="Volledige schade foto" />
                    </div>
                </div>
            )}
        </div>
    );
};

export default BackofficeSchademeldingenPage;