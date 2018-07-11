import * as React from "react";
import { IYearSummary } from "./IYearSummary";
import { Currency } from "../utilities/currency";

export interface IYearSummaryGridRowProps {
    summary: IYearSummary;
}

export class YearSummaryGridRow extends React.Component<IYearSummaryGridRowProps, {}> {
    public render() {
        const valueCells = this.props.summary.value.map((n, i) => (
            <td key={i} className="currency"><Currency value={n} /></td>
        ));
        return (
            <tr>
                <td>{this.props.summary.key.category}</td>
                {valueCells}
            </tr>
        );
    }
}