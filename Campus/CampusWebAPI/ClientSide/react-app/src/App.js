import './App.scss';
import LogInPage from './components/LogInPage/LogInPage';
import StudentView from './components/StudentView/StudentView';
import AccountPage from './components/AccountPage/AccountPage';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';

function App() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<LogInPage />}/>
        <Route path='/student-view' element={<StudentView />}/>
        <Route path='/profile' element={<AccountPage />}/>
      </Routes>
    </Router>
  );
}

export default App;
