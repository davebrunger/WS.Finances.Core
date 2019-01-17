export interface ITransaction {
    accountName: string;
    transactionID: number;
    year: number;
    month: number;
    sourceFileName: string;
    category: string;
    description: string;
    timestamp: string;
    moneyIn?: number;
    moneyOut?: number;
    balance?: number;
}