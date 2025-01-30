import React from 'react';
import './Privacybeleid.css';

const Privacybeleid = () => {
    return (
        <div className="privacybeleid-container">
            <h1>Privacybeleid Rent-IT</h1>
            <p>
                Bij CarAndAll hechten we veel waarde aan uw privacy en de bescherming van uw persoonlijke gegevens. Dit privacybeleid beschrijft hoe wij omgaan met uw gegevens, waarom we deze verzamelen, hoe ze worden verwerkt en welke rechten u heeft.
            </p>
            <section>
                <h2>1. Verzamelde Gegevens</h2>
                <p>Bij het gebruik van onze applicatie kunnen we de volgende gegevens van u verzamelen:</p>
                <ul>
                    <li><strong>Persoonsgegevens:</strong> Naam, adres, e-mailadres, telefoonnummer.</li>
                    <li><strong>Verhuurgegevens:</strong> Geselecteerde voertuigen, huurdata, reisbestemmingen, verwachte kilometers.</li>
                    <li><strong>Zakelijke gegevens:</strong> Bedrijfsnaam, adres, KVK-nummer (voor zakelijke klanten).</li>
                    <li><strong>Authenticatiegegevens:</strong> Gebruikersnaam, wachtwoorden (versleuteld opgeslagen).</li>
                    <li><strong>Betalingsinformatie:</strong> Gegevens die nodig zijn voor transacties (niet zichtbaar voor ons, verwerkt via een beveiligde betalingsprovider).</li>
                </ul>
            </section>
            <section>
                <h2>2. Doeleinden van Verzameling</h2>
                <p>De gegevens worden verzameld voor:</p>
                <ul>
                    <li>Het mogelijk maken van verhuur en reserveringen.</li>
                    <li>Het beheren van gebruikersaccounts en abonnementen.</li>
                    <li>Het versturen van huurbevestigingen, notificaties en belangrijke updates.</li>
                    <li>Het verbeteren van onze diensten door middel van statistische analyses.</li>
                </ul>
            </section>
            <section>
                <h2>3. Bewaartermijn</h2>
                <p>Uw gegevens worden niet langer bewaard dan noodzakelijk is voor de bovengenoemde doeleinden:</p>
                <ul>
                    <li>Persoonsgegevens worden bewaard zolang uw account actief is of zolang wettelijk verplicht.</li>
                    <li>Verhuur- en betalingsgegevens worden bewaard conform fiscale en administratieve verplichtingen.</li>
                </ul>
            </section>
            <section>
                <h2>4. Gegevensbeveiliging</h2>
                <p>Wij nemen passende technische en organisatorische maatregelen om uw gegevens te beschermen tegen verlies, misbruik en ongeautoriseerde toegang. Dit omvat:</p>
                <ul>
                    <li>Het gebruik van <strong>HTTPS</strong>-versleuteling voor datatransmissie.</li>
                    <li>Versleutelde opslag van wachtwoorden.</li>
                    <li>Regelmatige beveiligingsaudits en updates.</li>
                </ul>
            </section>
            <section>
                <h2>5. Delen van Gegevens</h2>
                <p>Uw gegevens worden alleen gedeeld met derden indien noodzakelijk:</p>
                <ul>
                    <li><strong>Betalingsverwerkers:</strong> Voor veilige transacties.</li>
                    <li><strong>Overheidsinstanties:</strong> Alleen indien wettelijk verplicht.</li>
                </ul>
            </section>
            <section>
                <h2>6. Rechten van Gebruikers</h2>
                <p>U heeft het recht om:</p>
                <ul>
                    <li>Inzage te krijgen in uw gegevens.</li>
                    <li>Uw gegevens te corrigeren of te laten verwijderen.</li>
                    <li>Bezwaar te maken tegen verwerking van uw gegevens.</li>
                    <li>Uw gegevens over te dragen aan een andere dienstverlener (dataportabiliteit).</li>
                </ul>
            </section>
            <section>
                <h2>7. Cookies</h2>
                <p>Onze applicatie maakt gebruik van functionele cookies om het gebruik te optimaliseren. Analytische cookies worden gebruikt om het verkeer te analyseren. Voor cookies waarvoor toestemming nodig is, vragen we expliciet uw akkoord.</p>
            </section>
            <section>
                <h2>8. Contact</h2>
                <p>Voor vragen over ons privacybeleid of om uw rechten uit te oefenen, kunt u contact opnemen met:</p>
                <ul>
                    <li><strong>CarAndAll Klantenservice</strong></li>
                    <li>E-mail: <a href="mailto:support@carandall.com">support@carandall.com</a></li>
                    <li>Telefoon: <a href="tel:+31201234567">+31 20 123 4567</a></li>
                </ul>
            </section>
            <p>Dit privacybeleid kan periodiek worden bijgewerkt. Wij raden u aan om het beleid regelmatig te bekijken voor de laatste informatie.</p>
        </div>
    );
};

export default Privacybeleid;



