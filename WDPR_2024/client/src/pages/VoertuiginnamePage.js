import React, { useEffect, useState } from "react";
import axios from "axios";

import "./VoertuiginnamePage.css";

const VoertuiginnamePage = () => {
    const [uitgegevenVoertuigen, setUitgegevenVoertuigen] = useState([]);
    const [selectedVoertuig, setSelectedVoertuig] = useState(null);
    const [schadeBeschrijving, setSchadeBeschrijving] = useState(null);
    const [opmerkingen, setOpmerkingen] = useState(null);
    const [foto, setFoto] = useState(null);
    const [modalVisible, setModalVisible] = useState(false);

    useEffect(() => {
        fetchUitgegevenVoertuigen();
    }, []);

    const fetchUitgegevenVoertuigen = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await axios.get("http://localhost:5000/api/voertuiginname/uitgegeven", {
                headers: { Authorization: `Bearer ${token}` },
            });
            console.log("Uitgegeven voertuigen:", response.data);
            setUitgegevenVoertuigen(response.data);
        } catch (error) {
            console.error("Error fetching uitgegeven voertuigen:", error);
        }
    };

    const handleOpenModal = (voertuig) => {
        setSelectedVoertuig(voertuig);
        setSchadeBeschrijving("");
        setOpmerkingen("");
        setFoto(null);
        setModalVisible(true);
    };

    const handleCloseModal = () => {
        setSelectedVoertuig(null);
        setSchadeBeschrijving("");
        setOpmerkingen("");
        setFoto(null);
        setModalVisible(false);
    };

    const handleInname = async () => {
        if (!selectedVoertuig) return;

        try {
            const token = localStorage.getItem("token");
            const data = new FormData();
            data.append("voertuigId", selectedVoertuig.voertuigID);
            data.append("klantId", selectedVoertuig.huidigeHuurderID);
            data.append("beschrijving", schadeBeschrijving);
            data.append("opmerkingen", opmerkingen);
            if (foto) {
                data.append("foto", foto);
            }
            console.log("Inname data:", data);
            await axios.post("http://localhost:5000/api/voertuiginname", data, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "multipart/form-data",
                },
            });

            alert("Voertuig succesvol ingenomen!");
            handleCloseModal();
            fetchUitgegevenVoertuigen(); // Refresh de lijst
        } catch (error) {
            console.error("Error processing inname:", error);
            alert("Er is een fout opgetreden bij het innemen van het voertuig.");
        }
    };

    return (
        <div className="voertuiginname-page-container">
            <h2>Voertuiginname en Schademeldingen</h2>
            <table className="voertuiginname-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Voertuig</th>
                        <th>Klant</th>
                        <th>Uitgiftedatum</th>
                        <th>Acties</th>
                    </tr>
                </thead>
                <tbody>
                    {uitgegevenVoertuigen.map((voertuig) => (
                        <tr key={voertuig.voertuigID}>
                            <td>{voertuig.voertuigID}</td>
                            <td>{voertuig.merk} {voertuig.type}</td>
                            <td>{voertuig.huidigeHuurderNaam}</td>
                            <td>{voertuig.uitgiftedatum}</td>
                            <td>
                                <button
                                    className="voertuiginname-button"
                                    onClick={() => handleOpenModal(voertuig)}
                                    aria-label={`Neem in ${voertuig.merk} ${voertuig.type}`}
                                >
                                    Neem in
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="voertuiginname-modal" role="dialog" aria-modal="true">
                    <div className="voertuiginname-modal-content">
                        <h3>Inname Registreren</h3>
                        <p><strong>Voertuig:</strong> {selectedVoertuig?.merk} {selectedVoertuig?.type}</p>
                        <p><strong>Klant:</strong> {selectedVoertuig?.huidigeHuurderNaam}</p>
                        <label htmlFor="schadeBeschrijving" className="sr-only">Beschrijving van de schade (optioneel)</label>
                        <textarea
                            id="schadeBeschrijving"
                            placeholder="Beschrijving van de schade (optioneel)"
                            value={schadeBeschrijving}
                            onChange={(e) => setSchadeBeschrijving(e.target.value)}
                            className="voertuiginname-textarea"
                        ></textarea>
                        <label htmlFor="opmerkingen" className="sr-only">Opmerkingen (optioneel)</label>
                        <textarea
                            id="opmerkingen"
                            placeholder="Opmerkingen (optioneel)"
                            value={opmerkingen}
                            onChange={(e) => setOpmerkingen(e.target.value)}
                            className="voertuiginname-textarea"
                        ></textarea>
                        <label htmlFor="foto" className="sr-only">Upload een foto</label>
                        <input
                            type="file"
                            id="foto"
                            onChange={(e) => setFoto(e.target.files[0])}
                            className="voertuiginname-file-input"
                        />
                        <div className="voertuiginname-modal-actions">
                            <button onClick={handleInname}>Bevestigen</button>
                            <button onClick={handleCloseModal}>Annuleren</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default VoertuiginnamePage;

