import * as React from "react";
import * as moment from "moment";

export enum DateAndTimeFormat {
    DateOnly,
    DateAndTime
}

export interface IDateAndTimeProps {
    date: Date;
    format: DateAndTimeFormat;
}

export class DateAndTime extends React.Component<IDateAndTimeProps, {}> {
    public render() {
        var format: string;
        switch (this.props.format) {
            case DateAndTimeFormat.DateOnly:
                format = "DD/MM/YYYY";
                break;
            case DateAndTimeFormat.DateOnly:
                format = "DD/MM/YYYY HH:mm:ss";
                break;
            default:
                throw `Unrecognised DateAndTimeFormat: ${this.props.format}`; 
        }
        return (
            <span>
                {moment(this.props.date).format(format)}
            </span>
        );
    }
}
