import Header from '../Header/Header';
import Profile from '../../images/circle-user-regular.svg';
import './AccountPage.scss';

function AccountPage({userData, role}) {
    return (
        <>
            <Header nameOfPage='profile' routeBack={role === 'Academic' ? '/academic-view' : '/student-view'}/>
            <div className='profilePageMainDiv'>
                <div className='profilePageWrapper'>
                    <img src={Profile} alt='profile'/>
                    <p>{role === 'Academic' ? userData.name : userData.fullName}</p>
                    {role === 'Academic' ? 
                    <p>Position: {userData.position}</p> :
                    <p>Group: {userData.groupName}</p>}
                    <p>Email: {userData.email}</p>
                    <p>Phone number: {userData.phoneNumber}</p>
                    {role === 'Academic' ? null :
                    <p>Date of birth: {userData.dateOfBirth}</p>}
                </div>
            </div>
        </>
    )
}

export default AccountPage;