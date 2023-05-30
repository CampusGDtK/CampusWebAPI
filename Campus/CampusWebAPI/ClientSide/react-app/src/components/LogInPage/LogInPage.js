import {useNavigate} from 'react-router-dom';
import validator from 'validator';
import './LogInPage.scss';

function LogInPage({setUserCreds}) {
    const navigate = useNavigate();

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
        validationLogin();
        validationPassword();
        if (login.value !== '' && login.value !== '') {
            fetch('http://localhost:5272/api/account/login', {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    email: `${login.value}`,
                    password: `${password.value}`
                })
            })
            .then(response =>  response.ok ? response.json() : new Error('Error on the server-side'))
            .then(data => {
                console.log(data);
                if (data.email) {
                    setUserCreds(data.userId, data.role, data.token)
                    data.role === 'Student' ? 
                    navigate('/student-view') : data.role === 'Academic' ? 
                    navigate('/academic-view') : navigate('/admin-view');
                }
                else {
                    login.style = "border: 3px solid #ff0000ab;"
                    password.style = "border: 3px solid #ff0000ab;"
                };
            })
            .catch(error => console.log(error))
        }
    }

    return (
        <div className='mainDiv'>
            <div className='wrapperDiv'>
                <p>Campus 3.0</p>
                <input 
                    onInput={validationLogin}
                    onBlur={validationLogin}
                    type='text' 
                    required 
                    placeholder='Type your login'
                    id='login'></input>
                <input 
                    onInput={validationPassword} 
                    onBlur={validationPassword}
                    type='password' 
                    required 
                    placeholder='Type your password' 
                    id='password'></input>
                <button onClick={authorization}>LOGIN</button>
            </div>
        </div>
    )
}

export default LogInPage;