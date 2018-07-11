import * as React from "react";
import { RouteComponentProps, Redirect } from "react-router-dom";
import { IMonthSummary } from "./IMonthSummary";
import { Month } from "../utilities/month";
import { MonthSummaryGrid } from "./monthSummaryGrid";
import { DateService } from "../../services/dateService";
import { HttpService } from "../../services/httpService";

export interface IMonthSummaryProps {
    year: string;
    month: string;
}

export interface IMonthSummaryState {
    summary?: IMonthSummary[];
    redirectPath? : string;
}

export class MonthSummary extends React.Component<RouteComponentProps<IMonthSummaryProps>, IMonthSummaryState> {

    constructor(props: any) {
        super(props);
        this.state = { summary: undefined, redirectPath : undefined };
        this.handleYearChange = this.handleYearChange.bind(this);
        this.handleMonthChange = this.handleMonthChange.bind(this);
    }

    private handleYearChange(event: React.FormEvent<HTMLSelectElement>) {
        const params = this.props.match.params;
        if (event.currentTarget.value !== params.year) {
            this.setState({ summary: undefined, redirectPath : `/summary/${event.currentTarget.value}/${params.month}` });
        }
    }

    private handleMonthChange(event: React.FormEvent<HTMLSelectElement>) {
        const params = this.props.match.params;
        if (event.currentTarget.value !== params.month) {
            this.setState({ summary: undefined, redirectPath : `/summary/${params.year}/${event.currentTarget.value}` });
        }
    }

    private getSummary() {
        const params = this.props.match.params;
        this.setState({ redirectPath : undefined });
        HttpService.get<IMonthSummary[]>(`/api/summary/${params.year}/${params.month}`, summary => {

            summary.sort((s1, s2) => {
                if (s1.key.section > s2.key.section) {
                    return -1;
                } else if (s1.key.section < s2.key.section) {
                    return 1;
                } else {
                    return s1.key.position - s2.key.position;
                }
            });

            this.setState({ summary: summary });
        });
    }

    public componentDidMount(): void {
        this.getSummary();
    }

    public componentDidUpdate(prevProps: RouteComponentProps<IMonthSummaryProps>) {
        if (prevProps.match.params.year !== this.props.match.params.year ||
            prevProps.match.params.month !== this.props.match.params.month) {
            this.getSummary();
        }
    }

    public render() {

        if (this.state.redirectPath)
        {
            return (
                <Redirect to={this.state.redirectPath} />
            );
        }

        const params = this.props.match.params;

        const years: JSX.Element[] = [];
        const date = new Date();
        for (let i = date.getFullYear() - 20; i < date.getFullYear() + 5; i++) {
            years.push(<option key={i} value={i}>{i}</option>);
        }

        const months: JSX.Element[] = [];
        for (let i = 1; i <= 12; i++) {
            months.push(<option key={i} value={i}>{DateService.getMonthName(i)}</option>);
        }

        return (
            <div>
                <h2>Summary for <Month monthNumber={parseInt(params.month, 10)} /> {params.year}</h2>
                <form className="form-inline">
                    <div className="form-group">
                        <label>Jump To:</label>
                    </div>
                    <div className="form-group">
                        <label className="sr-only">Month</label>
                        <select className="form-control" value={params.month} onChange={this.handleMonthChange}>
                            {months}
                        </select>
                    </div>
                    <div className="form-group">
                        <label className="sr-only">Year</label>
                        <select className="form-control" value={params.year} onChange={this.handleYearChange}>
                            {years}
                        </select>
                    </div>
                </form>
                <MonthSummaryGrid summary={this.state.summary} />
            </div>
        );
    }
}
