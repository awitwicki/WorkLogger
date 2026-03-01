import React from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { Badge, Button, Calendar, Panel } from 'rsuite';

const HomeScreen = () => {
    const navigate = useNavigate();
    const appStateUserInfo = useSelector((state) => state.auth.userInfo);

    function renderCell(date) {
        const dayOfWeek = date.getDay();
        if (dayOfWeek === 0 || dayOfWeek === 6) {
            return <Badge className="calendar-todo-item-badge" />;
        }
        return null;
    }

    return (
        <div className="show-grid">
            <Panel header={`Welcome, ${appStateUserInfo?.name || 'User'}`} bordered>
                <Calendar compact renderCell={renderCell} style={{ width: 320 }} />
                <div style={{ marginTop: 20 }}>
                    <Button appearance="primary" onClick={() => navigate('/month')}>
                        Go to Current Month
                    </Button>
                </div>
            </Panel>
        </div>
    );
};

export default HomeScreen;
