import React, { useEffect, useState } from 'react';
import { useSelector } from "react-redux";
import { NavLink, Outlet, Navigate, useLocation } from "react-router-dom";
import { Loader } from 'rsuite';
import { getEmployeeSettings } from './services/employeeSettingsService';

const ProtectedRoute = () => {
  const appStateUserInfo = useSelector((state) => state.auth.userInfo);
  const location = useLocation();
  const [settingsChecked, setSettingsChecked] = useState(false);
  const [hasSettings, setHasSettings] = useState(true);

  useEffect(() => {
    if (!appStateUserInfo) return;

    const checkSettings = async () => {
      try {
        const response = await getEmployeeSettings();
        if (response.ok) {
          const data = await response.json();
          setHasSettings(data !== null);
        }
      } catch (error) {
        console.error('Failed to check settings:', error);
      } finally {
        setSettingsChecked(true);
      }
    };

    checkSettings();
  }, [appStateUserInfo]);

  if (!appStateUserInfo) {
    return <Navigate to='/login' />;
  }

  if (!settingsChecked) {
    return <Loader center size="lg" />;
  }

  if (!hasSettings && location.pathname !== '/settings') {
    return <Navigate to='/settings' />;
  }

  return (<Outlet />);
};
export default ProtectedRoute;
