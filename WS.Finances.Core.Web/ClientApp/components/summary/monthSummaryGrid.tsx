import * as React from "react";
import { IMonthSummary } from "./IMonthSummary";
import { MonthSummaryGridRow } from "./monthSummaryGridRow";
import { MonthSummaryTotalRow } from "./monthSummaryTotalRow";

export interface IMonthSummaryGridProps {
    summary?: IMonthSummary[];
}

export class MonthSummaryGrid extends React.Component<IMonthSummaryGridProps, {}> {
    public render() {

        let rows: JSX.Element[];
        if (!this.props.summary) {
            rows = [<tr key={1}><td colSpan={2}>Please wait...</td></tr>];
        } else if (this.props.summary.length === 0) {
            rows = [<tr key={1}><td colSpan={2}>No summary data found</td></tr>];
        } else {
            var section = "";
            var total: number | null = null;
            var outgoingsTotal : number | null = null;
            rows = this.props.summary.map(s => {
                    var result: JSX.Element[] = [];
                    if (s.key.section !== section) {
                        if (total || total === 0) {
                            result.push(<MonthSummaryTotalRow key={`${section} total`} total={total}/>);
                        }
                        section = s.key.section;
                        result.push(<tr key={section}><th colSpan={2}>{section}</th></tr>);
                        // ReSharper disable once UnusedParameter
                        total = 0;
                    }
                    result.push(<MonthSummaryGridRow key={s.key.category} summary={s}/>);
                    total = (total || 0) + s.value;
                    if (section !== "Income") {
                        outgoingsTotal = (outgoingsTotal || 0) + s.value;
                    }
                    return result;
                })
                .reduce((a, b) => a.concat(b));
            if (total || total === 0) {
                rows.push(<MonthSummaryTotalRow key={`${section} total`} total={total || 0} />);
            }
            if (outgoingsTotal || outgoingsTotal === 0) {
                rows.push(<tr key="Outgoings"><th colSpan={2}>Outgoings</th></tr>);
                rows.push(<MonthSummaryTotalRow key="Outgoings Total" total={outgoingsTotal || 0} />);
            }
        }

        return (
            <table className="table table-condensed table-striped">
                <thead>
                <tr>
                    <th>
                        Category
                    </th>
                    <th className="currency">
                        Amount
                    </th>
                </tr>
                </thead>
                <tbody>
                {rows}
                </tbody>
            </table>
        );
    }
}