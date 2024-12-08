import React from 'react';
import ReactDOM from 'react-dom';
import './HeaderV.DashWPB.css';
import logo from '../assets/Logo.png';




const HeaderVDashWPB = () => {
    return (
        <header>
            <button aria-label="Home">
                <img src={logo} alt="Car And All logo"></img>
            </button>
            <nav>
                <ul>
                    <li>
                        <a href="#">Voertuigen</a>
                    </li>
                    <li>
                        <a href="#">Huurders</a>
                    </li>
                    <li>
                        <a href="#">Schademeldingen</a>
                    </li>
               </ul>
           </nav>
        </header>
    );
}

export default HeaderVDashWPB;