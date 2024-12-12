import React from 'react';
import { BrowserRouter as Router, Route, Routes, useNavigate } from 'react-router-dom';
import Register from './pages/Register';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import Login from './pages/Login';
import './App.css';
import HeaderVDashWPB from './components/HeaderWPB';
import ManageCompanyEmployees from './pages/ManageBedrijfsMedewerkers';
import VoertuigOverzichtPage from './pages/VoertuigOverzichtPage';

const MainPage = () => {
    const navigate = useNavigate();

    const handleRegisterClick = () => {
        navigate('/register');
    };

    const handleLoginClick = () => {
        navigate('/login');
    };




    return (
        <div className="main-page">
            <button className="main-button" onClick={handleRegisterClick}>Registreer</button>
            <button className="main-button" style={{ marginLeft: '10px' }} onClick={handleLoginClick}>Login</button>
        </div>
    );
};

const App = () => {
    return (
        <Router>
            
            <Routes>
                <Route path="/" element={<MainPage />} />
                <Route path="/register" element={<Register />} />
                <Route path="/login" element={<Login />} />
                <Route path="/medewerkers" element={<ManageCompanyEmployees />} />
                <Route path="/voertuigoverzicht" element={<VoertuigOverzichtPage />} />

            </Routes>
            <Footer />
        </Router>
    );
};

export default App;
