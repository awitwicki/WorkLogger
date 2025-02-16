import React, { useEffect, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { fetchAdminData, fetchUserData } from './../generalApiThunks';
import {
    Badge,
    Button,
    FlexboxGrid,
    Heading,
    IconButton,
    Table,
    DatePicker,
    Input,
    Card,
    Text,
    Form,
    SelectPicker,
    Panel
} from 'rsuite';
import { FaTrashCan } from "react-icons/fa6";
import PlusIcon from '@rsuite/icons/Plus';

const { Column, HeaderCell, Cell } = Table;

const data = [
    {
        date: 1,
        name: "sdfdsf"
    },
    {
        date: 2,
        name: "sdfdsf"
    },
    {
        date: 3,
        name: "sdfdsf"
    },
    {
        date: 4,
        name: "sdfdsf"
    },
];

const HolidaysScreen = () => {
    // const testAdminRequest = async () => {
    //     try {
    //         const data = await dispatch(fetchAdminData()).unwrap();
    //         console.log(data);
    //     } catch (err) {
    //      //   setError(err.message); // Якщо помилка, зберігаємо в локальний стан
    //     } finally {
    //        // setLoading(false);
    //     }
    // };
    
    return (
        <>
            <div className="show-grid">
                <FlexboxGrid>
                    <FlexboxGrid.Item colspan={12}>
                                            <Panel header="Holidays" bordered>
                                                <Table data={data} autoHeight={true} bordered={true} >
                                                    <Column  align="center" fixed>
                                                        <HeaderCell>Date</HeaderCell>
                                                        <Cell dataKey="date"/>
                                                    </Column>

                                                    <Column >
                                                        <HeaderCell>Name</HeaderCell>
                                                        <Cell dataKey="name"/>
                                                    </Column>

                                                    <Column fixed="right">
                                                        <HeaderCell>...</HeaderCell>

                                                        <Cell style={{padding: '6px'}}>
                                                            {rowData => (
                                                                <IconButton icon={<FaTrashCan/>} appearance="default"
                                                                            onClick={() => alert(`date to remove:${rowData.date}`)}/>
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
                                <DatePicker />
                                <Input placeholder="Default Input" />
                            </Card.Body>
                            <Card.Footer>
                                <IconButton icon={<PlusIcon />}>Add</IconButton>
                            </Card.Footer>
                        </Card>
                    </FlexboxGrid.Item>
                </FlexboxGrid>
            </div>
        </>
    )
}
export default HolidaysScreen