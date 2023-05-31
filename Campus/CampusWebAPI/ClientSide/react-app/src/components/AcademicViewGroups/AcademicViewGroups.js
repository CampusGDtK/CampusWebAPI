import { useEffect, useState } from 'react';
import './AcademicViewGroups.scss';
import Spinner from '../Spinner/Spinner';

function AcademicViewGroups ({subjectId, academicId, token}) {
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
                        Choose any subject to <br/> view your marks
                    </p> :
                    <>
                        <p className='mainParagraph'>
                            {disciplineName}
                        </p>
                    </> :
                    <Spinner/>;

    return (
        <div className='mainDivView'>
            {content}
        </div>
    )
}

export default AcademicViewGroups;