import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { HttpService } from '../../services/httpService';
import { IAccount } from './IAccount';
import { AccountGrid } from './accounyGrid';

export interface IAccountsState {
    accounts?: IAccount[];
}

export class Accounts extends React.Component<RouteComponentProps<{}>, IAccountsState> {
    constructor(props : any) {
        super(props);
        this.state = { accounts: undefined };
    }

    public componentDidMount(): void {
        HttpService.get<IAccount[]>("/api/accounts", accounts => {
            this.setState({ accounts: accounts });
        });
    }

    public render() {
        return (
            <div>
                <h2>Accounts</h2>
                <AccountGrid accounts={this.state.accounts} />
            </div>
        );
    }
}
