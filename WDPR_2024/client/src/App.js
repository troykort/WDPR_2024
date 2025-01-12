import React from 'react';
import { BrowserRouter as Router, Route, Routes, useNavigate, useLocation, Navigate } from 'react-router-dom';
import Register from './pages/Register';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import BusinessRegister from './pages/BusinessRegister';
import BusinessSubscription from './pages/BusinessSubscription';
import Login from './pages/Login';
import './App.css';
import HeaderWPB from './components/HeaderWPB';
import HeaderABO from './components/HeaderABO';
import HeaderParticulier from './components/HeaderParticulier';
import HeaderFrontOffice from './components/HeaderFrontOffice';
import ManageCompanyEmployees from './pages/ManageBedrijfsMedewerkers';
import VoertuigOverzichtPage from './pages/VoertuigOverzichtPage';
import DashboardWPB from './pages/DashboardWPB';
import DashboardABO from './pages/DashboardABO'; 
import DashboardParticulier from './pages/DashboardParticulier';
import DashboardBackoffice from './pages/DashboardBackoffice';
import Voertuigverhuur from './pages/Voertuigverhuur';
import StatistiekenWPB from './pages/StatistiekenWPB';
import AccountSettings from './pages/AccountSettings';
import Privacybeleid from './pages/Privacybeleid';
import ProtectedRoute from './components/ProtectedRoute';
import BOVerhuurAanvragenPage from './pages/BOVerhuurAanvragenPage';
import { getRoleFromToken, getUserIdFromToken } from './utils/authHelpers'; 
import DashboardFrontoffice from './pages/DashboardFrontoffice';
import HeaderBackOffice from './components/HeaderBackOffice';
import FOVerhuurAanvragenPage from './pages/FOVerhuurAanvragenPage';

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

const ProfielPage = () => {
    const role = getRoleFromToken();

    return (
        <>
            {role === 'Particulier' && <HeaderParticulier />}
            {role === 'Backoffice' && <HeaderBackOffice />}
            {role === 'Wagenparkbeheerder' && <HeaderWPB />}
            <AccountSettings />
        </>
    );
};

const App = () => {
    const location = useLocation();
    const isDashboardWPB = location.pathname.startsWith('/dashboardwpb') || location.pathname.startsWith('/medewerkers') || location.pathname.startsWith('/voertuigoverzicht') || location.pathname.startsWith('/statistieken') || location.pathname.startsWith('/profiel');
    const isDashboardABO = location.pathname.startsWith('/dashboardabo');
    const isDashboardParticulier = location.pathname.startsWith('/dashboardparticulier');
    const isVoertuigverhuur = location.pathname.startsWith('/voertuigverhuur');
    const isDashboardBackoffice = location.pathname.startsWith('/backoffice-dashboard');
    const isDashboardFrontoffice = location.pathname.startsWith('/frontoffice-dashboard');

    return (
        <div className="app-container">
            {!isDashboardWPB && !isDashboardFrontoffice && !isDashboardBackoffice && !isDashboardABO && !isDashboardParticulier && !isVoertuigverhuur && <Navbar />}
            <div className="main-content">
                <Routes>
                    <Route path="/" element={<MainPage />} />
                    <Route path="/register" element={<Register />} />
                    <Route path="/business-register" element={<BusinessRegister />}>
                        <Route path="subscriptions" element={<BusinessSubscription />} />
                    </Route>
                    <Route path="/login" element={<Login />} />
                    <Route path="/medewerkers" element={
                        <ProtectedRoute allowedRoles={["Backoffice", "Abonnementbeheerder"]}>
                            <HeaderWPB />
                            <ManageCompanyEmployees />
                        </ProtectedRoute>
                    } />
                    <Route path="/voertuigoverzicht" element={
                        <ProtectedRoute allowedRoles={["Wagenparkbeheerder"]}>
                            <HeaderWPB />
                            <VoertuigOverzichtPage />
                        </ProtectedRoute>
                    } />
                    <Route path="/dashboardwpb" element={
                        <ProtectedRoute allowedRoles={["Wagenparkbeheerder", "Backoffice"]}>
                            <HeaderWPB />
                            <DashboardWPB />
                        </ProtectedRoute>
                    } />
                    <Route path="/statistieken" element={
                        <ProtectedRoute allowedRoles={["Wagenparkbeheerder", "Backoffice"]}>
                            <HeaderWPB />
                            <StatistiekenWPB />
                        </ProtectedRoute>
                    } />
                    <Route path="/profiel" element={
                        <ProtectedRoute allowedRoles={["Wagenparkbeheerder", "Backoffice", "Particulier", "Frontoffice", "Abonnementbeheerder", "Zakelijk"]}>
                            <ProfielPage />
                        </ProtectedRoute>
                    } />
                    <Route path="/dashboardabo" element={
                        <ProtectedRoute allowedRoles={["Abonnementbeheerder"]}>
                            <HeaderABO />
                            <DashboardABO />
                        </ProtectedRoute>
                    } />
                    <Route path="/dashboardparticulier" element={
                        <ProtectedRoute allowedRoles={["Particulier"]}>
                            <HeaderParticulier />
                            <DashboardParticulier />
                        </ProtectedRoute>
                    } />
                    <Route path="/voertuigverhuur" element={
                        <ProtectedRoute allowedRoles={["Particulier"]}>
                            <HeaderParticulier />
                            <Voertuigverhuur />
                        </ProtectedRoute>
                    } />
                    <Route path="/backoffice-dashboard" element={
                        <ProtectedRoute allowedRoles={["Backoffice"]}>
                            <HeaderBackOffice />
                            <BOVerhuurAanvragenPage />
                        </ProtectedRoute>
                    } /><Route path="/frontoffice-dashboard" element={
                        <ProtectedRoute allowedRoles={["Frontoffice"]}>
                            <HeaderFrontOffice />
                            <DashboardFrontoffice />
                        </ProtectedRoute>
                    } /><Route path="/FO-verhuuraanvragen" element={
                        <ProtectedRoute allowedRoles={["Frontoffice"]}>
                            <HeaderFrontOffice />
                            <FOVerhuurAanvragenPage />
                        </ProtectedRoute>
                    } />


                    <Route path="/privacybeleid" element={<Privacybeleid />} />
                    <Route path="/unauthorized" element={<h2>Toegang geweigerd</h2>} />
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
