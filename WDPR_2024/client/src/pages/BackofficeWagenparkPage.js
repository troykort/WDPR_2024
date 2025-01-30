import React, { useEffect, useState } from "react";
import axios from "axios";
import "./BackofficeWagenparkPage.css";

const BackofficeWagenparkPage = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filteredVoertuigen, setFilteredVoertuigen] = useState([]);
    const [modalVisible, setModalVisible] = useState(false);
    const [editMode, setEditMode] = useState(false);
    const [formData, setFormData] = useState({
        voertuigID: 0,
        merk: "",
        type: "",
        typeVoertuig: "",
        kleur: "",
        kenteken: "",
        aanschafjaar: "",
        prijsPerDag: "",
        status: "",
    });
    const [merkSearchTerm, setMerkSearchTerm] = useState("");
    const [typeSearchTerm, setTypeSearchTerm] = useState("");
    const [error, setError] = useState("");

    useEffect(() => {
        fetchVoertuigen();
    }, []);

    const fetchVoertuigen = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await axios.get("http://localhost:5000/api/voertuigen", {
                headers: { Authorization: `Bearer ${token}` },
            });
            setVoertuigen(response.data);
            setFilteredVoertuigen(response.data); // Initialize filtered list
        } catch (error) {
            console.error("Error fetching voertuigen:", error);
            setError("Er is een fout opgetreden bij het ophalen van de voertuigen.");
        }
    };

    const handleSearch = () => {
        const filtered = voertuigen.filter(
            (voertuig) =>
                voertuig.merk.toLowerCase().includes(merkSearchTerm.toLowerCase()) &&
                voertuig.type.toLowerCase().includes(typeSearchTerm.toLowerCase())
        );
        setFilteredVoertuigen(filtered);
    };

    useEffect(() => {
        handleSearch();
    }, [merkSearchTerm, typeSearchTerm]);

    const openModal = (voertuig = null) => {
        if (voertuig) {
            setEditMode(true);
            setFormData({
                voertuigID: voertuig.voertuigID,
                merk: voertuig.merk,
                type: voertuig.type,
                typeVoertuig: voertuig.typeVoertuig,
                kleur: voertuig.kleur,
                kenteken: voertuig.kenteken,
                aanschafjaar: voertuig.aanschafjaar,
                prijsPerDag: voertuig.prijsPerDag,
                status: voertuig.status,
            });
        } else {
            setEditMode(false);
            setFormData({
                voertuigID: 0,
                merk: "",
                type: "",
                typeVoertuig: "",
                kleur: "",
                kenteken: "",
                aanschafjaar: "",
                prijsPerDag: "",
                status: "",
            });
        }
        setModalVisible(true);
    };

    const closeModal = () => {
        setModalVisible(false);
        setFormData({
            voertuigID: 0,
            merk: "",
            type: "",
            typeVoertuig: "",
            kleur: "",
            kenteken: "",
            aanschafjaar: "",
            prijsPerDag: "",
            status: "",
        });
    };

    const handleFormSubmit = async (e) => {
        e.preventDefault();
        try {
            const token = localStorage.getItem("token");
            if (editMode) {
                await axios.put(
                    `http://localhost:5000/api/voertuigen/${formData.voertuigID}`,
                    formData,
                    { headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` } }
                );
                alert("Voertuig succesvol bijgewerkt!");
            } else {
                await axios.post("http://localhost:5000/api/voertuigen", formData, {
                    headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                });
                alert("Voertuig succesvol toegevoegd!");
            }
            fetchVoertuigen();
            closeModal();
        } catch (error) {
            console.error("Error submitting form:", error);
            setError("Er is een fout opgetreden. Controleer de ingevoerde gegevens.");
        }
    };

    const handleDelete = async (voertuigID) => {
        if (!window.confirm("Weet je zeker dat je dit voertuig wilt verwijderen?")) return;
        try {
            const token = localStorage.getItem("token");
            await axios.delete(`http://localhost:5000/api/voertuigen/${voertuigID}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            alert("Voertuig succesvol verwijderd!");
            fetchVoertuigen();
        } catch (error) {
            console.error("Error deleting voertuig:", error);
            setError("Er is een fout opgetreden bij het verwijderen van het voertuig.");
        }
    };

    return (
        <div className="backoffice-wagenpark-page-container" role="main">
            <h2>Wagenparkbeheer</h2>
            {error && <div role="alert" className="error-message">{error}</div>}
            <div className="wagenpark-header">
                <label htmlFor="merkSearch" className="sr-only">Zoek op merk</label>
                <input
                    id="merkSearch"
                    type="text"
                    placeholder="Zoek op merk..."
                    value={merkSearchTerm}
                    onChange={(e) => setMerkSearchTerm(e.target.value)}
                    className="wagenpark-search-input"
                    aria-label="Zoek op merk"
                />
                <label htmlFor="typeSearch" className="sr-only">Zoek op type</label>
                <input
                    id="typeSearch"
                    type="text"
                    placeholder="Zoek op type..."
                    value={typeSearchTerm}
                    onChange={(e) => setTypeSearchTerm(e.target.value)}
                    className="wagenpark-search-input"
                    aria-label="Zoek op type"
                />
                <button
                    className="wagenpark-add-button"
                    onClick={() => openModal()}
                    aria-label="Voertuig toevoegen"
                >
                    Voertuig Toevoegen
                </button>
            </div>
            <table className="wagenpark-table" aria-label="Tabel met voertuigen">
                <thead>
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">Merk</th>
                        <th scope="col">Type</th>
                        <th scope="col">Type Voertuig</th>
                        <th scope="col">Kleur</th>
                        <th scope="col">Kenteken</th>
                        <th scope="col">Aanschafjaar</th>
                        <th scope="col">Prijs per Dag</th>
                        <th scope="col">Status</th>
                        <th scope="col">Acties</th>
                    </tr>
                </thead>
                <tbody>
                    {filteredVoertuigen.map((voertuig) => (
                        <tr key={voertuig.voertuigID}>
                            <td>{voertuig.voertuigID}</td>
                            <td>{voertuig.merk}</td>
                            <td>{voertuig.type}</td>
                            <td>{voertuig.typeVoertuig}</td>
                            <td>{voertuig.kleur}</td>
                            <td>{voertuig.kenteken}</td>
                            <td>{voertuig.aanschafjaar}</td>
                            <td>€ {voertuig.prijsPerDag}</td>
                            <td>{voertuig.status}</td>
                            <td>
                                <button
                                    className="wagenpark-edit-button"
                                    onClick={() => openModal(voertuig)}
                                    aria-label={`Wijzig ${voertuig.merk} ${voertuig.type}`}
                                >
                                    Wijzigen
                                </button>
                                <button
                                    className="wagenpark-delete-button"
                                    onClick={() => handleDelete(voertuig.voertuigID)}
                                    aria-label={`Verwijder ${voertuig.merk} ${voertuig.type}`}
                                >
                                    Verwijderen
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="wagenpark-modal" role="dialog" aria-modal="true" aria-labelledby="modal-heading">
                    <div className="wagenpark-modal-content">
                        <h3 id="modal-heading">{editMode ? "Voertuig Wijzigen" : "Voertuig Toevoegen"}</h3>
                        <form onSubmit={handleFormSubmit}>
                            <label htmlFor="merk" className="sr-only">Merk</label>
                            <input
                                id="merk"
                                type="text"
                                placeholder="Merk"
                                value={formData.merk}
                                onChange={(e) =>
                                    setFormData({ ...formData, merk: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <label htmlFor="type" className="sr-only">Type</label>
                            <input
                                id="type"
                                type="text"
                                placeholder="Type"
                                value={formData.type}
                                onChange={(e) =>
                                    setFormData({ ...formData, type: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <label htmlFor="typeVoertuig" className="sr-only">Type Voertuig</label>
                            <select
                                id="typeVoertuig"
                                value={formData.typeVoertuig}
                                onChange={(e) =>
                                    setFormData({ ...formData, typeVoertuig: e.target.value })
                                }
                                required
                                aria-required="true"
                            >
                                <option value="Auto">Auto</option>
                                <option value="Camper">Camper</option>
                                <option value="Caravan">Caravan</option>
                            </select>
                            <label htmlFor="kleur" className="sr-only">Kleur</label>
                            <input
                                id="kleur"
                                type="text"
                                placeholder="Kleur"
                                value={formData.kleur}
                                onChange={(e) =>
                                    setFormData({ ...formData, kleur: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <label htmlFor="kenteken" className="sr-only">Kenteken</label>
                            <input
                                id="kenteken"
                                type="text"
                                placeholder="Kenteken"
                                value={formData.kenteken}
                                onChange={(e) =>
                                    setFormData({ ...formData, kenteken: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <label htmlFor="aanschafjaar" className="sr-only">Aanschafjaar</label>
                            <input
                                id="aanschafjaar"
                                type="text"
                                placeholder="Aanschafjaar"
                                value={formData.aanschafjaar}
                                onChange={(e) =>
                                    setFormData({ ...formData, aanschafjaar: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <label htmlFor="prijsPerDag" className="sr-only">Prijs per Dag</label>
                            <input
                                id="prijsPerDag"
                                type="number"
                                placeholder="Prijs per Dag"
                                value={formData.prijsPerDag}
                                onChange={(e) =>
                                    setFormData({ ...formData, prijsPerDag: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <label htmlFor="status" className="sr-only">Status</label>
                            <input
                                id="status"
                                type="text"
                                placeholder="Status"
                                value={formData.status}
                                onChange={(e) =>
                                    setFormData({ ...formData, status: e.target.value })
                                }
                                required
                                aria-required="true"
                            />
                            <div className="wagenpark-modal-actions">
                                <button type="submit">
                                    {editMode ? "Wijzigen" : "Toevoegen"}
                                </button>
                                <button type="button" onClick={closeModal}>
                                    Annuleren
                                </button>
                            </div>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
};

export default BackofficeWagenparkPage;