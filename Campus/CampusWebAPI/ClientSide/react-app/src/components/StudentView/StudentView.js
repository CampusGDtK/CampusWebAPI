import './StudentView.scss';
import Header from '../Header/Header';
import ListOfSubjects from '../ListOfSubjects/ListOfSubjects';
import StudentViewMark from '../StudentViewMark/StudentViewMark';

function StudentView() {
    return (
        <>
            <Header nameOfPage='view'/>
            <div className='mainSection'>
                <ListOfSubjects />
                <div className='verticalLine'/>
                <StudentViewMark />                
            </div>
        </>
    )
}

export default StudentView;