import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import './Voertuigverhuur.css';

const Voertuigverhuur = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filters, setFilters] = useState({
        merk: '',
        type: '',
        typeVoertuig: '',
        sort: '',
        startDate: '',
        endDate: '',
    });
    const [selectedVoertuig, setSelectedVoertuig] = useState();
    const [showPopup, setShowPopup] = useState(false);
    const [klantID, setKlantID] = useState(null);
    const [displayCount, setDisplayCount] = useState(8);

    useEffect(() => {
        const token = localStorage.getItem('token');
        const KlantID = localStorage.getItem('userId');
        const KlantIDn = Number(KlantID);
        if (token) {
            try {
                const decodedToken = jwtDecode(token);
                console.log('Decoded token:', decodedToken);
                setKlantID(KlantIDn || null);
            } catch (err) {
                console.error('Error decoding token:', err);
            }
        }
    }, []);

    useEffect(() => {
        const fetchVoertuigen = async () => {
            try {
                const response = await axios.get('http://localhost:5000/api/voertuigen/');
                setVoertuigen(response.data);
            } catch (error) {
                console.error('Error fetching voertuigen:', error);
            }
        };
        fetchVoertuigen();
    }, []);

    const applyFilters = () => {
        return voertuigen
            .filter((voertuig) => {
                const isMerkMatch = filters.merk ? voertuig.merk.toLowerCase().includes(filters.merk.toLowerCase()) : true;
                const isTypeMatch = filters.type ? voertuig.type.toLowerCase().includes(filters.type.toLowerCase()) : true;
                const isTypeVoertuigMatch = voertuig.typeVoertuig.toLowerCase() === 'auto';
                return isMerkMatch && isTypeMatch && isTypeVoertuigMatch;
            })
            .sort((a, b) => {
                if (filters.sort === 'priceAsc') return a.prijsPerDag - b.prijsPerDag;
                if (filters.sort === 'priceDesc') return b.prijsPerDag - a.prijsPerDag;
                return 0;
            });
    };

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters((prev) => ({ ...prev, [name]: value }));
    };

    const handleSelectVoertuig = (voertuig) => {
        console.log('Clicked voertuig:', voertuig);
        setSelectedVoertuig((prev) => (prev?.VoertuigID === voertuig.VoertuigID ? voertuig : null));
    };

    const handleSubmit = () => {
        console.log('Selected voertuig:', selectedVoertuig);
        if (!selectedVoertuig) {
            alert('Selecteer een voertuig.');
            return;
        }

        if (!filters.startDate || !filters.endDate) {
            alert('Selecteer een startdatum en einddatum.');
            return;
        }
        setShowPopup(true);
    };

    const handlePopupClose = () => {
        setShowPopup(false);
    };

    const handlePopupConfirm = async () => {
        console.log("Selected voertuig:", selectedVoertuig);
        try {
            const token = localStorage.getItem('token');
            if (!token) {
                alert('Geen geldig token gevonden. Log opnieuw in.');
                return;
            }

            const payload = {
                KlantID: klantID,
                VoertuigID: selectedVoertuig.voertuigID,
                StartDatum: new Date(filters.startDate).toISOString(), // Convert to ISO 8601 format
                EindDatum: new Date(filters.endDate).toISOString(),    // Convert to ISO 8601 format
            };

            console.log('Payload:', payload);

            const response = await axios.post(
                'http://localhost:5000/api/verhuur-aanvragen',
                payload,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            if (response.status === 201 || response.status === 200) {
                alert('Huur aanvraag succesvol verstuurd!');
                setShowPopup(false);
                setSelectedVoertuig(null);
            } else {
                alert('Er is iets misgegaan bij het versturen van de aanvraag.');
            }
        } catch (error) {
            console.error('Error during verhuuraanvraag submission:', error);

            alert('Fout bij het versturen van de aanvraag. Controleer uw invoer en probeer opnieuw.');
        }
    };

    const handleLoadMore = () => {
        setDisplayCount((prevCount) => prevCount + 4);
    };

    return (
        <div className="voertuigen-en-huur-page-container">
            <div className="voertuigen-en-huur-page">
                <h1>Voertuigen Selecteren</h1>

                <div className="filters">
                    <input
                        type="text"
                        name="merk"
                        value={filters.merk}
                        onChange={handleFilterChange}
                        placeholder="Zoek op merk"
                    />
                    <input
                        type="text"
                        name="type"
                        value={filters.type}
                        onChange={handleFilterChange}
                        placeholder="Zoek op type"
                    />
                    <select
                        name="typeVoertuig"
                        value={filters.typeVoertuig}
                        onChange={handleFilterChange}
                    >
                        <option value="">Alle types</option>
                        <option value="auto">Auto</option>
                        <option value="caravan">Caravan</option>
                        <option value="camper">Camper</option>
                    </select>
                    <input
                        type="date"
                        name="startDate"
                        value={filters.startDate}
                        onChange={handleFilterChange}
                        placeholder="Startdatum"
                    />
                    <input
                        type="date"
                        name="endDate"
                        value={filters.endDate}
                        onChange={handleFilterChange}
                        placeholder="Einddatum"
                    />
                    <select
                        name="sort"
                        value={filters.sort}
                        onChange={handleFilterChange}
                    >
                        <option value="">Sorteer op</option>
                        <option value="priceAsc">Prijs: Laag naar Hoog</option>
                        <option value="priceDesc">Prijs: Hoog naar Laag</option>
                    </select>
                </div>
                <div className="voertuigen-list">
                    {applyFilters().slice(0, displayCount).map((voertuig) => (
                        <div
                            key={voertuig.VoertuigID}
                            className={`voertuig-card ${selectedVoertuig?.voertuigID === voertuig.voertuigID ? 'selected' : ''}`}
                            onClick={() => handleSelectVoertuig(voertuig)}
                        >
                            <div className="voertuig-details">
                                <h3 style={{ marginBottom: '0px' }}>{voertuig.merk}</h3>
                                <p style={{ margin: 0, letterSpacing: '-0.1em', lineHeight: '1' }}>__________________________________</p>
                                <p style={{ marginTop: '10px' }}>Type: {voertuig.type}</p>
                                <p>Type Voertuig: {voertuig.typeVoertuig}</p>
                                <p>Prijs per dag: €{voertuig.prijsPerDag}</p>
                                <p>
                                    {voertuig.availableFrom
                                        ? `Niet beschikbaar tot: ${voertuig.availableFrom}`
                                        : 'Beschikbaar'}
                                </p>
                            </div>
                        </div>
                    ))}
                </div>

                <button className="reset-button" onClick={() => setDisplayCount(8)}>
                    Reset
                </button>
                {displayCount < applyFilters().length && (
                    <button className="load-more-button" onClick={handleLoadMore}>
                        Meer laden
                    </button>
                )}
                <button className="submit-button" onClick={handleSubmit}>
                    Bevestig
                </button>

                {showPopup && (
                    <div className="popup-overlay">
                        <div className="popup">
                            <h2>Bevestig Huur</h2>
                            <p>Merk: {selectedVoertuig.merk}</p>
                            <p>Type: {selectedVoertuig.type}</p>
                            <p>Prijs: €{selectedVoertuig.prijsPerDag} per dag</p>
                            <p>Startdatum: {filters.startDate}</p>
                            <p>Einddatum: {filters.endDate}</p>
                            <button onClick={handlePopupConfirm}>Bevestig</button>
                            <button onClick={handlePopupClose}>Annuleer</button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default Voertuigverhuur;
