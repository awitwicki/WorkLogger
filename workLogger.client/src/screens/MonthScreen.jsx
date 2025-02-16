import React, { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { logout, setGoogleToken } from './../AuthSlice'
import { loginWithGoogle } from './../authThunks';
import { fetchAdminData, fetchUserData } from './../generalApiThunks';
import {Badge, Button, Calendar} from 'rsuite';

const CLIENT_ID = process.env.REACT_APP_CLIENT_ID;

const MonthScreen = () => {
    
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
            const data = await dispatch(fetchUserData()).unwrap();
            console.log(data);
        } catch (err) {
            //   setError(err.message); // Якщо помилка, зберігаємо в локальний стан
        } finally {
            // setLoading(false);
        }
    };


    function renderCell(date) {

        if (1) {
            return <Badge className="calendar-todo-item-badge" />;
        }

        return null;
    }

    return (
          <>
              <Calendar compact renderCell={renderCell} style={{ width: 320 }} />
                    
                    <Button onClick={testUserRequest}>
                        save
                    </Button>
          </>
    )
}
export default MonthScreen