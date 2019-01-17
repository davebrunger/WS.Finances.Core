import * as React from "react";
import { Link } from "react-router-dom";
import { ITransaction } from "./ITransaction";
import { TransactionState } from "./transactionState";
import { Currency } from "../utilities/currency";
import { DateAndTime, DateAndTimeFormat } from "../utilities/dateAndTime";
import * as moment from "moment";

export interface ITransactionGridRowProps {
    transactionState: string;
    index: number;
    transaction: ITransaction;
    selected: boolean;
    selectedChanged: (index: number, selected: boolean) => void;
}

export interface ITransactionGridRowState {
    selected: boolean;
}

export class TransactionGridRow extends React.Component<ITransactionGridRowProps, ITransactionGridRowState> {

    constructor(props: ITransactionGridRowProps) {
        super(props);
        this.state = { selected: props.selected };
        this.handlSelectedChange = this.handlSelectedChange.bind(this);
    }

    private handlSelectedChange(event: React.FormEvent<HTMLInputElement>) {
        const newValue = event.currentTarget.checked;
        if (newValue !== this.state.selected) {
            this.setState({ selected: newValue });
            this.props.selectedChanged(this.props.index, newValue);
        }
    }

    public componentWillReceiveProps(nextProps: ITransactionGridRowProps) {
        if (nextProps.transaction.description !== this.props.transaction.description) {
            this.setState({ selected: nextProps.selected });
        }
    }

    public render() {

        const mapLink = `/transactions/${this.props.transaction.year}/${this.props.transaction.month}/${this.props.transaction.accountName}/${this.props.transaction.transactionID}`;

        const category = this.props.transaction.category
            ? <span>{this.props.transaction.category}</span>
            : <Link to={mapLink} className="btn btn-default btn-xs">Map</Link>;

        const checkbox = this.props.transactionState === TransactionState.mapped
            ? <span>&nbsp;</span>
            : <input type="checkbox" checked={this.state.selected} onChange={this.handlSelectedChange} />

        return (
            <tr>
                <td><DateAndTime date={moment(this.props.transaction.timestamp).toDate()} format={DateAndTimeFormat.DateOnly}/></td>
                <td>{checkbox}</td>
                <td>{category}</td>
                <td>{this.props.transaction.description}</td>
                <td className="currency"><Currency value={this.props.transaction.moneyIn} /></td>
                <td className="currency"><Currency value={this.props.transaction.moneyOut} invertColours={true} /></td>
            </tr>
        );
    }
}
