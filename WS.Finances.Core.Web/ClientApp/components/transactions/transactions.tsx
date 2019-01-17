import * as React from "react";
import { ITransaction } from "./ITransaction";
import { TransactionGrid } from "./transactionGrid";
import { HttpService } from "../../services/httpService";
import { IAccount } from "../accounts/IAccount";
import { Month } from "../utilities/month";
import { DateService } from "../../services/dateService";
import { IMap } from "../map/IMap";
import { RouteComponentProps, Redirect } from "react-router-dom";
import * as moment from "moment";

export interface ITransactionsProps {
    year: string;
    month: string;
    accountName: string;
}

export interface ITransactionsState {
    unmapped?: ITransaction[];
    mapped?: ITransaction[];
    accountNames?: string[];
    selected?: boolean[];
    map?: IMap[];
    category?: string;
    redirectUrl?: string;
}

export class Transactions extends React.Component<RouteComponentProps<ITransactionsProps>, ITransactionsState> {

    constructor(props: any) {
        super(props);
        this.state = {};
        this.handleYearChange = this.handleYearChange.bind(this);
        this.handleMonthChange = this.handleMonthChange.bind(this);
        this.handleAccountNameChange = this.handleAccountNameChange.bind(this);
        this.handlSelectedChange = this.handlSelectedChange.bind(this);
        this.handleMapClick = this.handleMapClick.bind(this);
        this.handlCategoryChange = this.handlCategoryChange.bind(this);
    }

    private getTransactions() {
        this.setState({ redirectUrl: undefined });
        const params = this.props.match.params;
        const transactionsUrl = `/api/transactions/${params.year}/${params.month}/${params.accountName}`;
        HttpService.get<ITransactionsState>(transactionsUrl,
            data => {
                const mapped = this.sortByDate(data.mapped || []);
                const unmmapped = this.sortByDescription(data.unmapped || []);
                this.setState({
                    mapped: mapped,
                    unmapped: unmmapped,
                    selected: unmmapped.map(() => false)
                });
            });
    }

    private sortByDate(transactions: ITransaction[]) {
        return transactions.sort((a, b) => moment(a.timestamp).toDate().valueOf() - moment(b.timestamp).toDate().valueOf());
    }

    private sortByDescription(transactions: ITransaction[]) {
        return transactions.sort((a, b) => a.description < b.description ? -1 : (a.description > b.description ? 1 : 0));
    }

    private handleYearChange(event: React.FormEvent<HTMLSelectElement>) {
        const params = this.props.match.params;
        if (event.currentTarget.value !== params.year) {
            this.setState({ unmapped: undefined, mapped: undefined });
            this.setState({ redirectUrl: `/transactions/${event.currentTarget.value}/${params.month}/${params.accountName}` });
        }
    }

    private handleMonthChange(event: React.FormEvent<HTMLSelectElement>) {
        const params = this.props.match.params;
        if (event.currentTarget.value !== params.month) {
            this.setState({ unmapped: undefined, mapped: undefined });
            this.setState({ redirectUrl: `/transactions/${params.year}/${event.currentTarget.value}/${params.accountName}` });
        }
    }

    private handleAccountNameChange(event: React.FormEvent<HTMLSelectElement>) {
        const params = this.props.match.params;
        if (event.currentTarget.value !== params.accountName) {
            this.setState({ unmapped: undefined, mapped: undefined });
            this.setState({ redirectUrl: `/transactions/${params.year}/${params.month}/${event.currentTarget.value}` });
        }
    }

    private handlSelectedChange(index: number, selected: boolean) {
        const newState = (this.state.selected || []).map((s, i) => i === index ? selected : s);
        this.setState({ selected: newState });
    }

    private handleMapClick() {
        const selectedTransactions = (this.state.unmapped || [])
            .filter((_, i) => (this.state.selected || [])[i])
            .map(t => t.transactionID);
        const params = this.props.match.params;
        const mapUrl = `/api/transactions/map/${params.year}/${params.month}/${params.accountName}`;
        HttpService.post(mapUrl,
            {
                transactionIds: selectedTransactions,
                category: this.state.category
            },
            undefined,
            () => {
                this.getTransactions();
            },
            (status, responseText) => alert(`Error mapping transaction ${status}: ${responseText}`));
    }

    private handlCategoryChange(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ category: event.currentTarget.value });
    }

    public componentDidMount(): void {
        HttpService.get<IAccount[]>("/api/accounts",
            accounts => {
                const accountNames = accounts.map(a => a.name);
                this.setState({ accountNames: accountNames });
            });
        HttpService.get<IMap[]>("/api/map", map => {
            map.sort((s1, s2) => {
                if (s1.section > s2.section) {
                    return -1;
                } else if (s1.section < s2.section) {
                    return 1;
                } else {
                    return s1.position - s2.position;
                }
            });
            this.setState({ map: map, category: map[0].category });
        });
        this.getTransactions();
    }

    public componentDidUpdate(prevProps: RouteComponentProps<ITransactionsProps>) {
        if (prevProps.match.params.year !== this.props.match.params.year ||
            prevProps.match.params.month !== this.props.match.params.month ||
            prevProps.match.params.accountName !== this.props.match.params.accountName) {
            this.getTransactions();
        }
    }

    public render() {
        if (this.state.redirectUrl) {
            return (
                <Redirect to={this.state.redirectUrl} />
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

        const accountNameOptions = this.state.accountNames
            ? this.state.accountNames.map(a => <option key={a} value={a}>{a}</option>)
            : [];

        const categories = this.state.map
            ? this.state.map.map(m => <option key={m.category} value={m.category}>{m.category}</option>)
            : [];

        const mapButtonDisabled = !(this.state.selected && this.state.selected.filter(v => v).length > 0 && this.state.category);

        const mapAllSection = this.state.unmapped && this.state.unmapped.length > 0
            ? [
                <button key="button" type="button" className="btn btn-primary" onClick={this.handleMapClick} disabled={mapButtonDisabled}>Map Checked As</button>,
                <div key="select" className="form-group">
                    <label className="sr-only" htmlFor="category">Category</label>
                    <select className="form-control" id="category" name="category" value={this.state.category} onChange={this.handlCategoryChange}>
                        {categories}
                    </select>
                </div>
            ]
            : null;

        return (
            <div>
                <h2>Transactions in&nbsp;<Month monthNumber={parseInt(params.month, 10)} />&nbsp;{params.year}&nbsp;for&nbsp;{params.accountName}
                </h2>
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
                    <div className="form-group">
                        <label className="sr-only">Account Name</label>
                        <select className="form-control" value={params.accountName}
                            onChange={this.handleAccountNameChange}>
                            {accountNameOptions}
                        </select>
                    </div>
                    {mapAllSection}
                </form>
                <TransactionGrid mapped={this.state.mapped} unmapped={this.state.unmapped} selected={this.state.selected} selectedChanged={this.handlSelectedChange} />
            </div>
        );
    }
}
