import React, { useEffect, useState } from 'react'
import {
    IconButton,
    Table,
    Panel, Loader, Placeholder, Tag
} from 'rsuite';
import { FaTrashCan } from "react-icons/fa6";
import { getEmployees } from '../services/employeeService'

const { Column, HeaderCell, Cell } = Table;

const UsersScreen = () => {
    const [loading, setLoading] = React.useState(true);
    const [employees, setEmployees] = useState([]);

    useEffect(() => {
        getEmployees()
            .then(response => {
                setLoading(false);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log(data); // This is your backend data
                setEmployees(data);
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }, []);

    const renderLoading = () => {
            return (
                    <Placeholder.Grid rows={9} columns={4} active />
            );
    };
        
    return (
        <>
            <div className="show-grid">
                 <Panel header="Users" bordered>
                     {employees ? null: null }
                     <Table loading={loading} renderLoading={renderLoading} data={employees} autoHeight={true} bordered={true} >
                         <Column  align="center" fixed>
                             <HeaderCell>Id</HeaderCell>
                             <Cell dataKey="user.id"/>
                         </Column>

                         <Column >
                             <HeaderCell>Email</HeaderCell>
                             <Cell dataKey="user.userName"/>
                         </Column>

                         <Column >
                             <HeaderCell>Roles</HeaderCell>
                             <Cell>
                                 {rowData => (
                                     <span>{rowData.userRoles.map((role, index) => (<Tag key={index}>{role}</Tag>))}</span>
                                 )}
                             </Cell>
                         </Column>
                         
                         <Column >
                             <HeaderCell>...</HeaderCell>

                             <Cell style={{padding: '6px'}}>
                                 {rowData => (
                                     <IconButton icon={<FaTrashCan/>} appearance="default"
                                                 onClick={() => alert(`user to archive: ${rowData.user.userName}`)}/>
                                 )}
                             </Cell>
                         </Column>
                     </Table>
                 </Panel>
            </div>
        </>
    )
}
export default UsersScreen