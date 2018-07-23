import * as React from "react";
import { ITransaction } from "./ITransaction";
import { TransactionGridRow } from "./transactionGridRow";
import { TransactionState } from "./transactionState";

export interface ITransactionGridProps {
    unmapped?: ITransaction[];
    mapped?: ITransaction[];
    selectedChanged?: (index: number, selected: boolean) => void;
    noTransactionMessageSuffixOverride?: string;
}

export class TransactionGrid extends React.Component<ITransactionGridProps, {}> {

    constructor(props: ITransactionGridProps) {
        super(props);
        this.handlSelectedChange = this.handlSelectedChange.bind(this);
    }

    private handlSelectedChange(index: number, selected: boolean) {
        if (this.props.selectedChanged) {
            this.props.selectedChanged(index, selected);
        }
    }

    private pushTransactionRows(source: ITransaction[], transactionState: string, target: JSX.Element[]) {
        target.push(<tr key={transactionState}><th colSpan={6}>{transactionState} Transactions</th></tr>);
        if ((!source) || (source.length === 0)) {
            var suffix = this.props.noTransactionMessageSuffixOverride || "for this month";
            var message = `There are no ${transactionState.toLowerCase()} transactions ${suffix}`;
            target.push(
                <tr key={`${transactionState} none`}>
                    <td colSpan={6}>{message}</td>
                </tr>
            );
        } else {
            source.forEach((transaction, index) => target.push(
                <TransactionGridRow key={`${transactionState} ${index}`} transactionState={transactionState} index={index} transaction={transaction} selectedChanged={this.handlSelectedChange} />
            ));
        }
    }

    public componentWillReceiveProps(nextProps: ITransactionGridProps) {
        if (nextProps.unmapped) {
            this.setState({ selected: nextProps.unmapped.map(() => false) });
        }
    }

    public render() {
        const rows: JSX.Element[] = [];

        if (!(this.props.unmapped || this.props.mapped)) {
            rows.push(<tr key={1}><td colSpan={5}>Please Wait...</td></tr>);
        } else {
            this.pushTransactionRows(this.props.unmapped || [], TransactionState.unmapped, rows);
            this.pushTransactionRows(this.props.mapped || [], TransactionState.mapped, rows);
        }


        return (
            <form>
                <table className="table table-condensed table-striped">
                    <thead>
                        <tr>
                            <th>Date &amp; Time</th>
                            <th colSpan={2}>Category</th>
                            <th>Description</th>
                            <th className="currency">Money In</th>
                            <th className="currency">Money Out</th>
                        </tr>
                    </thead>
                    <tbody>
                        {rows}
                    </tbody>
                </table>
            </form>
        );
    }
}