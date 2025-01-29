import React, { useState, useEffect } from 'react';
import './RentalHistory.css';

function RentalHistoryPage() {
    const [history, setHistory] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchRentalHistory = async () => {
            try {
                const token = localStorage.getItem('token');
                const klantId = localStorage.getItem('userId');
                const klantIdInt = parseInt(klantId, 10);

                if (isNaN(klantIdInt)) {
                    throw new Error('Invalid klantId');
                }

                const response = await fetch(`http://localhost:5000/api/verhuur-aanvragen/geschiedenis/${klantIdInt}`, {
                    method: 'GET',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                    },
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch rental history.');
                }

                const data = await response.json();
                console.log("Fetched rental history data:", data);  // Add this line to check data structure

                const safeHistory = data.map(rental => ({
                    VoertuigInfo: rental.voertuigInfo || "Unknown vehicle",
                    StartDatum: rental.startDatum || "Unknown start date",
                    EindDatum: rental.eindDatum || "Unknown end date",
                    KlantNaam: rental.klantNaam || "Unknown customer",
                }));

                console.log("Mapped rental history:", safeHistory);  // Check if mapped data is correct

                setHistory(safeHistory);
                setLoading(false);
            } catch (error) {
                console.error('Error fetching rental history:', error.message);
                setError(error.message);
                setLoading(false);
            }
        };

        fetchRentalHistory();
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div className="rental-history-page">
            <h1>Rental History</h1>
            {history.length === 0 ? (
                <p>No rental history found.</p>
            ) : (
                <table className="rental-history-table">
                    <thead>
                        <tr>
                            <th>Vehicle</th>
                            <th>Start Date</th>
                            <th>End Date</th>
                            <th>Customer</th>
                        </tr>
                    </thead>
                    <tbody>
                        {history.map((rental, index) => (
                            <tr key={index}>
                                <td>{rental.VoertuigInfo}</td>
                                <td>{rental.StartDatum}</td>
                                <td>{rental.EindDatum}</td>
                                <td>{rental.KlantNaam}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
}

export default RentalHistoryPage;
