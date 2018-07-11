import * as React from "react";
import { YearSummaryGridRow as SummaryGridRow } from "./yearSummaryGridRow";
import { YearSummaryTotalRow as SummaryTotalRow } from "./yearSummaryTotalRow";
import { IYearSummary } from "./IYearSummary";
import { Month } from "../utilities/month";
import { NavLink } from "react-router-dom";

export interface IYearSummaryGridProps {
    year? : number;
    summary?: IYearSummary[];
}

export class YearSummaryGrid extends React.Component<IYearSummaryGridProps, {}> {
    public render() {

        let rows: JSX.Element[];
        if (!this.props.summary) {
            rows = [<tr key={1}><td colSpan={13}>Please wait...</td></tr>];
        } else if (this.props.summary.length === 0) {
            rows = [<tr key={1}><td colSpan={13}>No summary data found</td></tr>];
        } else {
            var section = "";
            var totals : number[] | null = null;
            rows = this.props.summary.map(s => {
                var result: JSX.Element[] = [];
                if (s.key.section !== section) {
                    if (totals) {
                        result.push(<SummaryTotalRow key={`${section} total`} totals={totals} />);
                    }
                    section = s.key.section;
                    result.push(<tr key={section}><th colSpan={13}>{section}</th></tr>);
                    // ReSharper disable once UnusedParameter
                    totals = s.value.map(v => 0);
                }
                result.push(<SummaryGridRow key={s.key.category} summary={s} />);
                if (totals)
                {
                    totals = totals.map((t, i) => t + (s.value[i] || 0));
                }
                return result;
            }).reduce((a, b) => a.concat(b));
            if (totals) {
                rows.push(<SummaryTotalRow key={`${section} total`} totals={totals} />);
            }
        }

        const months: JSX.Element[] = [];
        for (let i = 1; i <= 12; i++) {
            months.push(<th key={i} className="currency"><NavLink to={`/summary/${this.props.year}/${i}`}><Month monthNumber={i} shortMonthFormat={true}/></NavLink></th>);
        }

        return (
            <table className="table table-condensed table-striped">
                <thead>
                    <tr>
                        <th>&nbsp;</th>
                        {months}
                    </tr>
                </thead>
                <tbody>
                    {rows}
                </tbody>
            </table>
        );
    }
}