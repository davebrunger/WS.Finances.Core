import * as React from "react";
import { Link } from "react-router-dom";
import { ITransaction } from "./ITransaction";
import { TransactionState } from "./transactionState";
import { Currency } from "../utilities/currency";
import { DateAndTime } from "../utilities/dateAndTime";

export interface ITransactionGridRowProps {
    transactionState: string;
    index: number;
    transaction: ITransaction;
    selectedChanged : (index : number, selected : boolean) => void;  
}

export interface ITransactionGridRowState {
    selected: boolean;
}

export class TransactionGridRow extends React.Component<ITransactionGridRowProps, ITransactionGridRowState> {

    constructor(props: ITransactionGridRowProps) {
        super(props);
        this.state = {selected:false};
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
            this.setState({ selected: false });
        }
    }

    public render() {

        const mapLink = `/transactions/${this.props.transaction.year}/${this.props.transaction.month}/${this.props.transaction.accountName}/${this.props.transaction.transactionID}`;

        const category = this.props.transaction.category
            ? <span>{this.props.transaction.category}</span>
            : <Link to={mapLink} className="btn btn-default btn-xs">Map</Link>;

        const checkbox = this.props.transactionState === TransactionState.mapped
            ? <span>&nbsp;</span>
            : <input type="checkbox" checked={this.state.selected} onChange={this.handlSelectedChange}/>

        return (
            <tr>
                <td><DateAndTime date={this.props.transaction.timestamp} /></td>
                <td>{checkbox}</td>
                <td>{category}</td>
                <td>{this.props.transaction.description}</td>
                <td className="currency"><Currency value={this.props.transaction.moneyIn} /></td>
                <td className="currency"><Currency value={this.props.transaction.moneyOut} invertColours={true} /></td>
            </tr>
        );
    }
}
