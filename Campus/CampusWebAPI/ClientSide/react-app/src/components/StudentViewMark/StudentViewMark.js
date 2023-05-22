import './StudentViewMark.scss';

function StudentViewMark({subjectId}) {
    function setContent(subjectId) {
        switch(subjectId) {
            case '-': 
                return (
                    <p className='chooseParagraph'>
                        Choose any subject to <br/> view your marks
                    </p>
                )
            case 'value1':
                return (
                    <>
                        <p className='mainParagraph'>
                            Subject 1
                        </p>
                        <div className='marksMainDiv'>
                            <div className='marksSubDiv'>
                                <div className='marksStudentViewCell'>MKR 1</div>
                                <div className='marksStudentViewCell'>Lab 1</div>
                                <div className='marksStudentViewCell'>MKR 2</div>
                                <div className='marksStudentViewCell'>Lab 2</div>
                                <div className='marksStudentViewCell'>Lab 3</div>
                            </div>
                            <div className='marksSubDiv'>
                                <div className='marksStudentViewCell'>10</div>
                                <div className='marksStudentViewCell'>5</div>
                                <div className='marksStudentViewCell'>15</div>
                                <div className='marksStudentViewCell'>12</div>
                                <div className='marksStudentViewCell'>3</div>
                            </div>
                        </div>
                        <p className='mainParagraph'>
                            Total score: 45
                        </p>
                    </>
                )
            default: 
                return (
                    <p>Unexpected subjectId</p>
                )
        }
    }

    const content = setContent(subjectId);

    return (
        <div className='mainDivView'>
            {content}
        </div>
    )
}

export default StudentViewMark;