import React from 'react'
import {NavLink} from "react-router-dom";

const UnauthorizedScreen = () => {
    return (
        <div className="unauthorized">
            <h1>Unauthorized :(</h1>
            <span>
            <NavLink to="/login">Login</NavLink> to gain access
        </span>
        </div>
    );
}
export default UnauthorizedScreen
