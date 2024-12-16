import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Voertuigverhuur.css';

const Voertuigverhuur = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filters, setFilters] = useState({
        merk: '',
        type: '',
        typeVoertuig: '',
        sort: '',
    });
    const [selectedVoertuig, setSelectedVoertuig] = useState(null);
    const [showPopup, setShowPopup] = useState(false);

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
                const isTypeVoertuigMatch = filters.typeVoertuig ? voertuig.typeVoertuig.toLowerCase().includes(filters.typeVoertuig.toLowerCase()) : true;
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
        setSelectedVoertuig(selectedVoertuig?.id === voertuig.id ? null : voertuig);
    };

    const handleSubmit = () => {
        if (!selectedVoertuig) {
            alert('Selecteer een voertuig.');
            return;
        }
        setShowPopup(true);
    };

    const handlePopupClose = () => {
        setShowPopup(false);
    };

    const handlePopupConfirm = () => {
        console.log('Huur aanvraag verstuurd:', {
            voertuigID: selectedVoertuig.id,
        });
        alert('Huur aanvraag succesvol verstuurd!');
        setShowPopup(false);
        setSelectedVoertuig(null);
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
                    {applyFilters().map((voertuig) => (
                        <div
                            key={voertuig.id}
                            className={`voertuig-card ${selectedVoertuig?.id === voertuig.id ? 'selected' : ''}`}
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

                <button className="submit-button" onClick={handleSubmit} disabled={!selectedVoertuig}>
                    Bevestig Huur
                </button>

                {showPopup && (
                    <div className="popup-overlay">
                        <div className="popup">
                            <h2>Bevestig Huur</h2>
                            <p>Merk: {selectedVoertuig.merk}</p>
                            <p>Type: {selectedVoertuig.type}</p>
                            <p>Prijs: €{selectedVoertuig.price} per dag</p>
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
