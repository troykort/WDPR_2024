import React, { useEffect, useState } from "react";
import "./VoertuigOverzichtPage.css";
import axios from "axios";

const VoertuigOverzichtPage = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filters, setFilters] = useState({
        startDate: "",
        endDate: "",
        voertuigType: "",
        huurderNaam: ""
    });

    useEffect(() => {
        fetchVoertuigen();
    }, [filters]);

    const fetchVoertuigen = async () => {
        try {
            const response = await axios.get("/api/verhuur-aanvragen/verhuurd", {
                params: {
                    startDate: filters.startDate,
                    endDate: filters.endDate,
                    voertuigType: filters.voertuigType,
                    huurderNaam: filters.huurderNaam
                }
            });
            setVoertuigen(response.data);
        } catch (error) {
            console.error("Error fetching voertuigen:", error);
        }
    };

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters((prev) => ({ ...prev, [name]: value }));
    };

    const exportToCSV = (voertuig) => {
        const csvContent = "data:text/csv;charset=utf-8," +
            ["Voertuig,Type,Huurder,Startdatum,Einddatum"].join(",") +
            "\n" +
            `${voertuig.voertuig.merk} ${voertuig.voertuig.type},${voertuig.voertuig.typeVoertuig},${voertuig.klant.naam},${voertuig.startDatum},${voertuig.eindDatum}`;
        const encodedUri = encodeURI(csvContent);
        const link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute("download", `${voertuig.voertuig.merk}_${voertuig.voertuig.type}.csv`);
        document.body.appendChild(link);
        link.click();
    };

    return (
        <div className="voertuig-overzicht">
            <h2>Voertuig Overzicht</h2>
            <div className="filters">
                <input type="date" name="startDate" value={filters.startDate} onChange={handleFilterChange} placeholder="Startdatum" />
                <input type="date" name="endDate" value={filters.endDate} onChange={handleFilterChange} placeholder="Einddatum" />
                <input type="text" name="voertuigType" value={filters.voertuigType} onChange={handleFilterChange} placeholder="Voertuigtype" />
                <input type="text" name="huurderNaam" value={filters.huurderNaam} onChange={handleFilterChange} placeholder="Huurder Naam" />
            </div>
            <div className="voertuig-list">
                {voertuigen.map((voertuig) => (
                    <div key={voertuig.id} className="voertuig-item">
                        <p>{voertuig.voertuig.merk} {voertuig.voertuig.type}</p>
                        <p>Type: {voertuig.voertuig.typeVoertuig}</p>
                        <p>Huurder: {voertuig.klant.naam}</p>
                        <p>Van: {voertuig.startDatum} Tot: {voertuig.eindDatum}</p>
                        <button
                            className="export-button"
                            onClick={() => exportToCSV(voertuig)}
                        >
                            Export to CSV
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default VoertuigOverzichtPage;
