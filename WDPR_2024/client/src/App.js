import React from 'react';
import { BrowserRouter as Router, Route, Routes, useNavigate, useLocation } from 'react-router-dom';
import Register from './pages/Register';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import BusinessRegister from './pages/BusinessRegister';
import BusinessSubscription from './pages/BusinessSubscription';
import Login from './pages/Login';
import './App.css';
import HeaderWPB from './components/HeaderWPB';
import ManageCompanyEmployees from './pages/ManageBedrijfsMedewerkers';
import VoertuigOverzichtPage from './pages/VoertuigOverzichtPage';
import DashboardWPB from './pages/DashboardWPB';
import StatistiekenWPB from './pages/StatistiekenWPB';
import ProfielWPB from './pages/ProfielWPB';
import Privacybeleid from './pages/Privacybeleid';
import AccountSettings  from './pages/AccountSettings';

const MainPage = () => {
    const navigate = useNavigate();

    const handleGoToDashboardWPB = () => {
        navigate('/dashboardwpb');
    };

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
            <button onClick={handleGoToDashboardWPB} className="btn dashboardwpb-btn" style={{ marginTop: '20px', backgroundColor: '#4c1a2a', color: 'white', padding: '10px 20px', border: 'none', borderRadius: '5px', cursor: 'pointer' }}>
                Ga naar Dashboard WPB
            </button>
        </div>
    );
};

const App = () => {
    const location = useLocation();
    const isDashboardWPB = location.pathname.startsWith('/dashboardwpb') || location.pathname.startsWith('/medewerkers') || location.pathname.startsWith('/voertuigoverzicht') || location.pathname.startsWith('/statistieken') || location.pathname.startsWith('/profiel');

    return (
        <div className="app-container">
            {!isDashboardWPB && <Navbar />}
            <div className="main-content">
                <Routes>
                    <Route path="/" element={<MainPage />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/business-register" element={<BusinessRegister />}>
                        <Route path="subscriptions" element={<BusinessSubscription />} />
                    </Route>
                    <Route path="/login" element={<Login />} />
                    <Route path="/medewerkers" element={<><HeaderWPB /><ManageCompanyEmployees /></>} />
                    <Route path="/voertuigoverzicht" element={<><HeaderWPB /><VoertuigOverzichtPage /></>} />
                    <Route path="/dashboardwpb" element={<><HeaderWPB /><DashboardWPB /></>} />
                    <Route path="/statistieken" element={<><HeaderWPB /><StatistiekenWPB /></>} />
                    <Route path="/profiel" element={<><HeaderWPB /><ProfielWPB /></>} />
                    <Route path="/privacybeleid" element={<Privacybeleid />} />
                    <Route path="/accountsettings"element={<AccountSettings />} />

                </Routes>
            </div>
            <Footer />
        </div>
    );
};

const AppWrapper = () => (
    <Router>
        <App />
    </Router>
);

export default AppWrapper;

