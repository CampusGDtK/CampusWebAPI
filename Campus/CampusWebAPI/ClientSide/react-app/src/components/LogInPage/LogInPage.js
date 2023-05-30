import {Link} from 'react-router-dom';
import validator from 'validator';
import './LogInPage.scss';

function LogInPage({chooseStudentId}) {
    function changeStudentId () {
        const studentId = 'uuu111'
        chooseStudentId(studentId);
    }

    function validationPassword () {
        const password = document.querySelector('#password');
        if (validator.isUUID(password.value)) {
            password.style = "border: none;"
        } else {
            password.style = "border: 3px solid #ff0000ab;" 
        }
    }

    function validationLogin () {
        const login = document.querySelector('#login');
        if (validator.isEmail(login.value)) {
            login.style = "border: none;"
        } else {
            login.style = "border: 3px solid #ff0000ab;" 
        }
    }

    async function authorization () {
        const password = document.querySelector('#password');
        const login = document.querySelector('#login');
        password.value === '' ? password.style = "border: 3px solid #ff0000ab;" : password.style = "border: none;";
        login.value === '' ? login.style = "border: 3px solid #ff0000ab;" : login.style = "border: none;";
        if (login.value !== '' && login.value !== '') {
            const auth = await fetch('http://localhost:5272/api/account/login', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: {
                    "email": `${login.value}`,
                    "password": `${password.value}`
                }
            })
            console.log(auth.json());
        }
    }

    return (
        <div className='mainDiv'>
            <div className='wrapperDiv'>
                <p>Campus 3.0</p>
                <input 
                    onInput={validationLogin}
                    type='text' 
                    required 
                    placeholder='Type your login'
                    id='login'></input>
                <input 
                    onInput={validationPassword} 
                    type='password' 
                    required 
                    placeholder='Type your password' 
                    id='password'></input>
                {/* <Link to='/student-view'> */}
                    <button onClick={authorization}>LOGIN</button>
                {/* </Link> */}
            </div>
        </div>
    )
}

export default LogInPage;