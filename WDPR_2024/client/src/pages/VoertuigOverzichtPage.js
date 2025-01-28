import React, { useEffect, useState } from "react";
import "./VoertuigOverzichtPage.css";
import axios from "axios";

const VoertuigOverzichtPage = () => {
    const [voertuigen, setVoertuigen] = useState([]);
    const [filters, setFilters] = useState({
        status: "",
        startDate: "",
        endDate: "",
        merk: "", 
        type: "", 
    });

    useEffect(() => {
        
        fetchVoertuigen();
        const interval = setInterval(fetchVoertuigen, 30000); 
        return () => clearInterval(interval); 
    }, []);

    const fetchVoertuigen = async () => {
        try {
            const response = await axios.get("http://localhost:5000/api/voertuigen/type/auto");
            setVoertuigen(response.data);
        } catch (error) {
            console.error("Error fetching voertuigen:", error);
        }
    };

    const applyFilters = () => {
        return voertuigen.filter((voertuig) => {
            const isStatusMatch = filters.status ? voertuig.status === filters.status : true;
            const isStartDateMatch = filters.startDate ? new Date(voertuig.startDatum) >= new Date(filters.startDate) : true;
            const isEndDateMatch = filters.endDate ? new Date(voertuig.eindDatum) <= new Date(filters.endDate) : true;
            const isMerkMatch = filters.merk ? voertuig.merk.toLowerCase().includes(filters.merk.toLowerCase()) : true;
            const isTypeMatch = filters.type ? voertuig.type.toLowerCase().includes(filters.type.toLowerCase()) : true;

            return isStatusMatch && isStartDateMatch && isEndDateMatch && isMerkMatch && isTypeMatch;
        });
    };

    const handleFilterChange = (e) => {
        const { name, value } = e.target;
        setFilters((prev) => ({ ...prev, [name]: value }));
    };

    const exportToCSV = (voertuig) => {
        const csvContent =
            "data:text/csv;charset=utf-8," +
            ["Voertuig,Type,Status,Huurder,Startdatum,Einddatum"].join(",") +
            "\n" +
            `${voertuig.merk} ${voertuig.type},${voertuig.typeVoertuig},${voertuig.status},${voertuig.huurderNaam},${voertuig.startDatum},${voertuig.eindDatum}`;
        const encodedUri = encodeURI(csvContent);
        const link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute("download", `${voertuig.merk}_${voertuig.type}.csv`);
        document.body.appendChild(link);
        link.click();
    };

    const exportAllToCSV = () => {
        const csvContent =
            "data:text/csv;charset=utf-8," +
            ["Voertuig,Type,Status,Huurder,Startdatum,Einddatum"].join(",") +
            "\n" +
            applyFilters()
                .map(
                    (v) =>
                        `${v.merk} ${v.type},${v.typeVoertuig},${v.status},${v.huurderNaam},${v.startDatum},${v.eindDatum}`
                )
                .join("\n");
        const encodedUri = encodeURI(csvContent);
        const link = document.createElement("a");
        link.setAttribute("href", encodedUri);
        link.setAttribute("download", "voertuigen_overzicht.csv");
        document.body.appendChild(link);
        link.click();
    };

    return (
        <div className="voertuig-overzicht">
            <h2>Voertuig Overzicht</h2>
            <div className="filters">
                <select name="status" value={filters.status} onChange={handleFilterChange}>
                    <option value="">Alle Statussen</option>
                    <option value="Beschikbaar">Beschikbaar</option>
                    <option value="Verhuurd">Verhuurd</option>
                    <option value="In reparatie">In Reparatie</option>
                    <option value="Geblokkeerd">Geblokkeerd</option>
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
                <input
                    type="text"
                    name="merk"
                    value={filters.merk}
                    onChange={handleFilterChange}
                    placeholder="Zoek op Merk"
                />
                <input
                    type="text"
                    name="type"
                    value={filters.type}
                    onChange={handleFilterChange}
                    placeholder="Zoek op Type"
                />
            </div>
            <button onClick={exportAllToCSV} className="export-button">
                Exporteer Alle Voertuigen naar CSV
            </button>
            <div className="voertuig-list">
                {applyFilters().map((voertuig) => (
                    <div key={voertuig.voertuigID} className="voertuig-item">
                        <p>{voertuig.merk} {voertuig.type}</p>
                        <p>Status: {voertuig.status}</p>
                        <p>Huurder: {voertuig.huurderNaam}</p>
                        <p>Van: {voertuig.startDatum} Tot: {voertuig.eindDatum}</p>
                        <button
                            className="export-button"
                            onClick={() => exportToCSV(voertuig)}
                        >
                            Exporteer naar CSV
                        </button>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default VoertuigOverzichtPage;
