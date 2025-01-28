﻿import React, { useEffect, useState } from "react";
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
            alert("Er is een fout opgetreden. Controleer de ingevoerde gegevens.");
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
            alert("Er is een fout opgetreden bij het verwijderen van het voertuig.");
        }
    };

    return (
        <div className="backoffice-wagenpark-page-container">
            <h2>Wagenparkbeheer</h2>
            <div className="wagenpark-header">
                <input
                    type="text"
                    placeholder="Zoek op merk..."
                    value={merkSearchTerm}
                    onChange={(e) => setMerkSearchTerm(e.target.value)}
                    className="wagenpark-search-input"
                />
                <input
                    type="text"
                    placeholder="Zoek op type..."
                    value={typeSearchTerm}
                    onChange={(e) => setTypeSearchTerm(e.target.value)}
                    className="wagenpark-search-input"
                />
                <button className="wagenpark-add-button" onClick={() => openModal()}>
                    Voertuig Toevoegen
                </button>
            </div>
            <table className="wagenpark-table">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Merk</th>
                        <th>Type</th>
                        <th>Type Voertuig</th>
                        <th>Kleur</th>
                        <th>Kenteken</th>
                        <th>Aanschafjaar</th>
                        <th>Prijs per Dag</th>
                        <th>Status</th>
                        <th>Acties</th>
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
                                >
                                    Wijzigen
                                </button>
                                <button
                                    className="wagenpark-delete-button"
                                    onClick={() => handleDelete(voertuig.voertuigID)}
                                >
                                    Verwijderen
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            {modalVisible && (
                <div className="wagenpark-modal">
                    <div className="wagenpark-modal-content">
                        <h3>{editMode ? "Voertuig Wijzigen" : "Voertuig Toevoegen"}</h3>
                        <form onSubmit={handleFormSubmit}>
                            <input
                                type="text"
                                placeholder="Merk"
                                value={formData.merk}
                                onChange={(e) =>
                                    setFormData({ ...formData, merk: e.target.value })
                                }
                                required
                            />
                            <input
                                type="text"
                                placeholder="Type"
                                value={formData.type}
                                onChange={(e) =>
                                    setFormData({ ...formData, type: e.target.value })
                                }
                                required
                            />
                            <select
                                value={formData.typeVoertuig}
                                onChange={(e) =>
                                    setFormData({ ...formData, typeVoertuig: e.target.value })
                                }
                                required
                            >
                                <option value="Auto">Auto</option>
                                <option value="Camper">Camper</option>
                                <option value="Caravan">Caravan</option>
                            </select>
                            <input
                                type="text"
                                placeholder="Kleur"
                                value={formData.kleur}
                                onChange={(e) =>
                                    setFormData({ ...formData, kleur: e.target.value })
                                }
                                required
                            />
                            <input
                                type="text"
                                placeholder="Kenteken"
                                value={formData.kenteken}
                                onChange={(e) =>
                                    setFormData({ ...formData, kenteken: e.target.value })
                                }
                                required
                            />
                            <input
                                type="text"
                                placeholder="Aanschafjaar"
                                value={formData.aanschafjaar}
                                onChange={(e) =>
                                    setFormData({ ...formData, aanschafjaar: e.target.value })
                                }
                                required
                            />
                            <input
                                type="number"
                                placeholder="Prijs per Dag"
                                value={formData.prijsPerDag}
                                onChange={(e) =>
                                    setFormData({ ...formData, prijsPerDag: e.target.value })
                                }
                                required
                            />
                            <input
                                type="text"
                                placeholder="Status"
                                value={formData.status}
                                onChange={(e) =>
                                    setFormData({ ...formData, status: e.target.value })
                                }
                                required
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
