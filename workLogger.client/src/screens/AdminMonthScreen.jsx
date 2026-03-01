import React, { useEffect, useState, useCallback } from 'react';
import {
    Button,
    Table,
    Panel,
    DatePicker,
    SelectPicker,
    FlexboxGrid,
    Input,
    Loader,
    Message,
    useToaster,
    ButtonToolbar
} from 'rsuite';
import { getEmployees } from '../services/employeeService';
import { getMonth, saveMonth, exportPdf } from '../services/monthService';

const { Column, HeaderCell, Cell } = Table;

const dayTypeOptions = [
    { label: 'Normal', value: 'normal' },
    { label: 'Vacation', value: 'vacation' },
    { label: 'L4 (Sick)', value: 'l4' },
    { label: 'Day Off', value: 'dayoff' }
];

const formatTime = (timeSpan) => {
    if (!timeSpan) return '';
    if (typeof timeSpan === 'string') {
        const parts = timeSpan.split(':');
        return `${parts[0]}:${parts[1]}`;
    }
    return '';
};

const getDayType = (day) => {
    if (day.isVacation) return 'vacation';
    if (day.isL4) return 'l4';
    if (day.isDayOff) return 'dayoff';
    return 'normal';
};

const formatDate = (dateStr) => {
    const d = new Date(dateStr);
    return d.toLocaleDateString('en-US', { weekday: 'short', day: '2-digit', month: '2-digit' });
};

const getDayName = (dateStr) => {
    const d = new Date(dateStr);
    return d.toLocaleDateString('en-US', { weekday: 'long' });
};

const isWeekend = (dateStr) => {
    const d = new Date(dateStr);
    return d.getDay() === 0 || d.getDay() === 6;
};

const EditableTimeCell = ({ rowData, dataKey, onChange }) => {
    const isDayOff = rowData.isDayOff || rowData.isVacation || rowData.isL4;
    return (
        <Cell style={getCellStyle(rowData)}>
            <Input
                size="sm"
                value={formatTime(rowData[dataKey])}
                disabled={isDayOff}
                onChange={(value) => onChange(rowData.date, dataKey, value)}
                style={{ width: 70 }}
                placeholder="HH:mm"
            />
        </Cell>
    );
};

const getCellStyle = (rowData) => {
    if (rowData.isDayOff || isWeekend(rowData.date)) {
        return { backgroundColor: '#1a3a5c', padding: '6px' };
    }
    return { padding: '6px' };
};

