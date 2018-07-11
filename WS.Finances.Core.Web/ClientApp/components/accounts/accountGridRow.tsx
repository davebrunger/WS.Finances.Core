import * as React from "react";
import { IAccount } from "./IAccount";

export interface IAccountGridRowProps {
    account: IAccount
}

export class AccountGridRow extends React.Component<IAccountGridRowProps, {}> {
    public render() {
        return (
            <tr>
                <td>{this.props.account.name}</td>
                <td>{this.props.account.startRow}</td>
                <td>{this.props.account.timestampColumn}</td>
                <td>{this.props.account.timestampFormat}</td>
                <td>{this.props.account.descriptionColumn}</td>
                <td>{this.props.account.moneyInColumn}</td>
                <td>{this.props.account.moneyOutColumn}</td>
                <td>{this.props.account.totalColumn}</td>
            </tr>
        );
    }
}
