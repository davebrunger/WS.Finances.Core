import * as React from "react";
import { RouteComponentProps } from "react-router";
import { HttpService } from "../../services/httpService";
import { ITransaction } from "./ITransaction";
import { TransactionGrid } from "./transactionGrid";

export interface ISearchState {
    year?: number;
    description?: string;
    mapped?: ITransaction[];
    unmapped?: ITransaction[];
    transactionCount?: number;
}

export class Search extends React.Component<RouteComponentProps<{}>, ISearchState> {

    constructor(props: any) {
        super(props);
        this.state = { year: new Date().getFullYear() };
        this.handleYearChange = this.handleYearChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
    }

    private performSearch() {
        var year = this.state.year || "";
        var description = encodeURIComponent(this.state.description || "");
        const transactionsUrl = `/api/transactions/search?year=${year}&descriptionPattern=${description}`;
        HttpService.get<ISearchState>(transactionsUrl,
            data => {
                const mapped = this.sortByDate(data.mapped || []);
                const unmmapped = this.sortByDate(data.unmapped || []);
                this.setState({
                    mapped: mapped,
                    unmapped: unmmapped,
                    transactionCount: data.transactionCount
                });
            });
    }

    private sortByDate(transactions: ITransaction[]) {
        return transactions.sort((a, b) => a.timestamp.valueOf() - b.timestamp.valueOf());
    }

    private isValidRegexPattern(regexPattern: string) {
        try {
            new RegExp(regexPattern);
        } catch (e) {
            return false;
        }
        return true;
    }

    private handleYearChange(event: React.FormEvent<HTMLSelectElement>) {
        var newYear: number | undefined = undefined;
        if (event.currentTarget.value) {
            newYear = parseInt(event.currentTarget.value, 10);
        }
        if (newYear !== this.state.year) {
            this.setState({ year: newYear }, () => this.performSearch());
        }
    }

    private handleDescriptionChange(event: React.FormEvent<HTMLInputElement>) {
        var newDescription: string | undefined = undefined;
        if (event.currentTarget.value && this.isValidRegexPattern(event.currentTarget.value)) {
            newDescription = event.currentTarget.value;
        }
        if (newDescription !== this.state.description) {
            this.setState({ description: newDescription }, () => this.performSearch());
        }
    }

    public componentDidMount() {
        this.performSearch();
    }

    public render() {
        const years: JSX.Element[] = [];
        const date = new Date();
        for (let i = date.getFullYear() - 20; i < date.getFullYear() + 5; i++) {
            years.push(<option key={i} value={i}>{i}</option>);
        }

        var message = "The search form will display the first 100 matching transactions";
        if ((this.state.transactionCount || 0) > 100) {
            message = `Displaying the first 100 of ${this.state.transactionCount} matching transactions`;
        }
        else if ((this.state.transactionCount || 0) > 1) {
            message = `Displaying all ${this.state.transactionCount} matching transactions`;
        }
        else if (this.state.transactionCount === 1) {
            message = "Displaying 1 matching transaction";
        }

        return (
            <div>
                <h2>Search Transactions</h2>
                <form className="form-inline">
                    <div className="form-group">
                        <label>Search for:&nbsp;</label>
                    </div>
                    <div className="form-group">
                        <label className="sr-only">Description Pattern</label>
                        <input type="text" className="form-control" value={this.state.description}
                            onChange={this.handleDescriptionChange} />
                    </div>
                    <div className="form-group">
                        <label>&nbsp;in year:&nbsp;</label>
                    </div>
                    <div className="form-group">
                        <label className="sr-only">Year</label>
                        <select className="form-control" value={this.state.year} onChange={this.handleYearChange}>
                            {years}
                        </select>
                    </div>
                </form>
                <p>
                    {message}
                </p>
                <TransactionGrid mapped={this.state.mapped} unmapped={this.state.unmapped} noTransactionMessageSuffixOverride={"that match the search criteria"} />
            </div>
        );
    }
}
