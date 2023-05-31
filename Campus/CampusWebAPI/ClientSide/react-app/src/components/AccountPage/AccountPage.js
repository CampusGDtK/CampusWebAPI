import Header from '../Header/Header';
import Profile from '../../images/circle-user-regular.svg';
import './AccountPage.scss';

function AccountPage({userData}) {
    return (
        <>
            <Header nameOfPage='profile'/>
            <div className='profilePageMainDiv'>
                <div className='profilePageWrapper'>
                    <img src={Profile} alt='profile'/>
                    <p>{userData.fullName}</p>
                    <p>Group: {userData.groupName}</p>
                    <p>Email: {userData.email}</p>
                    <p>Phone number: {userData.phoneNumber}</p>
                    <p>Date of birth: {userData.dateOfBirth}</p>
                </div>
            </div>
        </>
    )
}

export default AccountPage;