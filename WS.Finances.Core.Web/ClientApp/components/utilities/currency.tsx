import * as React from "react";

export interface ICurrencyProps {
    value?: number;
    invertColours? : boolean;
}

export class Currency extends React.Component<ICurrencyProps, {}> {
    public render() {
        let className = "currency";
        const classValue = this.props.invertColours ? -(this.props.value || 0) : (this.props.value || 0);
        if (classValue < 0) {
            className = className + " currency-negative";
        } else if (classValue > 0) {
            className = className + " currency-positive";
        } else {
            className = className + " currency-zero";
        }
        const value = this.props.value || this.props.value === 0
            ? this.props.value.toFixed(2)
            : null;
        return (
            <span className={className}>{value}</span>
        );
    }
}