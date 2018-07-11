import * as React from "react";
import {DateService} from "../../services/dateService";

export interface IMonthProps {
    monthNumber: number;
    shortMonthFormat?: boolean;
}

export class Month extends React.Component<IMonthProps, {}> {
    public render() {
        return (
            <span>
                {DateService.getMonthName(this.props.monthNumber, this.props.shortMonthFormat)}
            </span>
        );
    }
}
