import './AcademicView.scss';
import { useState, useEffect } from 'react';
import Header from '../Header/Header';
import ListOfSubjects from '../ListOfSubjects/ListOfSubjects';
import AcademicViewGroups from '../AcademicViewGroups/AcademicViewGroups';
import Spinner from '../Spinner/Spinner';
import Close from '../../images/close.svg';

function AcademicView ({userId, token, changeUserData, userData}) {
    const [subjectId, setSubjectId] = useState('-');
    const [listOfSubjects, setListOfSubjects] = useState();
    const [groupId, setGroupId] = useState();

    useEffect(() => {
        async function getUserData () {
            const resp = await fetch(`http://localhost:5272/api/academics/${userId}`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            const data = await resp.json();
            console.log(data);
            changeUserData(data);
            return data;
        }

        async function getListOfSubjects () {
            const resp = await fetch(`http://localhost:5272/api/academics/${userId}/disciplines`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            const data = await resp.json();
            const resultData = [...new Map(data.map(item =>
                [item['name'], item])).values()];
            console.log(resultData);
            setListOfSubjects(resultData)
            return resultData;
        }

        getListOfSubjects()
        .catch(error => console.error(error))

        getUserData()
        .catch(error => console.error(error))
    }, [])


    function chooseSubject(id) {
        setSubjectId(id)
    }

    function changeGroupId(id) {
        setGroupId(id);
    }

    function changeModalVisibility () {
        const modal = document.querySelector('.modalWrapper');
        if (modal.style.display === 'block') modal.style = 'display: none;';
        else modal.style = 'display: block;';
    }

    const content = userData && listOfSubjects ?
    <>
        <Header nameOfPage='view' title={userData.name} role={'Academic'} sex={userData.gender}/>
        <div className='mainSection'>
            <ListOfSubjects chooseSubject={chooseSubject} listOfSubjects={listOfSubjects}/>
            <div className='verticalLine'/>
            <AcademicViewGroups 
            subjectId={subjectId} 
            academicId={userId} 
            token={token} 
            changeGroupId={changeGroupId} 
            changeModalVisibility={changeModalVisibility}/>                
        </div>
        <div className='modalWrapper'>
            <div className='mainModalWindow'>
                <img src={Close} alt='close' className='closeImg' onClick={changeModalVisibility}/>
            </div>
        </div>
        {/* <div className='modalWrapper'>
            <div className='subModalWindow'>
                
            </div>
        </div> */}
    </> : <Spinner />;

    return (
        <>
            {content}
        </>
    )
}

export default AcademicView;