import './App.scss';
import LogInPage from './components/LogInPage/LogInPage';
import StudentView from './components/StudentView/StudentView';
import AccountPage from './components/AccountPage/AccountPage';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import { useState } from 'react';
import AcademicView from './components/AcademicView/AcademicView';

function App() {
  const [userId, setUserId] = useState();
  const [role, setRole] = useState();
  const [token, setToken] = useState();
  const [userData, setUserData] = useState();

  function setUserCreds(id, role, token) {
    setUserId(id);
    setRole(role);
    setToken(token);
  }

  function changeUserData (data) {
    setUserData(data);
  }

  return (
    <Router>
      <Routes>
        <Route path='/' element={<LogInPage setUserCreds={setUserCreds} />}/>
        <Route path='/student-view' element={<StudentView 
                                              userId={userId} 
                                              token={token} 
                                              changeUserData={changeUserData}
                                              userData={userData}/>}/>
        <Route path='/academic-view' element={<AcademicView
                                              userId={userId}
                                              token={token}
                                              changeUserData={changeUserData}
                                              userData={userData}/>}/>
        <Route path='/profile' element={<AccountPage userData={userData} role={role}/>}/>
      </Routes>
    </Router>
  );
}

export default App;
