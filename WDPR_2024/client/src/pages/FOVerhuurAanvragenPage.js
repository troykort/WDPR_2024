import React, { useEffect, useState } from "react";
import axios from "axios";

import "./VerhuurAanvragenPage.css";

const VerhuurAanvragenPage = () => {
    const [aanvragen, setAanvragen] = useState([]);
    const [modalVisible, setModalVisible] = useState(false);

    useEffect(() => {
        fetchAanvragen();
    }, []);

    const fetchAanvragen = async () => {
        try {
            const token = localStorage.getItem("token");
            const response = await axios.get("http://localhost:5000/api/verhuur-aanvragen", {
                headers: { Authorization: `Bearer ${token}` },
            });
            setAanvragen(response.data);
            console.log("Fetched aanvragen:", response.data);
        } catch (error) {
            console.error("Error fetching verhuuraanvragen:", error);
        }
    };


    return (
        <div className="verhuur-aanvragen-page-container">
            <h2>Verhuur Aanvragen</h2>
            <table className="verhuur-aanvragen-table">
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
                        <tr key={aanvraag.verhuurAanvraagID}>
                            <td>{aanvraag.verhuurAanvraagID}</td>
                            <td>{aanvraag.klantNaam || "Onbekend"}</td>
                            <td>{aanvraag.voertuigInfo || "Onbekend voertuig"}</td>
                            <td>{`${aanvraag.startDatum} - ${aanvraag.eindDatum}` || "Geen datum beschikbaar"}</td>
                            <td>{aanvraag.status}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default VerhuurAanvragenPage;
