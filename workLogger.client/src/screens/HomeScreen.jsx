import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { logout, setGoogleToken } from './../AuthSlice'
import { loginWithGoogle } from './../authThunks';
import { fetchAdminData, fetchUserData } from './../generalApiThunks';
import { Button } from 'rsuite';
import {getEmployeeSettings} from "../services/employeeSettingsService";


const CLIENT_ID = process.env.REACT_APP_CLIENT_ID;

const HomeScreen = () => {
    
    const dispatch = useDispatch()

    const appStateUserInfo = useSelector((state) => state.auth.userInfo);
    const userJwtToken = useSelector((state) => state.auth.userJwtToken);
    const userGoogleToken = useSelector((state) => state.auth.userGoogleToken);
    

    const testAdminRequest = async () => {
        try {
            const data = await dispatch(fetchAdminData()).unwrap();
            console.log(data);
        } catch (err) {
         //   setError(err.message); // Якщо помилка, зберігаємо в локальний стан
        } finally {
           // setLoading(false);
        }
    };

    const testUserRequest = async () => {
        try {
            const data = await getEmployeeSettings();
            //const data = await dispatch(fetchUserData()).unwrap();
            console.log(data);
        } catch (err) {
            //   setError(err.message); // Якщо помилка, зберігаємо в локальний стан
        } finally {
            // setLoading(false);
        }
    };

    return (
          <>
           <p>
                        Lets log
                    </p>
                    <p>
                        userJwtToken:
                        {userJwtToken?.slice(0, 30)}...
                    </p>
                    <p>
                        userGoogleToken:
                        {userGoogleToken?.slice(0, 30)}...
                    </p>
                    <p>
                        {appStateUserInfo?.name}
                    </p>
                    <ul>
                        Roles:
                        {appStateUserInfo?.roles.map((str, index) => (
                            <li key={str}>{str}</li> // Render each string as a list item
                        ))}
                    </ul>
                    <Button className='button' onClick={() => dispatch(logout())}>
                        Logout
                    </Button>
                    <Button onClick={testAdminRequest}>
                        Test admin
                    </Button>
                    <Button onClick={testUserRequest}>
                        Test user
                    </Button>
          </>
    )
}
export default HomeScreen