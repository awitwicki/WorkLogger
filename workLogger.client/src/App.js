// import './App.less';
import 'rsuite/styles/index.less';
import 'rsuite/dist/rsuite.min.css';
import React from 'react';
import { CustomProvider } from 'rsuite';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import Layout from './Layout';
import HomeScreen from './screens/HomeScreen';
import LoginScreen from './screens/LoginScreen';
import ProtectedRoute from './ProtectedRoute';
import Unauthorized from "./screens/Unauthorized";
import MonthScreen from "./screens/MonthScreen";
import HolidaysScreen from "./screens/HolidaysScreen";
import UsersScreen from "./screens/UsersScreen";

function App() {
    // TODO rework to redux state
    const [theme, setTheme] = React.useState('dark');

    const toggleTheme = checked => {
        setTheme(checked ? 'light' : 'dark');
    };

    return (
        <CustomProvider theme={theme}>
            <Router>
                <Routes>
                    <Route path="/login" element={<LoginScreen/>}/>
                    <Route path="/unauthorized" element={<Unauthorized/>}/>

                    <Route element={<Layout/>}>
                        <Route element={<ProtectedRoute/>}>
                            <Route path="/" element={<HomeScreen/>}/>
                            <Route path="/month" element={<MonthScreen/>}/>
                            <Route path="/profile" element={<Unauthorized/>}/>
                            <Route path="/holidays" element={<HolidaysScreen/>}/>
                            <Route path="/users" element={<UsersScreen/>}/>
                        </Route>
                    </Route>

                    <Route path="*" element={<Navigate to="/login"/>}/>

                </Routes>
            </Router>
        </CustomProvider>
    );
}

export default App;
