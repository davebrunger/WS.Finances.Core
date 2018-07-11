import * as React from "react";
import { Currency } from "../utilities/currency";

export interface IMonthSummaryTotalRowProps {
    total: number;
}

export class MonthSummaryTotalRow extends React.Component<IMonthSummaryTotalRowProps, {}> {
    public render() {
        return (
            <tr>
                <th>&nbsp;</th>
                <th className="currency"><Currency value={this.props.total} /></th>
            </tr>
        );
    }
}