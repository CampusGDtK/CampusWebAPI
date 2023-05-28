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
            console.log('password ok');
        } else {
            console.log('password error');
        }
        console.log(password.value);
    }

    function validationLogin () {
        const login = document.querySelector('#login');
        if (login.value.length < 4) {
            console.log('login is too short');
        } else if (login.value.length > 20) {
            console.log('login is too long');
        } else {
            console.log('login is ok')
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
                <Link to='/student-view'>
                    <button>LOGIN</button>
                </Link>
            </div>
        </div>
    )
}

export default LogInPage;