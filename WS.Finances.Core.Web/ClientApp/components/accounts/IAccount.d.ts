export interface IAccount {
    name: string;
    startRow: number;
    timestampColumn: number;
    timestampFormat: string;
    descriptionColumn: number;
    moneyInColumn?: number;
    moneyOutColumn?: number;
    totalColumn?: number;
}