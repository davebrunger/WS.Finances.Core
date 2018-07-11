import * as React from "react";
import { Currency } from "../utilities/currency";

export interface IYearSummaryTotalRowProps {
    totals: number[];
}

export class YearSummaryTotalRow extends React.Component < IYearSummaryTotalRowProps, {} > {
    public render() {
        const totalCells = this.props.totals.map((n, i) => (
            <th key={i} className="currency"><Currency value={n} /></th>
        ));
        return (
            <tr>
                <th>&nbsp;</th>
                {totalCells}
            </tr>
        );
    }
}