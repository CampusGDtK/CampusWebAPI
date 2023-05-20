import './OneSubject.scss';

function OneSubject({value, text}) {
    return (
        <p className='oneSubjectParagraph' data-value={value}>
            {text}
        </p>
    )
}

export default OneSubject;