import { useEffect, useState } from 'react';
import './StudentViewMark.scss';
import Spinner from '../Spinner/Spinner';

function StudentViewMark({subjectId, studentId, token}) {
    const [marks, setMarks] = useState();
 
    useEffect(() => {
        async function getUserMarks(subjectId) {
            if (subjectId === '-') {
                setMarks('-');
            } else {
                const resp = await fetch(`http://localhost:5272/api/students/${studentId}/marks/${subjectId}`, {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`
                    }
                });
                const data = await resp.json();
                console.log(data);
                setMarks(data);
                
            }
        }

        getUserMarks(subjectId)
        .catch(error => console.error(error));
    }, [subjectId])

    const content = marks ?
                    marks === '-' ?
                    <p className='chooseParagraph'>
                        Choose any subject to <br/> view your marks
                    </p> :
                    <>
                        <p className='mainParagraph'>
                            {marks.disciplineName}
                        </p>
                        <div className='marksMainDiv'>
                            <div className='marksSubDiv'>
                                {marks.details.map((element, index) => {
                                    return (
                                        <div key={index} className='marksStudentViewCell'>{element}</div>
                                    )
                                })}
                            </div>
                            <div className='marksSubDiv'>
                                {marks.marks.map((element, index) => {
                                    return (
                                        <div key={index} className='marksStudentViewCell'>{element}</div>
                                    )
                                })}
                            </div>
                        </div>
                        <p className='mainParagraph'>
                            Total score: {marks.totalMark}
                        </p>
                    </>
                : <Spinner/>;

    return (
        <div className='mainDivView'>
            {content}
        </div>
    )
}

export default StudentViewMark;