const AdminMonthScreen = () => {
    const [selectedDate, setSelectedDate] = useState(new Date());
    const [selectedUserId, setSelectedUserId] = useState(null);
    const [users, setUsers] = useState([]);
    const [days, setDays] = useState([]);
    const [loading, setLoading] = useState(false);
    const [saving, setSaving] = useState(false);
    const toaster = useToaster();

    useEffect(() => {
        getEmployees()
            .then(response => {
                if (!response.ok) throw new Error('Failed to load users');
                return response.json();
            })
            .then(data => {
                setUsers(data.map(e => ({
                    label: e.user.userName,
                    value: e.user.id
                })));
            })
            .catch(err => console.error(err));
    }, []);

    const loadMonth = useCallback(async (date, userId) => {
        if (!userId) return;
        setLoading(true);
        try {
            const isoDate = `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-01`;
            const response = await getMonth(isoDate, userId);
            if (!response.ok) {
                throw new Error(`Failed to load month: ${response.status}`);
            }
            const data = await response.json();
            setDays(data.days || []);
        } catch (err) {
            console.error(err);
            setDays([]);
            toaster.push(<Message type="error">Failed to load month data</Message>, { duration: 3000 });
        } finally {
            setLoading(false);
        }
    }, [toaster]);

    useEffect(() => {
        if (selectedUserId) {
            loadMonth(selectedDate, selectedUserId);
        }
    }, [selectedDate, selectedUserId, loadMonth]);

    const handleCellChange = (date, key, value) => {
        setDays(prev => prev.map(day => {
            if (day.date === date) {
                return { ...day, [key]: value };
            }
            return day;
        }));
    };

    const handleTypeChange = (date, type) => {
        setDays(prev => prev.map(day => {
            if (day.date === date) {
                return {
                    ...day,
                    isVacation: type === 'vacation',
                    isL4: type === 'l4',
                    isDayOff: type === 'dayoff' || isWeekend(day.date),
                    startHour: (type !== 'normal') ? null : day.startHour,
                    endHour: (type !== 'normal') ? null : day.endHour,
                };
            }
            return day;
        }));
    };

    const handleSave = async () => {
        if (!selectedUserId) return;
        setSaving(true);
        try {
            const isoDate = `${selectedDate.getFullYear()}-${String(selectedDate.getMonth() + 1).padStart(2, '0')}-01`;
            const payload = days.map(d => ({
                date: d.date,
                startHour: d.startHour || null,
                endHour: d.endHour || null,
                isVacation: d.isVacation || false,
                isDayOff: d.isDayOff || false,
                isL4: d.isL4 || false,
            }));
            const response = await saveMonth(payload, isoDate, selectedUserId);
            if (!response.ok) throw new Error('Failed to save');
            toaster.push(<Message type="success">Month saved successfully</Message>, { duration: 3000 });
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to save month</Message>, { duration: 3000 });
        } finally {
            setSaving(false);
        }
    };

    const handleExportPdf = async () => {
        if (!selectedUserId) return;
        try {
            const isoDate = `${selectedDate.getFullYear()}-${String(selectedDate.getMonth() + 1).padStart(2, '0')}-01`;
            const response = await exportPdf(isoDate, selectedUserId);
            if (!response.ok) throw new Error('Failed to export PDF');
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = `WorkLog_${selectedDate.getFullYear()}-${String(selectedDate.getMonth() + 1).padStart(2, '0')}.pdf`;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        } catch (err) {
            console.error(err);
            toaster.push(<Message type="error">Failed to export PDF</Message>, { duration: 3000 });
        }
    };

    return (
        <div className="show-grid">
            <Panel header="Admin - User Months" bordered>
                <FlexboxGrid style={{ marginBottom: 20, gap: 10 }} align="middle">
                    <FlexboxGrid.Item>
                        <SelectPicker
                            data={users}
                            value={selectedUserId}
                            onChange={setSelectedUserId}
                            placeholder="Select user"
                            style={{ width: 250 }}
                            searchable={true}
                        />
                    </FlexboxGrid.Item>
                    <FlexboxGrid.Item>
                        <DatePicker
                            format="yyyy-MM"
                            value={selectedDate}
                            onChange={(value) => value && setSelectedDate(value)}
                            cleanable={false}
                            style={{ width: 200 }}
                        />
                    </FlexboxGrid.Item>
                    <FlexboxGrid.Item>
                        <ButtonToolbar>
                            <Button appearance="primary" onClick={handleSave} loading={saving} disabled={!selectedUserId}>
                                Save
                            </Button>
                            <Button appearance="ghost" onClick={handleExportPdf} disabled={!selectedUserId}>
                                Export PDF
                            </Button>
                        </ButtonToolbar>
                    </FlexboxGrid.Item>
                </FlexboxGrid>

                {!selectedUserId ? (
                    <Message type="info">Select a user to view their month data</Message>
                ) : loading ? (
                    <Loader center content="Loading..." />
                ) : days.length === 0 ? (
                    <Message type="info">No data for this month</Message>
                ) : (
                    <Table
                        data={days}
                        autoHeight={true}
                        bordered={true}
                        cellBordered={true}
                    >
                        <Column width={130} fixed>
                            <HeaderCell>Date</HeaderCell>
                            <Cell style={(rowData) => getCellStyle(rowData)}>
                                {rowData => formatDate(rowData.date)}
                            </Cell>
                        </Column>

                        <Column width={110}>
                            <HeaderCell>Day</HeaderCell>
                            <Cell style={(rowData) => getCellStyle(rowData)}>
                                {rowData => getDayName(rowData.date)}
                            </Cell>
                        </Column>

                        <Column width={100}>
                            <HeaderCell>Start Hour</HeaderCell>
                            {rowData => (
                                <EditableTimeCell
                                    rowData={rowData}
                                    dataKey="startHour"
                                    onChange={handleCellChange}
                                />
                            )}
                        </Column>

                        <Column width={100}>
                            <HeaderCell>End Hour</HeaderCell>
                            {rowData => (
                                <EditableTimeCell
                                    rowData={rowData}
                                    dataKey="endHour"
                                    onChange={handleCellChange}
                                />
                            )}
                        </Column>

                        <Column width={140}>
                            <HeaderCell>Type</HeaderCell>
                            <Cell style={(rowData) => getCellStyle(rowData)}>
                                {rowData => {
                                    const isWeekendDay = isWeekend(rowData.date);
                                    if (isWeekendDay && !rowData.isVacation && !rowData.isL4) {
                                        return 'Weekend';
                                    }
                                    return (
                                        <SelectPicker
                                            data={dayTypeOptions}
                                            value={getDayType(rowData)}
                                            onChange={(value) => handleTypeChange(rowData.date, value)}
                                            cleanable={false}
                                            searchable={false}
                                            size="sm"
                                            style={{ width: 120 }}
                                            disabled={isWeekendDay}
                                        />
                                    );
                                }}
                            </Cell>
                        </Column>

                        <Column flexGrow={1}>
                            <HeaderCell>Info</HeaderCell>
                            <Cell style={(rowData) => getCellStyle(rowData)}>
                                {rowData => rowData.holiday?.name || ''}
                            </Cell>
                        </Column>
                    </Table>
                )}
            </Panel>
        </div>
    );
};

export default AdminMonthScreen;
