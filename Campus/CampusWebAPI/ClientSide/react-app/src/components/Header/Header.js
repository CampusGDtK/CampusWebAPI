import React from 'react';
import {Link} from 'react-router-dom';
import './Header.scss';
import LOGOUT from '../../images/right-from-bracket-solid.svg';
import Profile from '../../images/circle-user-regular.svg';
import Back from '../../images/back.svg';

function Header({nameOfPage, role, sex, title, routeBack}) {
    const prep = role === 'Academic' ? sex === 'Male' ? 'Mr. ' : 'Mrs. ' : '';

    function chooseButton (nameOfPage) {
        switch (nameOfPage) {
            case 'view':
                return (
                    <>
                        <Link to='/profile'>
                            <p>{prep}{title}</p>
                        </Link>
                        <Link to='/profile'>
                            <img src={Profile} alt='profile' />
                        </Link>
                    </>
                )
            case 'profile':
                return (
                    <>
                        <Link to={routeBack}>
                            <p>Back</p>
                        </Link>
                        <Link to={routeBack}>
                            <img src={Back} alt='back'/>
                        </Link>
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
                <Link to='/'>
                    <img src={LOGOUT} alt='LogOut'/>                
                </Link>
                <Link to='/'>
                    <p>LOGOUT</p>
                </Link>
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