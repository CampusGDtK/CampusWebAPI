import './App.scss';
import LogInPage from './components/LogInPage/LogInPage';
import StudentView from './components/StudentView/StudentView';
import AccountPage from './components/AccountPage/AccountPage';
import {BrowserRouter as Router, Route, Routes} from 'react-router-dom';
import { useState } from 'react';

function App() {
  const [studentId, setStudentId] = useState();

  function chooseStudentId(id) {
    console.log(id);
    setStudentId(id);
  }

  return (
    <Router>
      <Routes>
        <Route path='/' element={<LogInPage setStudentId={chooseStudentId} />}/>
        <Route path='/student-view' element={<StudentView studentId={studentId}/>}/>
        <Route path='/profile' element={<AccountPage />}/>
      </Routes>
    </Router>
  );
}

export default App;
