import './AcademicView.scss';
import { useState, useEffect } from 'react';
import Header from '../Header/Header';
import ListOfSubjects from '../ListOfSubjects/ListOfSubjects';
import AcademicViewGroups from '../AcademicViewGroups/AcademicViewGroups';
import Spinner from '../Spinner/Spinner';

function AcademicView ({userId, token, changeUserData, userData}) {
    const [subjectId, setSubjectId] = useState('-');
    const [listOfSubjects, setListOfSubjects] = useState();

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

    const content = userData && listOfSubjects ?
    <>
        <Header nameOfPage='view' title={userData.name} role={'Academic'} sex={userData.gender}/>
        <div className='mainSection'>
            <ListOfSubjects chooseSubject={chooseSubject} listOfSubjects={listOfSubjects}/>
            <div className='verticalLine'/>
            <AcademicViewGroups subjectId={subjectId} academicId={userId} token={token}/>                
        </div>
    </> : <Spinner />;

    return (
        <>
            {content}
        </>
    )
}

export default AcademicView;