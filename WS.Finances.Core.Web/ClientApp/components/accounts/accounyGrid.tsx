import * as React from "react";
import { IAccount } from "./IAccount";
import { AccountGridRow } from "./accountGridRow";

export interface IAccountGridProps {
    accounts?: IAccount[];
}

export class AccountGrid extends React.Component<IAccountGridProps, {}> {
    public render() {
        let accounts: JSX.Element[];
        if (!this.props.accounts) {
            accounts = [<tr key={0}><td colSpan={8}>Please wait...</td></tr>];
        } else if (this.props.accounts.length === 0) {
            accounts = [<tr key={0}><td colSpan={8}>No accounts found</td></tr>];
        } else {
            accounts = this.props.accounts.map(a => (<AccountGridRow key={a.name} account={a} />));
        }

        return (
            <table className="table table-condensed">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Start Row</th>
                        <th>Timestamp Column</th>
                        <th>Timestamp Format</th>
                        <th>Description Column</th>
                        <th>Money In Column</th>
                        <th>Money Out Column</th>
                        <th>Total Column</th>
                    </tr>
                </thead>
                <tbody>
                    {accounts}
                </tbody>
            </table>
        );
    }
}