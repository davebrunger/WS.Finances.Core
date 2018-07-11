import * as React from "react";
import { RouteComponentProps, Redirect } from "react-router-dom";

export class CurrentMonthSummary extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {

        const date = new Date();
        let year = date.getFullYear();
        let month = date.getMonth();
        if (month === 0) {
            year = year - 1;
            month = 12;
        }
        var redirectUrl = `/summary/${year}/${month}`;

        return (
            <Redirect to={redirectUrl} />
        );
    }
}
