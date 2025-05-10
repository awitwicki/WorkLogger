import React, {useEffect, useState} from 'react'
import {
    Button,
    FlexboxGrid,
    Heading,
    IconButton,
    Table,
    DatePicker,
    Input,
    Card,
    Text,
    Panel
} from 'rsuite';
import {FaTrashCan} from "react-icons/fa6";
import PlusIcon from '@rsuite/icons/Plus';
import {getHolidaysList, addHoliday, removeHoliday, importHolidays} from "../services/holidaysService";

const {Column, HeaderCell, Cell} = Table;


const HolidaysScreen = () => {
    const [loading, setLoading] = useState(true);
    const [holidays, setHolidays] = useState([]);

    const [holidayName, setHolidayName] = useState('testio');
    const [holidayDate, setHolidayDate] = useState(new Date());
    
    useEffect(() => {
        getHolidaysList()
            .then(response => {
                setLoading(false);
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                setHolidays(data);
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
    }, []);

    const addHolidayBtnClick = async () => {
        if (!holidayName || !holidayDate) {
            alert('Please enter holiday name and date');
            return;
        }

        let result = await addHoliday(holidayDate, holidayName);

        if (result.ok) {
            const newHoliday = {
                    name: holidayName,
                    dateDay: holidayDate.toISOString()
            };
           
            setHolidays(prevHolidays => [...prevHolidays, newHoliday]);
        } else {
            const errorMessage = await result.text();
            alert(`Failed to add holiday: ${errorMessage}`);
        }
    };

    const removeHolidayBtnClick = async (holidayDate) => {
        let result = await removeHoliday(holidayDate);

        if (result.ok) {
            setHolidays(prevHolidays =>
                prevHolidays.filter(holiday => holiday.dateDay !== holidayDate)
            );
        } else {
            const errorMessage = await result.text();
            alert(`Failed to remove holiday: ${errorMessage}`);
        }
    };

    const importHolidaysBtnClick = async () => {
        let result = await importHolidays();

        if (result.ok) {
            getHolidaysList()
                .then(response => {
                    setLoading(false);
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    setHolidays(data);
                })
                .catch(error => {
                    console.error('There was a problem with the fetch operation:', error);
                });
        } else {
            const errorMessage = await result.text();
            alert(`Failed to remove holiday: ${errorMessage}`);
        }
    };

    // TODO
    // const renderLoading = () => {
    //     return (
    //         <Placeholder.Grid rows={9} columns={4} active/>
    //     );
    // };

    // TODO: fix frontend bug when backend is offline
    


    return (
        <div className="show-grid">
            <FlexboxGrid>
                <FlexboxGrid.Item colspan={12}>
                    <Panel header="Holidays" bordered>
                        <Table loading={loading} data={holidays} autoHeight={true} bordered={true}>
                            <Column align="center" fixed>
                                <HeaderCell>Date</HeaderCell>
                                <Cell dataKey="dateDay"/>
                            </Column>

                            <Column>
                                <HeaderCell>Name</HeaderCell>
                                <Cell dataKey="name"/>
                            </Column>

                            <Column fixed="right">
                                <HeaderCell>...</HeaderCell>

                                <Cell style={{padding: '6px'}}>
                                    {rowData => (
                                        <IconButton icon={<FaTrashCan/>} appearance="default"
                                                    onClick={() => removeHolidayBtnClick(rowData.dateDay)}/>
                                    )}
                                </Cell>
                            </Column>
                        </Table>
                    </Panel>

                </FlexboxGrid.Item>
                <FlexboxGrid.Item colspan={12}>
                    <Card width={400} shaded size="lg">
                        <Card.Header>
                            <Heading level={4}>Add new holiday</Heading>
                            <Text muted>Fill in the form below to create a new project</Text>
                        </Card.Header>
                        <Card.Body>
                            <DatePicker value={holidayDate}
                                        onChange={setHolidayDate}/>
                            <Input placeholder="Default Input"  value={holidayName}
                                   onChange={setHolidayName}/>
                        </Card.Body>
                        <Card.Footer>
                            <IconButton icon={<PlusIcon/>} onClick={addHolidayBtnClick}>Add</IconButton>
                            <Button onClick={importHolidaysBtnClick}>Import holidays 2024</Button>
                        </Card.Footer>
                    </Card>
                </FlexboxGrid.Item>
            </FlexboxGrid>
        </div>
    )
}
export default HolidaysScreen