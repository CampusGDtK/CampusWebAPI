import Loader from '../../images/loader.gif';
import './Spinner.scss';

function Spinner () {
    return (
        <>
            <img src={Loader} alt='loader' className='spinner'/>
        </>
    )
}

export default Spinner;