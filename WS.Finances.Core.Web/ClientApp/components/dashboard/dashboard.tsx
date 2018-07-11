import * as React from 'react';
import * as moment from 'moment';
import { RouteComponentProps } from 'react-router';
import { IYearSummary } from '../summary/IYearSummary';
import { HttpService } from '../../services/httpService';
import { YearSummaryGrid } from '../summary/yearSummaryGrid';

export interface IDashboardState {
    summary?: IYearSummary[];
    year?: number;
}

export class Dashboard extends React.Component<RouteComponentProps<{}>, IDashboardState> {
    
    constructor(props: any) {
        super(props);
        this.state = {
            summary: undefined,
            year: moment().subtract(1, "months").year()
        };
    }

    public componentDidMount(): void {
        HttpService.get<IYearSummary[]>(`/api/summary/${this.state.year}`, summary => {

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
    public render() {
        return (
            <div>
            <h2>Welcome to the dashboard for {this.state.year}</h2>
            <YearSummaryGrid summary={this.state.summary} year={this.state.year} />
        </div>
     );
    }
}
