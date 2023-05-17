import './Header.scss';
import React from 'react';
import LOGOUT from '../../images/right-from-bracket-solid.svg';
import Profile from '../../images/circle-user-regular.svg';

function Header(){
    return (
        <header className='header'>
            <div className='headerDiv headerDivLeft'>
                <img src={LOGOUT} alt='LogOut'/>
                <p>LOGOUT</p>
            </div>
            <div className='headerDiv headerDivCenter'>
                <p>
                    Campus 3.0
                </p>
            </div>
            <div className='headerDiv headerDivRight'>
                <p>Daniil</p>
                <img src={Profile} alt='profile'/>
            </div>
        </header>
    )
}

export default Header;