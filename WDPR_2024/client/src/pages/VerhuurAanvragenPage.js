import React, { useEffect, useState } from "react";
import axios from "axios";
import "./VerhuurAanvragenPage.css";

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
            const response = await axios.get("http://localhost:5000/api/verhuur-aanvragen");
            setAanvragen(response.data);
        } catch (error) {
            console.error("Error fetching verhuuraanvragen:", error);
        }
    };

    const handleGoedkeuren = async (id) => {
        try {
            await axios.post(`http://localhost:5000/api/verhuur-aanvragen/${id}/goedkeuren`);
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
            await axios.post(`http://localhost:5000/api/verhuur-aanvragen/${selectedAanvraag}/afwijzen`, {
                reden: afwijsReden,
            });
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
        <div className="verhuur-aanvragen-page">
            <h2>Verhuur Aanvragen</h2>
            <table className="aanvragen-table">
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
                        <tr key={aanvraag.id}>
                            <td>{aanvraag.id}</td>
                            <td>{aanvraag.huurderNaam}</td>
                            <td>{aanvraag.voertuig}</td>
                            <td>{aanvraag.datum}</td>
                            <td>{aanvraag.status}</td>
                            <td>
                                <button onClick={() => handleGoedkeuren(aanvraag.id)}>Goedkeuren</button>
                                <button onClick={() => openAfwijzenModal(aanvraag.id)}>Afwijzen</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="modal">
                    <div className="modal-content">
                        <h3>Reden voor afwijzing</h3>
                        <textarea
                            value={afwijsReden}
                            onChange={(e) => setAfwijsReden(e.target.value)}
                            placeholder="Voer een reden in"
                        ></textarea>
                        <div className="modal-actions">
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
