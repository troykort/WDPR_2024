import React from 'react';
import './SubscriptionDetails.css';

const SubscriptionDetails = () => {
    return (
        <div className="subscription-details">
            <h2>Abonnementen</h2>
            <section className="subscription">
                <h3>Pay-as-you-go-abonnement</h3>
                <p>Omschrijving: Klanten betalen een vast maandelijks bedrag voor toegang tot de dienst, met een korting op huurprijzen per voertuig.</p>
                <ul>
                    <li>Maandelijks abonnementskosten: €50 - €100</li>
                    <li>Korting op voertuighuur: 10% - 20%</li>
                    <li>Toeslag voor premium voertuigen (optioneel): €20 - €50 per huurperiode</li>
                </ul>
                <p>Voordelen:</p>
                <ul>
                    <li>Flexibiliteit: geen vooraf bepaalde huurhoeveelheid.</li>
                    <li>Geschikt voor bedrijven met onregelmatige behoeften.</li>
                </ul>
            </section>
            <section className="subscription">
                <h3>Prepaid-abonnement (op basis van huurdagen)</h3>
                <p>Omschrijving: Bedrijven betalen vooraf voor een bepaald aantal huurdagen per jaar.</p>
                <ul>
                    <li>30 dagen/jaar: €1.500</li>
                    <li>60 dagen/jaar: €2.800</li>
                    <li>100 dagen/jaar: €4.500</li>
                </ul>
                <p>Overgebruik:</p>
                <ul>
                    <li>Extra dagen worden gefactureerd tegen een standaardtarief, bijvoorbeeld €50 - €70 per dag.</li>
                </ul>
                <p>Voordelen:</p>
                <ul>
                    <li>Kostenbesparing bij regelmatig gebruik.</li>
                    <li>Ideaal voor bedrijven met voorspelbare huurbehoeften.</li>
                </ul>
            </section>
        </div>
    );
};

export default SubscriptionDetails;


