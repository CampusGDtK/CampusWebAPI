import Header from '../Header/Header';
import Profile from '../../images/circle-user-regular.svg';
import './AccountPage.scss';

function AccountPage() {
    return (
        <>
            <Header nameOfPage='profile'/>
            <div className='profilePageMainDiv'>
                <div className='profilePageWrapper'>
                    <img src={Profile} alt='profile'/>
                    <p>Daniil Dziubenko</p>
                    <p>Group: IP-15</p>
                    <p>Email: freekick2017uk@gmail.com</p>
                    <p>Phone number: +380996601848</p>
                    <p>Date of birth: 04-10-2003</p>
                </div>
            </div>
        </>
    )
}

export default AccountPage;