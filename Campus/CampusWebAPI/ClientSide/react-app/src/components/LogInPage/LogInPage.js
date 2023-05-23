import {Link} from 'react-router-dom';
import './LogInPage.scss';

function LogInPage() {
    return (
        <div className='mainDiv'>
            <div className='wrapperDiv'>
                <p>Campus 3.0</p>
                <input type='text' required placeholder='Type your login'></input>
                <input type='password' required placeholder='Type your password'></input>
                <Link to='/student-view'>
                    <button>LOGIN</button>
                </Link>
            </div>
        </div>
    )
}

export default LogInPage;