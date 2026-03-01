import React, { useEffect, useState } from 'react';
import {
    IconButton,
    Table,
    Panel,
    Tag,
    Button,
    Message,
    useToaster,
    Modal,
    SelectPicker,
    ButtonToolbar,
    Form,
    Input,
    InputNumber,
    DatePicker,
    Loader
} from 'rsuite';
import { FaTrashCan } from "react-icons/fa6";
import { FaGear } from "react-icons/fa6";
import { getEmployees, removeUser, addRoleToUser, cleanUserRoles } from '../services/employeeService';
import { getEmployeeSettingsById, saveEmployeeSettingsById } from '../services/employeeSettingsService';
import { useNavigate } from 'react-router-dom';

const { Column, HeaderCell, Cell } = Table;

const roleOptions = [
    { label: 'admin', value: 'admin' },
    { label: 'user', value: 'user' }
];

const UsersScreen = () => {
    const [loading, setLoading] = useState(true);
    const [employees, setEmployees] = useState([]);
    const [showDeleteModal, setShowDeleteModal] = useState(false);
    const [userToDelete, setUserToDelete] = useState(null);
    const [roleToAdd, setRoleToAdd] = useState(null);
    const [roleUserId, setRoleUserId] = useState(null);
    const [showSettingsModal, setShowSettingsModal] = useState(false);
    const [settingsUserId, setSettingsUserId] = useState(null);
    const [settingsLoading, setSettingsLoading] = useState(false);
    const [settingsSaving, setSettingsSaving] = useState(false);
    const [settingsFullName, setSettingsFullName] = useState('');
    const [settingsContractDate, setSettingsContractDate] = useState(null);
    const [settingsVacationDays, setSettingsVacationDays] = useState(20);
    const toaster = useToaster();
    const navigate = useNavigate();

    const loadEmployees = async () => {
        try {
            const response = await getEmployees();
            if (!response.ok) throw new Error('Network response was not ok');
            const data = await response.json();
            setEmployees(data);
        } catch (error) {
            console.error('Failed to load employees:', error);
            toaster.push(<Message type="error">Failed to load employees</Message>, { duration: 3000 });
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadEmployees();
    }, []);

    const handleDeleteClick = (user) => {
        setUserToDelete(user);
        setShowDeleteModal(true);
    };

    const handleDeleteConfirm = async () => {
        if (!userToDelete) return;
        try {
            const response = await removeUser(userToDelete.id);
            if (!response.ok) throw new Error('Failed to delete user');
            setEmployees(prev => prev.filter(e => e.user.id !== userToDelete.id));
            toaster.push(<Message type="success">User removed</Message>, { duration: 3000 });
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to remove user</Message>, { duration: 3000 });
        } finally {
            setShowDeleteModal(false);
            setUserToDelete(null);
        }
    };

    const handleAddRole = async (userId) => {
        if (!roleToAdd) return;
        try {
            const response = await addRoleToUser(userId, roleToAdd);
            if (!response.ok) throw new Error('Failed to add role');
            toaster.push(<Message type="success">Role added</Message>, { duration: 3000 });
            setRoleToAdd(null);
            setRoleUserId(null);
            setLoading(true);
            await loadEmployees();
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to add role</Message>, { duration: 3000 });
        }
    };

    const handleCleanRoles = async (userId) => {
        try {
            const response = await cleanUserRoles(userId);
            if (!response.ok) throw new Error('Failed to clean roles');
            toaster.push(<Message type="success">Roles cleaned</Message>, { duration: 3000 });
            setLoading(true);
            await loadEmployees();
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to clean roles</Message>, { duration: 3000 });
        }
    };

    const handleOpenSettings = async (userId) => {
        setSettingsUserId(userId);
        setShowSettingsModal(true);
        setSettingsLoading(true);
        try {
            const response = await getEmployeeSettingsById(userId);
            if (!response.ok) throw new Error('Failed to load settings');
            const data = await response.json();
            if (data) {
                setSettingsFullName(data.fullName || '');
                setSettingsContractDate(data.contractStartedDate ? new Date(data.contractStartedDate) : null);
                setSettingsVacationDays(data.vacationDaysPerYear || 20);
            } else {
                setSettingsFullName('');
                setSettingsContractDate(null);
                setSettingsVacationDays(20);
            }
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to load user settings</Message>, { duration: 3000 });
        } finally {
            setSettingsLoading(false);
        }
    };

    const handleSaveSettings = async () => {
        setSettingsSaving(true);
        try {
            const response = await saveEmployeeSettingsById(settingsUserId, {
                fullName: settingsFullName,
                contractStartedDate: settingsContractDate ? settingsContractDate.toISOString() : null,
                vacationDaysPerYear: settingsVacationDays
            });
            if (!response.ok) throw new Error('Failed to save settings');
            toaster.push(<Message type="success">Settings saved</Message>, { duration: 3000 });
            setShowSettingsModal(false);
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to save settings</Message>, { duration: 3000 });
        } finally {
            setSettingsSaving(false);
        }
    };

    return (
        <>
            <div className="show-grid">
                <Panel header="Users" bordered>
                    <Table loading={loading} data={employees} autoHeight={true} bordered={true}>
                        <Column width={280} align="center" fixed>
                            <HeaderCell>Id</HeaderCell>
                            <Cell>{rowData => rowData.user.id}</Cell>
                        </Column>

                        <Column width={250}>
                            <HeaderCell>Email</HeaderCell>
                            <Cell>{rowData => rowData.user.userName}</Cell>
                        </Column>

                        <Column width={200}>
                            <HeaderCell>Roles</HeaderCell>
                            <Cell>
                                {rowData => (
                                    <span>{rowData.userRoles.map((role, index) => (
                                        <Tag key={index} color={role === 'admin' ? 'red' : 'blue'}>{role}</Tag>
                                    ))}</span>
                                )}
                            </Cell>
                        </Column>

                        <Column width={350}>
                            <HeaderCell>Actions</HeaderCell>
                            <Cell style={{ padding: '6px' }}>
                                {rowData => (
                                    <ButtonToolbar>
                                        {roleUserId === rowData.user.id ? (
                                            <>
                                                <SelectPicker
                                                    data={roleOptions}
                                                    value={roleToAdd}
                                                    onChange={setRoleToAdd}
                                                    size="xs"
                                                    style={{ width: 100 }}
                                                    searchable={false}
                                                    placeholder="Role"
                                                />
                                                <Button size="xs" appearance="primary" onClick={() => handleAddRole(rowData.user.id)}>
                                                    OK
                                                </Button>
                                                <Button size="xs" onClick={() => { setRoleUserId(null); setRoleToAdd(null); }}>
                                                    Cancel
                                                </Button>
                                            </>
                                        ) : (
                                            <>
                                                <Button size="xs" appearance="ghost" onClick={() => setRoleUserId(rowData.user.id)}>
                                                    Add Role
                                                </Button>
                                                <Button size="xs" appearance="ghost" color="orange"
                                                        onClick={() => handleCleanRoles(rowData.user.id)}>
                                                    Clean Roles
                                                </Button>
                                                <Button size="xs" appearance="ghost"
                                                        onClick={() => navigate(`/admin/month?userId=${rowData.user.id}`)}>
                                                    View Months
                                                </Button>
                                                <IconButton icon={<FaGear />} size="xs" appearance="subtle"
                                                            onClick={() => handleOpenSettings(rowData.user.id)} />
                                                <IconButton icon={<FaTrashCan />} size="xs" appearance="subtle" color="red"
                                                            onClick={() => handleDeleteClick(rowData.user)} />
                                            </>
                                        )}
                                    </ButtonToolbar>
                                )}
                            </Cell>
                        </Column>
                    </Table>
                </Panel>
            </div>

            <Modal open={showDeleteModal} onClose={() => setShowDeleteModal(false)} size="xs">
                <Modal.Header>
                    <Modal.Title>Confirm Delete</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    Are you sure you want to delete user <strong>{userToDelete?.userName}</strong>?
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleDeleteConfirm} appearance="primary" color="red">
                        Delete
                    </Button>
                    <Button onClick={() => setShowDeleteModal(false)} appearance="subtle">
                        Cancel
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal open={showSettingsModal} onClose={() => setShowSettingsModal(false)} size="sm">
                <Modal.Header>
                    <Modal.Title>User Settings</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    {settingsLoading ? (
                        <Loader center />
                    ) : (
                        <Form fluid>
                            <Form.Group>
                                <Form.ControlLabel>Full Name</Form.ControlLabel>
                                <Input value={settingsFullName} onChange={setSettingsFullName} />
                            </Form.Group>
                            <Form.Group>
                                <Form.ControlLabel>Contract Started Date</Form.ControlLabel>
                                <DatePicker
                                    value={settingsContractDate}
                                    onChange={setSettingsContractDate}
                                    oneTap
                                    block
                                />
                            </Form.Group>
                            <Form.Group>
                                <Form.ControlLabel>Vacation Days Per Year</Form.ControlLabel>
                                <InputNumber value={settingsVacationDays} onChange={setSettingsVacationDays} min={0} max={365} />
                            </Form.Group>
                        </Form>
                    )}
                </Modal.Body>
                <Modal.Footer>
                    <Button onClick={handleSaveSettings} appearance="primary" loading={settingsSaving}>
                        Save
                    </Button>
                    <Button onClick={() => setShowSettingsModal(false)} appearance="subtle">
                        Cancel
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
};

export default UsersScreen;
