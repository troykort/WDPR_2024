import React from 'react';
import './Support.css';

const Support = () => {
    return (
        <div className="support-container">
            <h1>Ondersteuning</h1>
            <p>Welkom bij de ondersteuningspagina van Rent-IT. We staan klaar om je te helpen met al je vragen of problemen.</p>

            <h2>Hoe kunnen we je helpen?</h2>
            <p>Of je nu hulp nodig hebt bij het reserveren van een voertuig, je accountinstellingen wilt wijzigen, of technische problemen ondervindt, ons supportteam is hier om je te ondersteunen.</p>

            <h2>Contactinformatie</h2>
            <p>Neem contact op via de onderstaande manieren:</p>
            <ul>
                <li><strong>E-mail:</strong> <a href="mailto:support@rentit.com">support@rentit.com</a></li>
                <li><strong>Telefoon:</strong> <a href="tel:+31201234567">+31 20 123 4567</a></li>
                <li><strong>Adres:</strong> Rent-IT, Straatnaam 123, 1011 AB, Amsterdam</li>
            </ul>

            <h2>Veelgestelde Vragen</h2>
            <p>Bezoek onze <a href="/faq">Veelgestelde Vragen (FAQ)</a> pagina voor snelle antwoorden op veel voorkomende vragen.</p>

            <h2>Hulp nodig met je account?</h2>
            <p>Als je problemen hebt met je account of inloggegevens, kun je de <a href="/profiel">accountinstellingen</a> raadplegen om je gegevens bij te werken of je wachtwoord opnieuw in te stellen.</p>

            <h2>Technische Ondersteuning</h2>
            <p>Als je technische problemen ondervindt, neem dan contact op met onze klantenservice via e-mail of telefoon. Zorg ervoor dat je zoveel mogelijk details verstrekt, zodat we je snel kunnen helpen.</p>

            <h2>Openingstijden</h2>
            <p>Ons supportteam is beschikbaar van maandag tot vrijdag van 9:00 tot 18:00 uur. We doen ons best om alle vragen zo snel mogelijk te beantwoorden.</p>
        </div>
    );
};

export default Support;

