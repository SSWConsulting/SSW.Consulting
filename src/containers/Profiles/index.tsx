import React, { useEffect, PropsWithChildren } from 'react';
import { Table } from 'components';
import { useGlobalState } from 'lightweight-globalstate';
import { State } from 'store';
import { TableCell, TableRow, Typography } from '@material-ui/core';
import { fetchData } from 'utils';
import { StaffListViewModel, StaffClient, AddStaffMemberProfileCommand, StaffDto } from 'services';
import { useAuthenticatedClient } from 'hooks';
import { getTrailingCommentRanges } from 'typescript';
import { ProfileTableRow } from './components/ProfileTableRow';
import { withRouter, RouteComponentProps } from 'react-router-dom';
import AddProfile from './components/AddProfile';

const Profiles = (props: PropsWithChildren<RouteComponentProps>): JSX.Element => {
    const { history } = props;

    const [state, updateState] = useGlobalState<State>();
    const client: StaffClient = useAuthenticatedClient<StaffClient>(state.staffClient, state.token);

    const getProfiles = async () => {
        const response = await fetchData<StaffListViewModel>(() => client.get());
        response && response.staff && updateState({ staffProfiles: response.staff.sort((a, b) => ((a.name as string) > (b.name as string)) ? 1 : -1) });
    };

    const goToUser = (name: string) => {
        history.push(`/profiles/${name}`);
    };

    useEffect(() => {
        client && getProfiles();
    }, [client]);

    const addProfile = async (staffMember: AddStaffMemberProfileCommand) => {
        const response = await fetchData<StaffDto>(() => client.addStaffMemberProfile(staffMember));
        response && updateState({ staffProfiles: [...state.staffProfiles, response] });
    };

    return (
        <>
            <h1>Profiles</h1>
            <Typography>All staff profiles available for editing</Typography>
            <AddProfile addProfile={(v) => addProfile(v)} />
            <Table items={['Name', 'Title']}>
                {state.staffProfiles && state.staffProfiles.map((r, i) => <ProfileTableRow key={i} profile={r} onClick={goToUser} />)}
            </Table>
        </>
    );
};

export default Profiles;
