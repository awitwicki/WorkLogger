import React, { useEffect, useState } from 'react';
import { Panel, Form, Button, InputNumber, Input, DatePicker, Message, useToaster, Loader } from 'rsuite';
import { useNavigate } from 'react-router-dom';
import { getEmployeeSettings, saveEmployeeSettings } from '../services/employeeSettingsService';

const SettingsScreen = () => {
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [fullName, setFullName] = useState('');
    const [contractStartedDate, setContractStartedDate] = useState(null);
    const [vacationDaysPerYear, setVacationDaysPerYear] = useState(20);
    const toaster = useToaster();
    const navigate = useNavigate();

    useEffect(() => {
        const loadSettings = async () => {
            try {
                const response = await getEmployeeSettings();
                if (!response.ok) throw new Error('Failed to load settings');
                const data = await response.json();
                if (data) {
                    setFullName(data.fullName || '');
                    setContractStartedDate(data.contractStartedDate ? new Date(data.contractStartedDate) : null);
                    setVacationDaysPerYear(data.vacationDaysPerYear || 20);
                }
            } catch (error) {
                console.error('Failed to load settings:', error);
                toaster.push(<Message type="error">Failed to load settings</Message>, { duration: 3000 });
            } finally {
                setLoading(false);
            }
        };
        loadSettings();
    }, []);

    const handleSave = async () => {
        if (!fullName.trim()) {
            toaster.push(<Message type="warning">Please enter your full name</Message>, { duration: 3000 });
            return;
        }
        if (!contractStartedDate) {
            toaster.push(<Message type="warning">Please select contract start date</Message>, { duration: 3000 });
            return;
        }

        setSaving(true);
        try {
            const response = await saveEmployeeSettings({
                fullName,
                contractStartedDate: contractStartedDate.toISOString(),
                vacationDaysPerYear
            });
            if (!response.ok) throw new Error('Failed to save settings');
            toaster.push(<Message type="success">Settings saved</Message>, { duration: 3000 });
            navigate('/');
        } catch (error) {
            console.error('Failed to save settings:', error);
            toaster.push(<Message type="error">Failed to save settings</Message>, { duration: 3000 });
        } finally {
            setSaving(false);
        }
    };

    if (loading) return <Loader center size="lg" />;

    return (
        <div className="show-grid">
            <Panel header="Settings" bordered>
                <Form fluid>
                    <Form.Group>
                        <Form.ControlLabel>Full Name</Form.ControlLabel>
                        <Input value={fullName} onChange={setFullName} placeholder="Enter your full name" />
                    </Form.Group>
                    <Form.Group>
                        <Form.ControlLabel>Contract Started Date</Form.ControlLabel>
                        <DatePicker
                            value={contractStartedDate}
                            onChange={setContractStartedDate}
                            oneTap
                            block
                            placeholder="Select date"
                        />
                    </Form.Group>
                    <Form.Group>
                        <Form.ControlLabel>Vacation Days Per Year</Form.ControlLabel>
                        <InputNumber value={vacationDaysPerYear} onChange={setVacationDaysPerYear} min={0} max={365} />
                    </Form.Group>
                    <Form.Group>
                        <Button appearance="primary" onClick={handleSave} loading={saving}>
                            Save
                        </Button>
                    </Form.Group>
                </Form>
            </Panel>
        </div>
    );
};

export default SettingsScreen;
