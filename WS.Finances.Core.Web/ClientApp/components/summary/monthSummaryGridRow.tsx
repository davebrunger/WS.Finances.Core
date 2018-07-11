import * as React from "react";
import { IMonthSummary } from "./IMonthSummary";
import { Currency } from "../utilities/currency";

export interface IMonthSummaryGridRowProps {
    summary: IMonthSummary;
}

export class MonthSummaryGridRow extends React.Component<IMonthSummaryGridRowProps, {}> {
    public render() {
        return (
            <tr>
                <td>{this.props.summary.key.category}</td>
                <td className="currency"><Currency value={this.props.summary.value} /></td>
            </tr>
        );
    }
}