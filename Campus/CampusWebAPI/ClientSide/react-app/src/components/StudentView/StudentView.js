import './StudentView.scss';
import Header from '../Header/Header';
import ListOfSubjects from '../ListOfSubjects/ListOfSubjects';
import StudentViewMark from '../StudentViewMark/StudentViewMark';
import Spinner from '../Spinner/Spinner';
import { useState, useEffect } from 'react';

function StudentView({userId, token, changeUserData, userData}) {
    const [subjectId, setSubjectId] = useState('-');
    const [listOfSubjects, setListOfSubjects] = useState();

    useEffect(() => {
        async function getUserData () {
            const resp = await fetch(`http://localhost:5272/api/students/${userId}`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            const data = await resp.json();
            console.log('useeff');
            changeUserData(data);
            return data;
        }

        async function getListOfSubjects () {
            const resp = await fetch(`http://localhost:5272/api/students/${userId}/disciplines`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            const data = await resp.json();
            console.log(data);
            setListOfSubjects(data)
            return data;
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
        <Header nameOfPage='view' title={userData.fullName}/>
        <div className='mainSection'>
            <ListOfSubjects chooseSubject={chooseSubject} listOfSubjects={listOfSubjects}/>
            <div className='verticalLine'/>
            <StudentViewMark subjectId={subjectId} studentId={userId} token={token}/>                
        </div>
    </> : <Spinner />;

    return (
        <>
            {content}
        </>
    )
}

export default StudentView;