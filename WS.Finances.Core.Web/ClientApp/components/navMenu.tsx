import * as React from 'react';
import { NavLink, } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
    public render() {
        return (
            <ul className="nav nav-pills">
                <li>
                    <NavLink to={'/'} exact activeClassName='active'>Home</NavLink>
                </li>
                <li>
                    <NavLink to={'/summary'} activeClassName='active'>Summary</NavLink>
                </li>
                <li>
                    <NavLink to={'/upload'} activeClassName='active'>Upload</NavLink>
                </li>
                <li>
                    <NavLink to={'/transactions'} activeClassName='active'>Transactions</NavLink>
                </li>
                <li>
                    <NavLink to={'/accounts'} activeClassName='active'>Accounts</NavLink>
                </li>
                <li>
                    <NavLink to={'/search'} activeClassName='active'>Search</NavLink>
                </li>
            </ul>
        );
    }
}
