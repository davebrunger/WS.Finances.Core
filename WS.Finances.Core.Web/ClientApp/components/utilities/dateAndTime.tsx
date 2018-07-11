import * as React from "react";
import * as moment from "moment";

export interface IDateAndTimeProps {
    date: Date;
}

export class DateAndTime extends React.Component<IDateAndTimeProps, {}> {
    public render() {
        return (
            <span>
                {moment(this.props.date).format("DD/MM/YYYY HH:mm:ss")}
            </span>
        );
    }
}
