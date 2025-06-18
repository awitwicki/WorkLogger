import React, { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import { useDispatch, useSelector } from 'react-redux'
import { setGoogleToken } from '../AuthSlice'
import { loginWithGoogle } from '../authThunks';

import {
    Message,
    Panel,
  } from 'rsuite';

const CLIENT_ID = process.env.REACT_APP_CLIENT_ID;

const LoginScreen = () => {
    const dispatch = useDispatch()
    const appStateUserInfo = useSelector((state) => state.auth.userInfo);
    const authError = useSelector((state) => state.auth.error);
    const navigate = useNavigate();

    const handleLoginFailure = (error) => {
        console.error('Google login failed:', error);
    };

    const handleLoginSuccess = (response) => {
        dispatch(setGoogleToken(response.credential));
        dispatch(loginWithGoogle(response.credential));
    };
    
    // Redirect authenticated user to the profile screen
    useEffect(() => {
        if (appStateUserInfo) {
            navigate('/')
        }
    }, [navigate, appStateUserInfo])
    
    return (
        <div
            className="app"
            style={{
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                height: '100vh', // Full height of the viewport
            }}
        >
            <Panel
                header="Sign in by Google account"
                bordered
                style={{
                    width: 400,
                    textAlign: 'center', // Center the header and content inside the panel
                }}
            >
                {authError &&
                    <Message showIcon type="error" header="A problem occurred" style={{ marginBottom: '1rem' }}>
                        Server temporary unavailable, try again later
                    </Message>}
               
                <GoogleOAuthProvider clientId={CLIENT_ID}>
                    <div
                        style={{
                            display: 'flex',
                            justifyContent: 'center',
                        }}
                    >
                        <GoogleLogin
                            onSuccess={handleLoginSuccess}
                            onError={handleLoginFailure}
                            auto_select={false}
                            prompt="select_account"
                        />
                    </div>
                </GoogleOAuthProvider>
            </Panel>
        </div>
    )
}
export default LoginScreen