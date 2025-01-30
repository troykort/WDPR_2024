import React, { useState, useEffect } from "react";
import axios from "axios";
import "./ManageBedrijfsMedewerkers.css";

const EmployeePage = () => {
    const [employees, setEmployees] = useState([]);
    const [filteredEmployees, setFilteredEmployees] = useState([]);
    const [domain, setDomain] = useState("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [filter, setFilter] = useState("");

    // Haal het domein van de wagenparkbeheerder op
    useEffect(() => {
        const fetchDomain = async () => {
            try {
                const response = await axios.get("http://localhost:5000/api/klanten/me", {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                });
                const email = response.data.email;
                const emailDomain = email.substring(email.indexOf("@"));
                setDomain(emailDomain);
            } catch (err) {
                console.error("Error fetching beheerder details:", err);
                setError("Kan domein niet ophalen. Probeer opnieuw.");
            } finally {
                setLoading(false);
            }
        };

        fetchDomain();
    }, []);

    // Haal medewerkers op
    useEffect(() => {
        if (domain) {
            const fetchEmployees = async () => {
                try {
                    const response = await axios.get(
                        `http://localhost:5000/api/klanten`,
                        {
                            params: { emailDomain: domain },
                            headers: {
                                Authorization: `Bearer ${localStorage.getItem("token")}`,
                            },
                        }
                    );
                    setEmployees(response.data);
                    setFilteredEmployees(response.data);
                } catch (err) {
                    console.error("Error fetching employees:", err);
                    setError("Kan medewerkers niet ophalen. Probeer opnieuw.");
                }
            };

            fetchEmployees();
        }
    }, [domain]);

    // Voeg een medewerker toe
    const addEmployee = async () => {
        const email = prompt("Voer het e-mailadres in van de medewerker:");
        if (email && email.endsWith(domain)) {
            try {
                const nieuweMedewerker = {
                    naam: "Onbekend",
                    adres: "Onbekend",
                    telefoonnummer: "0000000000",
                    email,
                    wachtwoord: "default123",
                };
                await axios.post(`http://localhost:5000/api/klanten`, nieuweMedewerker, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`,
                    },
                });
                setEmployees((prev) => [...prev, nieuweMedewerker]);
                setFilteredEmployees((prev) => [...prev, nieuweMedewerker]);
                alert("Medewerker toegevoegd!");
            } catch (err) {
                console.error("Error adding employee:", err);
                alert("Kan medewerker niet toevoegen. Probeer opnieuw.");
            }
        } else {
            alert(`Ongeldig e-mailadres. Alleen e-mails met domein ${domain} zijn toegestaan.`);
        }
    };

    // Verwijder een medewerker
    const removeEmployee = async (email) => {
        const bevestigen = window.confirm("Weet u zeker dat u deze medewerker wilt verwijderen?");
        if (bevestigen) {
            try {
                const medewerker = employees.find((e) => e.email === email);
                if (medewerker && medewerker.id) {
                    await axios.delete(`http://localhost:5000/api/klanten/${medewerker.id}`, {
                        headers: {
                            Authorization: `Bearer ${localStorage.getItem("token")}`,
                        },
                    });
                    setEmployees((prev) => prev.filter((e) => e.email !== email));
                    setFilteredEmployees((prev) => prev.filter((e) => e.email !== email));
                    alert("Medewerker verwijderd!");
                } else {
                    alert("Medewerker niet gevonden.");
                }
            } catch (err) {
                console.error("Error removing employee:", err);
                alert("Kan medewerker niet verwijderen. Probeer opnieuw.");
            }
        }
    };

    // Filter medewerkers
    useEffect(() => {
        setFilteredEmployees(
            employees.filter((employee) =>
                employee.email.toLowerCase().includes(filter.toLowerCase())
            )
        );
    }, [filter, employees]);

    // Laadstatus en foutmeldingen weergeven
    if (loading) return <p>Gegevens laden...</p>;
    if (error) return <p>{error}</p>;

    return (
        <main className="employee-page">
            <div className="add-employee">
                <h2>Medewerkers</h2>
                <button className="toevoegen-button" onClick={addEmployee} aria-label="Medewerker toevoegen">
                    Toevoegen
                </button>
                <label htmlFor="filter" className="sr-only">Filter medewerkers</label>
                <input
                    type="text"
                    id="filter"
                    placeholder="Filter medewerkers"
                    value={filter}
                    onChange={(e) => setFilter(e.target.value)}
                    className="filter-input"
                />
            </div>
            <div className="employee-list" role="list">
                {filteredEmployees.map((employee) => (
                    <div key={employee.email} className="employee-item" role="listitem">
                        <span>{employee.email}</span>
                        <button onClick={() => removeEmployee(employee.email)} aria-label={`Verwijder medewerker ${employee.email}`}>
                            Verwijderen
                        </button>
                    </div>
                ))}
            </div>
        </main>
    );
};

export default EmployeePage;




