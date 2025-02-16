import React, { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';
import { useDispatch, useSelector } from 'react-redux'
import { setGoogleToken } from './../AuthSlice'
import { loginWithGoogle } from './../authThunks';

import {
    Panel
  } from 'rsuite';

const CLIENT_ID = process.env.REACT_APP_CLIENT_ID;

const LoginScreen = () => {
    
    const dispatch = useDispatch()
    const appStateUserInfo = useSelector((state) => state.auth.userInfo);
    const navigate = useNavigate();

    const handleLoginFailure = (error) => {
        console.error('Google login failed:', error);
    };

    const handleLoginSuccess = (response) => {
        dispatch(setGoogleToken(response.credential));
        dispatch(loginWithGoogle(response.credential));
    };
    
    // Redirect authenticated user to profile screen
    useEffect(() => {
        if (appStateUserInfo) {
            navigate('/')
        }
    }, [navigate, appStateUserInfo])
    
    // Simple form in center of screen
    // Centered form 
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