import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './Layout';
import { Dashboard } from './dashboard/Dashboard';
import { Accounts } from './accounts/accounts';
import { Upload } from './files/upload';
import { MonthSummary } from './summary/monthSummary';
import { CurrentMonthSummary } from './summary/currentMonthSummary';
import { CurrentTransactions } from './transactions/currentTransactions';
import { Transactions } from './transactions/transactions';
import { MapTransaction } from './transactions/mapTransaction';
import { Search } from './transactions/search';

export const routes = (
    <Layout>
        <Route exact path='/' component={Dashboard} />
        <Route exact path='/accounts' component={Accounts} />
        <Route exact path='/upload' component={Upload} />
        <Route exact path='/summary' component={CurrentMonthSummary} />
        <Route exact path='/summary/:year/:month' component={MonthSummary} />
        <Route exact path='/transactions' component={CurrentTransactions} />
        <Route exact path='/transactions/:year/:month/:accountName' component={Transactions} />
        <Route exact path='/transactions/:year/:month/:accountName/:transactionId' component={MapTransaction} />
        <Route exact path='/search' component={Search} />
    </Layout>
);
