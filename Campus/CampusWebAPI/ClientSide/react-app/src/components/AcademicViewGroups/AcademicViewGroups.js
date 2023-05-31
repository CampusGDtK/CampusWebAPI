import { useEffect, useState } from 'react';
import './AcademicViewGroups.scss';
import Spinner from '../Spinner/Spinner';

function AcademicViewGroups ({subjectId, academicId, token, changeGroupId, changeModalVisibility}) {
    const [groups, setGroups] = useState();
    const [disciplineName, setDisciplineName] = useState();

    useEffect(() => {
        async function getGroups () {
            if (subjectId === '-') {
                setGroups('-');
            } else {
                const resp = await fetch(`http://localhost:5272/api/academics/${academicId}/disciplines/${subjectId}/groups`, {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`
                    }
                });
                const data = await resp.json();
                console.log(data);
                setGroups(data);
            }
        }

        async function getNameOfDiscipline () {
            const resp = await fetch(`http://localhost:5272/api/disciplines/${subjectId}`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`
                }
            })
            const data = await resp.json();
            console.log(data);
            setDisciplineName(data.name);
        }
        
        getNameOfDiscipline()
        .catch(error => console.log(error));

        getGroups()
        .catch(error => console.log(error));

    }, [subjectId])

    

    const content = groups ?
                    groups === '-' ?
                    <p className='chooseParagraph'>
                        Choose any of your subjects <br/> to view groups
                    </p> :
                    <div className='subDivView'>
                        <p className='mainParagraph'>
                            {disciplineName}
                        </p>
                        <p className='subParagraph'>
                            Choose any of these groups
                        </p>
                        <div className='mainGroupsDiv'>
                            {groups.map(element => {
                                console.log(element);
                                return (
                                <div key={element.id} 
                                    className='oneGroupDiv' 
                                    onClick={() => changeGroupId(element.id)}
                                    >{element.name}</div>
                                )
                            })}
                        </div>
                        <p className='subParagraph'>
                            Or you can edit RSO of this subject
                        </p>
                        <div className='button' onClick={changeModalVisibility}>Edit RSO</div>
                    </div> :
                    <Spinner/>;

    return (
        <div className='mainDivView'>
            {content}
        </div>
    )
}

export default AcademicViewGroups;