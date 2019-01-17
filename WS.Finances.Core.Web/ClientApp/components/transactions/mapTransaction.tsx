import * as React from "react";
import { HttpService } from "../../services/httpService";
import { IMap } from "../map/IMap";
import { ITransaction } from "./ITransaction";
import { Currency } from "../utilities/currency";
import { Month } from "../utilities/month";
import { DateAndTime, DateAndTimeFormat } from "../utilities/dateAndTime";
import { RouteComponentProps } from "react-router-dom";
import * as moment from "moment";

export interface IMapTransactionsProps {
    year: string;
    month: string;
    accountName: string;
    transactionId: string;
}

export interface IMapTransactionState {
    category?: string;
    map?: IMap[];
    transaction?: ITransaction;
    redirectUrl? : string;
}

export class MapTransaction extends React.Component<RouteComponentProps<IMapTransactionsProps>, IMapTransactionState> {

    constructor(props: any) {
        super(props);
        this.state = {};
        this.handlCategoryChange = this.handlCategoryChange.bind(this);
        this.handleMapClick = this.handleMapClick.bind(this);
    }

    private handlCategoryChange(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ category: event.currentTarget.value });
    }

    private handleMapClick() {
        if (!this.state.transaction) {
            alert("Please wait for transaction details to load");
            return;
        }
        if (!this.state.category) {
            alert("Please select a category");
            return;
        }
        const params = this.props.match.params;
        const mapUrl = `/api/transactions/map/${params.year}/${params.month}/${params.accountName}`;
        HttpService.post(mapUrl,
            {
                transactionIds: [this.state.transaction.transactionID],
                category: this.state.category
            },
            undefined,
            () => {
                const gridUrl = `/transactions/${params.year}/${params.month}/${params.accountName}`;
                this.setState({redirectUrl : gridUrl});
            },
            (status, responseText) => alert(`Error mapping transaction ${status}: ${responseText}`));
    }

    public componentDidMount() {
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
        const params = this.props.match.params;
        const transactionUrl = `/api/transactions/${params.year}/${params.month}/${params.accountName}/${params.transactionId}`;
        HttpService.get<ITransaction>(transactionUrl, transaction => this.setState({ transaction: transaction }));
    }

    public render() {

        const categories = this.state.map
            ? this.state.map.map(m => <option key={m.category} value={m.category}>{m.section} - {m.category}</option>)
            : [];

        let dateAndTime = <span>Please wait...</span>;
        let description = "Please wait...";
        let moneyIn = <span>Please wait...</span>;
        let moneyOut = <span>Please wait...</span>;
        if (this.state.transaction) {
            dateAndTime = <DateAndTime date={moment(this.state.transaction.timestamp).toDate()} format={DateAndTimeFormat.DateOnly}/>;
            description = this.state.transaction.description;
            moneyIn = <Currency value={this.state.transaction.moneyIn} />;
            moneyOut = <Currency value={this.state.transaction.moneyOut} invertColours={true} />;
        }

        return (
            <div>
                <h2>Map Transaction</h2>
                <form>
                    <div className="form-group">
                        <label htmlFor="year">Year</label>
                        <p className="form-control-static">{this.props.match.params.year}</p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="month">Month</label>
                        <p className="form-control-static"><Month monthNumber={parseInt(this.props.match.params.month, 10)} /></p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="accountName">Account Name</label>
                        <p className="form-control-static">{this.props.match.params.accountName}</p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="dateAndTime">Date &amp; Time</label>
                        <p className="form-control-static">{dateAndTime}</p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="description">Description</label>
                        <p className="form-control-static">{description}</p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="moneyIn">Money In</label>
                        <p className="form-control-static">{moneyIn}</p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="moneyOut">Money Out</label>
                        <p className="form-control-static">{moneyOut}</p>
                    </div>
                    <div className="form-group">
                        <label htmlFor="category">Category</label>
                        <select className="form-control" id="category" name="category" value={this.state.category} onChange={this.handlCategoryChange}>
                            {categories}
                        </select>
                    </div>
                    <div className="form-group">
                        <button type="button" className="btn btn-primary" onClick={this.handleMapClick}>Map</button>
                    </div>
                </form>
            </div>
        );
    }
}