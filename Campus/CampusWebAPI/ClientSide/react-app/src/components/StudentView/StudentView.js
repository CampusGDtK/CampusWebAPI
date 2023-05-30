import './StudentView.scss';
import Header from '../Header/Header';
import ListOfSubjects from '../ListOfSubjects/ListOfSubjects';
import StudentViewMark from '../StudentViewMark/StudentViewMark';
import { useState, useEffect } from 'react';

function StudentView({userId, token}) {
    const [subjectId, setSubjectId] = useState('-');

    useEffect(() => {
        fetch(`http://localhost:5272/api/students/${userId}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`
            }
        })
    }, [])

    function chooseSubject(id) {
        setSubjectId(id)
    }

    return (
        <>
            <Header nameOfPage='view'/>
            <div className='mainSection'>
                <ListOfSubjects chooseSubject={chooseSubject}/>
                <div className='verticalLine'/>
                <StudentViewMark subjectId={subjectId}/>                
            </div>
        </>
    )
}

export default StudentView;