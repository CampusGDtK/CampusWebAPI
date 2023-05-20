import './Header.scss';
import React from 'react';
import LOGOUT from '../../images/right-from-bracket-solid.svg';
import Profile from '../../images/circle-user-regular.svg';
import Back from '../../images/back.svg';

function Header({nameOfPage, role, sex}) {
    const title = role === 'academic' ? sex === 'male' ? 'Mr. ' : 'Mrs. ' : '';

    function chooseButton (nameOfPage) {
        switch (nameOfPage) {
            case 'view':
                return (
                    <>
                        <p>{title}Daniil</p>
                        <img src={Profile} alt='profile'/>
                    </>
                )
            case 'profile':
                return (
                    <>
                        <p>Back</p>
                        <img src={Back} alt='back'/>
                    </>
                )
        
            default:
                return "Something went wrong!";
        }
    }

    const content = chooseButton(nameOfPage);

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
                {content}
            </div>
        </header>
    )
}

export default Header;