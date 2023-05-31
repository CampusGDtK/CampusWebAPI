import OneSubject from '../OneSubject/OneSubject';
import './ListOfSubjects.scss';

function ListOfSubjects({chooseSubject, listOfSubjects}) {

    function renderItems(array) {
        const items = array.map(elem => {
            return (<OneSubject 
                key={elem.id} 
                text={elem.name} 
                chooseSubject={() => chooseSubject(elem.id)}/>)
        });

        return (
            <>
                {items}
            </>
        )
    }

    const subjectsContent = renderItems(listOfSubjects)

    return (
        <div className='mainDivList'>
            {subjectsContent}
        </div>
    )
}

export default ListOfSubjects;