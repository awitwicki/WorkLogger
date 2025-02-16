import { useSelector } from "react-redux";
import { NavLink, Outlet, Navigate } from "react-router-dom";

const ProtectedRoute = () => {
  const appStateUserInfo = useSelector((state) => state.auth.userInfo);

  // isf user not loginned ther redirect to UnauthorizedScreen
  if (!appStateUserInfo) {
      console.log('ProtectedRoute login');
      return <Navigate to='/login' />
  }
  // is user has no role then returm no access


  // show unauthorized screen if no user is found in redux store
  if (!appStateUserInfo) {
    return (
      <div className="unauthorized">
        <h1>Unauthorized :(</h1>
        <span>
          <NavLink to="/login">Login</NavLink> to gain access
        </span>
      </div>
    );
  }

  // returns child route elements
  return (<Outlet />);
};
export default ProtectedRoute;

// Also like this you can make role-based protected route
