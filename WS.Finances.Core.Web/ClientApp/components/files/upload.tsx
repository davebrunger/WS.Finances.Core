import * as React from 'react';
import { RouteComponentProps, Redirect } from 'react-router';
import { HttpService } from '../../services/httpService';
import { IAccount } from '../accounts/IAccount';
import { Month } from '../utilities/month';

export interface IUploadState {
    year?: number;
    month?: number;
    accountName?: string;
    files?: FileList;
    accountNames?: string[];
    redirectUrl? : string;
}

export class Upload extends React.Component<RouteComponentProps<{}>, IUploadState> {
    constructor(props: any) {
        super(props);
        const date = new Date();
        let year = date.getFullYear();
        let month = date.getMonth();
        if (month === 0) {
            year = year - 1;
            month = 12;
        }
        this.state = { year: year, month: month };
        this.handleYearChange = this.handleYearChange.bind(this);
        this.handleMonthChange = this.handleMonthChange.bind(this);
        this.handleAccountNameChange = this.handleAccountNameChange.bind(this);
        this.handleFileChange = this.handleFileChange.bind(this);
        this.handleUploadClick = this.handleUploadClick.bind(this);
    }

    private handleYearChange(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ year: parseInt(event.currentTarget.value, 10) });
    }

    private handleMonthChange(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ month: parseInt(event.currentTarget.value, 10) });
    }

    private handleAccountNameChange(event: React.FormEvent<HTMLSelectElement>) {
        this.setState({ accountName: event.currentTarget.value });
    }

    private handleFileChange(event: React.FormEvent<HTMLInputElement>) {
        this.setState({ files: event.currentTarget.files || undefined });
    }

    private handleUploadClick() {
        if (!this.state.accountName) {
            alert("Please select an account Name");
            return;
        }
        if (!this.state.files) {
            alert("Please select a file");
            return;
        }
        HttpService.post(`/api/csv/${this.state.year}/${this.state.month}/${this.state.accountName}`,
            undefined,
            this.state.files,
            () => {
                alert("Upload complete");
                this.setState({redirectUrl : `/transactions/${this.state.year}/${this.state.month}/${this.state.accountName}`});
            },
            (status, responseText) => alert(`Error uploading file ${status}: ${responseText}`));
    }

    public componentDidMount() {
        HttpService.get<IAccount[]>("/api/accounts", accounts => {
            const accountNames = accounts.map(a => a.name);
            this.setState({ accountNames: accountNames, accountName: accountNames[0] });
        });
    }

    public render() {

        if (this.state.redirectUrl)
        {
            return (
                <Redirect to={this.state.redirectUrl} />
            );
        }

        const fileButtonText = this.state.files && this.state.files.length > 0 ? "Change" : "Browse";
        const fileHelpBlock = this.state.files && this.state.files.length > 0
            ? (<p className="help-block">{this.state.files[0].name}</p>)
            : null;

        const years: JSX.Element[] = [];
        const date = new Date();
        for (let i = date.getFullYear() - 5; i < date.getFullYear() + 5; i++) {
            years.push(<option key={i} value={i}>{i}</option>);
        }

        const months: JSX.Element[] = [];
        for (let i = 1; i <= 12; i ++) {
            months.push(<option key={i} value={i}><Month monthNumber={i}/></option>);
        }

        const accountNames = this.state.accountNames
            ? this.state.accountNames.map(a => (<option key={a} value={a}>{a}</option>))
            : [];

        return (
            <div>
                <h2>Upload File</h2>
                <form method="POST" encType="multipart/form-data">
                    <div className="form-group">
                        <label htmlFor="year">Year</label>
                        <select className="form-control" id="year" name="year" value={this.state.year} onChange={this.handleYearChange}>
                            {years}
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="month">Month</label>
                        <select className="form-control" id="month" name="month" value={this.state.month} onChange={this.handleMonthChange}>
                            {months}
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="accountName">Account Name</label>
                        <select className="form-control" id="accountName" name="accountName" value={this.state.accountName} onChange={this.handleAccountNameChange}>
                            {accountNames}
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="file">File to upload</label>
                        <div style={{ width: "100%" }}>
                            <label className="btn btn-default btn-file">
                                {fileButtonText} <input type="file" id="file" name="file" onChange={this.handleFileChange} />
                            </label>
                        </div>
                        {fileHelpBlock}
                    </div>
                    <div className="form-group">
                        <button className="btn btn-primary" type="button" id="upload" name="upload"
                            onClick={this.handleUploadClick}>
                            Upload
                    </button>
                    </div>
                </form>
            </div>
        );
    }
}
