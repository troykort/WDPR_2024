import React from 'react';
import { BrowserRouter as Router, Route, Routes, useNavigate } from 'react-router-dom';
import Register from './pages/Register';
import Navbar from './components/Navbar';
import './App.css';

const MainPage = () => {
    const navigate = useNavigate();

    const handleClick = () => {
        navigate('/register');
    };

    return (
        <div className="main-page">
            <button className="main-button" onClick={handleClick}>Registreer</button>
        </div>
    );
};

const App = () => {
    return (
        <Router>
            <Navbar />
            <Routes>
                <Route path="/" element={<MainPage />} />
                <Route path="/register" element={<Register />} />
            </Routes>
        </Router>
    );
};

export default App;
