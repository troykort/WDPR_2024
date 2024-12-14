import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Register from './pages/Register';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import BusinessRegister from './pages/BusinessRegister';
import BusinessSubscription from './pages/BusinessSubscription'; // Import the new component
import Login from './pages/Login';
import './App.css';
import HeaderWPB from './components/HeaderWPB';
import ManageCompanyEmployees from './pages/ManageBedrijfsMedewerkers';
import VoertuigOverzichtPage from './pages/VoertuigOverzichtPage';

const MainPage = () => {
    return (
        <div className="main-page">
            <h1>Welkom bij Rent-IT!</h1>
            <p>De gemakkelijke manier om auto's, campers en caravans te huren.</p>
            <p className="narrow-paragraph">Bij Rent-IT! maken we voertuigverhuur eenvoudig en toegankelijk. Of je nu een auto, camper of caravan nodig hebt, je kunt het huren zonder gedoe met real-time beschikbaarheid.</p>
            <div className="vehicle-overview">
                <div className="vehicle-box">
                    <h2>Auto's</h2>
                    <p>Bekijk ons aanbod van auto's.</p>
                </div>
                <div className="vehicle-box">
                    <h2>Caravans</h2>
                    <p>Bekijk ons aanbod van caravans.</p>
                </div>
                <div className="vehicle-box">
                    <h2>Campers</h2>
                    <p>Bekijk ons aanbod van campers.</p>
                </div>
            </div>
        </div>
    );
};

const App = () => {
    return (
        <Router>
            <div className="app-container">
                {/*<HeaderWPB />*/}
                <Navbar />
                <div className="main-content">
                    <Routes>
                        <Route path="/" element={<MainPage />} />
                        <Route path="/register" element={<Register />} />
                        <Route path="/business-register" element={<BusinessRegister />}>
                            <Route path="subscriptions" element={<BusinessSubscription />} />
                        </Route>
                        <Route path="/login" element={<Login />} />
                        <Route path="/medewerkers" element={<ManageCompanyEmployees />} />
                        <Route path="/voertuigoverzicht" element={<VoertuigOverzichtPage />} />
                    </Routes>
                </div>
                <Footer />
            </div>
        </Router>
    );
};

export default App;





