import * as React from "react";
import { RouteComponentProps, Redirect } from "react-router-dom";
import { HttpService } from "../../services/httpService";
import { IAccount } from "../accounts/IAccount";

export interface ICurrentTransactionsState {
    redirectUrl?: string;
}

export class CurrentTransactions extends React.Component<RouteComponentProps<{}>, ICurrentTransactionsState> {

    constructor(props: any) {
        super(props);
        this.state = {};
    }

    public componentDidMount(): void {
        HttpService.get<IAccount[]>("/api/accounts", accounts => {
            const date = new Date();
            let year = date.getFullYear();
            let month = date.getMonth();
            if (month === 0) {
                year = year - 1;
                month = 12;
            }
            this.setState({ redirectUrl: `/transactions/${year}/${month}/${accounts[0].name}` });
        });
    }

    public render() {
        if (this.state.redirectUrl) {
            return (
                <Redirect to={this.state.redirectUrl} />
            );
        }
        return (
            <div>Please Wait...</div>
        );
    }
}
