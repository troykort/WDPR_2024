import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'; // Proper import
import './Voertuigverhuur.css';

const Voertuigverhuur = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filters, setFilters] = useState({
        merk: '',
        type: '',
        typeVoertuig: '',
        sort: '',
        startDate: '', // Start date for rental
        endDate: '', // End date for rental
    });
    const [selectedVoertuig, setSelectedVoertuig] = useState();
    const [showPopup, setShowPopup] = useState(false);
    const [klantID, setKlantID] = useState(null); // Store decoded ID

    useEffect(() => {
        // Decode the token once and store klantID
        const token = localStorage.getItem('token');
        const KlantID = localStorage.getItem('userId');
        const KlantIDn = Number(KlantID); 
        if (token) {
            try {
                
                const decodedToken = jwtDecode(token);
                console.log('Decoded token:', decodedToken);
                setKlantID(KlantIDn || null); // Extract ID
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
                const isTypeVoertuigMatch = filters.typeVoertuig
                    ? voertuig.typeVoertuig.toLowerCase().includes(filters.typeVoertuig.toLowerCase())
                    : true;
                return isMerkMatch && isTypeMatch && isTypeVoertuigMatch;
            })
            .sort((a, b) => {
                if (filters.sort === 'priceAsc') return a.price - b.price;
                if (filters.sort === 'priceDesc') return b.price - a.price;
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
                KlantID:klantID,
                VoertuigID:selectedVoertuig.voertuigID,
                StartDatum:new Date(filters.startDate).toISOString(), // Convert to ISO 8601 format
                EindDatum:new Date(filters.endDate).toISOString(),    // Convert to ISO 8601 format
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
                <button className="submit-button" onClick={handleSubmit}>
                    Bevestig Huur
                </button>
                <div className="voertuigen-list">
                    {applyFilters().map((voertuig) => (
                        <div
                            key={voertuig.VoertuigID}
                            className={`voertuig-card ${selectedVoertuig?.VoertuigID === voertuig.VoertuigID ? 'selected' : ''}`}
                            onClick={() => handleSelectVoertuig(voertuig)}
                        >
                            <div className="voertuig-details">
                                <h3>{voertuig.merk}</h3>
                                <p>Type: {voertuig.type}</p>
                                <p>Type Voertuig: {voertuig.typeVoertuig}</p>
                                <p>Prijs per dag: €{voertuig.price}</p>
                                <p>
                                    {voertuig.availableFrom
                                        ? `Niet beschikbaar tot: ${voertuig.availableFrom}`
                                        : 'Beschikbaar'}
                                </p>
                            </div>
                        </div>
                    ))}
                </div>


                {showPopup && (
                    <div className="popup-overlay">
                        <div className="popup">
                            <h2>Bevestig Huur</h2>
                            <p>Merk: {selectedVoertuig.merk}</p>
                            <p>Type: {selectedVoertuig.type}</p>
                            <p>Prijs: €{selectedVoertuig.price} per dag</p>
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
