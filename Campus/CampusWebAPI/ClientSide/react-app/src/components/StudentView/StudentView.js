import './StudentView.scss';
import Header from '../Header/Header';
import ListOfSubjects from '../ListOfSubjects/ListOfSubjects';
import StudentViewMark from '../StudentViewMark/StudentViewMark';
import { useState } from 'react';

function StudentView() {
    const [subjectId, setSubjectId] = useState('-');

    function chooseSubject(id) {
        console.log(id)
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