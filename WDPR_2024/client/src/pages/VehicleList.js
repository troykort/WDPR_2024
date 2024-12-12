import React, { useEffect, useState } from 'react';
import './VehicleList.css';

function VehicleList() {
    const [vehicles, setVehicles] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchVehicles = async () => {
            try {
                const response = await fetch('http://localhost:5000/api/voertuigen');
                setVehicles(response.data);
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        };

        fetchVehicles();
    }, []);

    if (loading) return <p>Laden...</p>;
    if (error) return <p>Error: {error}</p>;

    return (
        <div className="vehicle-list">
            <h1>Alle voertuigen</h1>
            <ul>
                {vehicles.map(vehicle => (
                    <li key={vehicle.id}>
                        <h2>{vehicle.name}</h2>
                        <p>Type: {vehicle.type}</p>
                        <p>Model: {vehicle.model}</p>
                        <p>Jaar: {vehicle.year}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default VehicleList;