import './OneSubject.scss';

function OneSubject({value, text, chooseSubject}) {
    return (
        <p className='oneSubjectParagraph' data-value={value} onClick={chooseSubject}>
            {text}
        </p>
    )
}

export default OneSubject;