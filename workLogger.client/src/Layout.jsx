import React from 'react';
import { Outlet, Link } from 'react-router-dom';
import { useDispatch } from 'react-redux'
import {
    Container,
    Header,
    Content,
    Sidebar,
    Sidenav,
    Nav,
    Stack,
    IconButton,
    HStack,
    Text, Navbar
} from 'rsuite';
import GearIcon from '@rsuite/icons/Gear';
import OffRoundIcon from '@rsuite/icons/OffRound';
import {Icon} from "@rsuite/icons";
import {
    MdDashboard,
    MdGroup,
    MdKeyboardArrowLeft,
    MdOutlineKeyboardArrowRight,
} from "react-icons/md";
import { MdHolidayVillage } from "react-icons/md";
import {FaReact} from "react-icons/fa";
import { logout } from './AuthSlice'

const Layout = ({ children }) => {
    const dispatch = useDispatch()
    const [expand, setExpand] = React.useState(true);
    
    const NavToggle = ({ expand, onChange }) => {
        return (
            <Stack className="nav-toggle" justifyContent={expand ? 'flex-end' : 'center'}>
                <IconButton
                    onClick={onChange}
                    appearance="subtle"
                    size="lg"
                    icon={expand ? <MdKeyboardArrowLeft /> : <MdOutlineKeyboardArrowRight />}
                />
            </Stack>
        );
    };

    const Brand = ({ expand }) => {
        return (
            <HStack className="page-brand" spacing={12}>
                <FaReact size={26} />
                {expand && <Text>WorkLogger</Text>}
            </HStack>
        );
    };

    return (
        <Container className="app">
            <Sidebar
                style={{ display: 'flex', flexDirection: 'column' }}
                width={expand ? 260 : 56}
                collapsible
            >
                <Sidenav.Header>
                    <Brand expand={expand} />
                </Sidenav.Header>
                <Sidenav expanded={expand} defaultOpenKeys={['3']} appearance="subtle">
                    <Sidenav.Body>
                        <Nav defaultActiveKey="1">
                            <Nav.Item eventKey="1" icon={<Icon as={MdDashboard} />} as={Link} to="/">
                                Dashboard
                            </Nav.Item>
                            <Nav.Item eventKey="2" icon={<Icon as={MdGroup} />} as={Link} to="/month">
                                Month
                            </Nav.Item>
                            <Nav.Item eventKey="3" icon={<Icon as={MdHolidayVillage} />} as={Link} to="/holidays">
                                Holidays
                            </Nav.Item>
                            <Nav.Item eventKey="2" icon={<Icon as={MdGroup} />} as={Link} to="/users">
                                Users
                            </Nav.Item>
                        </Nav>
                    </Sidenav.Body>
                </Sidenav>
                <NavToggle expand={expand} onChange={() => setExpand(!expand)} />
            </Sidebar>
            <Content>
                <Container>
                    <Header>
                        <Navbar>
                            <Nav pullRight>
                                <Nav.Item icon={<OffRoundIcon />} onClick={() => dispatch(logout())}>Logout</Nav.Item>
                            </Nav>
                        </Navbar>
                    </Header>
                    <Outlet />
                </Container>
            </Content>
        </Container>
    );
};

export default Layout;
