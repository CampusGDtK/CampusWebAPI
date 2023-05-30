import './App.scss';
import LogInPage from './components/LogInPage/LogInPage';
import StudentView from './components/StudentView/StudentView';
import AccountPage from './components/AccountPage/AccountPage';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import { useState } from 'react';

function App() {
  const [userId, setUserId] = useState();
  const [role, setRole] = useState();
  const [token, setToken] = useState();

  function setUserCreds(id, role, token) {
    setUserId(id);
    setRole(role);
    setToken(token);
  }

  return (
    <Router>
      <Routes>
        <Route path='/' element={<LogInPage setUserCreds={setUserCreds} />}/>
        <Route path='/student-view' element={<StudentView userId={userId} token={token}/>}/>
        <Route path='/profile' element={<AccountPage />}/>
      </Routes>
    </Router>
  );
}

export default App;
