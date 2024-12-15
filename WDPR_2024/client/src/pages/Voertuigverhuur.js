import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './Voertuigverhuur.css';

const Voertuigverhuur = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filters, setFilters] = useState({
        type: '',
        startDate: '',
        endDate: '',
        sort: '',
    });
    const [selectedVoertuig, setSelectedVoertuig] = useState(null);
    const [visibleCount, setVisibleCount] = useState(6); // Aantal voertuigen dat in eerste instantie wordt weergegeven

    useEffect(() => {
        fetchVoertuigen();
    }, []);

    const fetchVoertuigen = async () => {
        try {
            const response = await axios.get('http://localhost:5000/api/voertuigen/');
            setVoertuigen(response.data);
        } catch (error) {
            console.error('Error fetching voertuigen:', error);
        }
    };

    const applyFilters = () => {
        return voertuigen.filter(voertuig => {
            const isTypeMatch = filters.type ? voertuig.typeVoertuig === filters.type : true;
            const isAvailable = filters.startDate && filters.endDate
                ? new Date(filters.startDate) >= new Date(voertuig.availableFrom) &&
                new Date(filters.endDate) <= new Date(voertuig.availableTo)
                : true;
            return isTypeMatch && isAvailable;
        }).sort((a, b) => {
            if (filters.sort === 'price') return a.price - b.price;
            if (filters.sort === 'merk') return a.merk.localeCompare(b.merk);
            if (filters.sort === 'availability') return new Date(a.availableFrom) - new Date(b.availableFrom);
            return 0;
        });
    };

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters((prev) => ({ ...prev, [name]: value }));
    };

    const handleSelectVoertuig = (voertuig) => {
        setSelectedVoertuig(voertuig);
    };

    const handleCheckboxChange = (voertuig) => {
        if (selectedVoertuig?.id === voertuig.id) {
            setSelectedVoertuig(null);
        } else {
            setSelectedVoertuig(voertuig);
        }
    };

    const handleSubmit = () => {
        if (!selectedVoertuig || !filters.startDate || !filters.endDate) {
            alert('Selecteer een voertuig en een geldige huurperiode.');
            return;
        }
        // Voeg hier API-aanroep toe voor het aanvragen van verhuur
        console.log('Huur aanvraag verstuurd:', {
            voertuigID: selectedVoertuig.id,
            startDate: filters.startDate,
            endDate: filters.endDate,
        });
        alert('Huur aanvraag succesvol verstuurd!');
    };

    const handleLoadMore = () => {
        setVisibleCount((prevCount) => prevCount + 6); // Laad 6 extra voertuigen
    };

    const handleReset = () => {
        setVisibleCount(6); // Reset het aantal zichtbare voertuigen
        setFilters({
            type: '',
            startDate: '',
            endDate: '',
            sort: '',
        });
        setSelectedVoertuig(null); // Deselecteer het voertuig
    };

    return (
        <div className="voertuigen-en-huur-page-container">
            <div className="voertuigen-en-huur-page">
                <h1>Voertuigen Selecteren & Huurperiode</h1>

                <div className="filters">
                    <select name="type" value={filters.type} onChange={handleFilterChange}>
                        <option value="">Alle Voertuigtypen</option>
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
                    <select name="sort" value={filters.sort} onChange={handleFilterChange}>
                        <option value="">Sorteer op</option>
                        <option value="price">Prijs</option>
                        <option value="merk">Merk</option>
                        <option value="availability">Beschikbaarheid</option>
                    </select>
                </div>

                <div className="voertuigen-list">
                    {applyFilters().slice(0, visibleCount).map((voertuig) => (
                        <div
                            key={voertuig.id}
                            className={`voertuig-card ${selectedVoertuig?.id === voertuig.id ? 'selected' : ''}`}
                        >
                            <img src={voertuig.imageUrl} alt={voertuig.merk} className="voertuig-image" />
                            <div className="voertuig-details">
                                <h3>{voertuig.merk}</h3>
                                <p>Type: {voertuig.typeVoertuig}</p>
                                <p>Prijs per dag: €{voertuig.price}</p>
                                <p>Beschikbaar van: {voertuig.availableFrom} tot {voertuig.availableTo}</p>
                                <input
                                    type="checkbox"
                                    checked={selectedVoertuig?.id === voertuig.id}
                                    onChange={() => handleCheckboxChange(voertuig)}
                                />
                            </div>
                        </div>
                    ))}
                </div>

                {visibleCount < applyFilters().length && (
                    <button className="load-more-button" onClick={handleLoadMore}>
                        Meer laden
                    </button>
                )}

                <button className="reset-button" onClick={handleReset}>
                    Reset
                </button>

                <button className="submit-button" onClick={handleSubmit}>
                    Bevestig Huur
                </button>
            </div>
        </div>
    );
};

export default Voertuigverhuur;
