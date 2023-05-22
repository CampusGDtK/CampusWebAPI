import OneSubject from '../OneSubject/OneSubject';
import './ListOfSubjects.scss';

function ListOfSubjects({chooseSubject}) {
    const arrayOfSubjects = [
        {
            text: "subject 1",
            value: "value1"
        },
        {
            text: "subject 2",
            value: "value2"
        },
        {
            text: "subject 3",
            value: "value3"
        },
        {
            text: "subject 4",
            value: "value4"
        },
        {
            text: "subject 5",
            value: "value5"
        }
    ];

    function renderItems(array) {
        const items = array.map(elem => {
            return (<OneSubject 
                key={elem.value} 
                text={elem.text} 
                chooseSubject={() => chooseSubject(elem.value)}/>)
        });

        return (
            <>
                {items}
            </>
        )
    }

    const subjectsContent = renderItems(arrayOfSubjects)

    return (
        <div className='mainDivList'>
            {subjectsContent}
        </div>
    )
}

export default ListOfSubjects;