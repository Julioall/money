import { TransactionType } from "./enums/transactionType.enum";

export class Transaction {
    id?: string;
    userId!: string;
    amount!: number;
    categoryId!: string;
    date!: Date;
    description?: string;
    type!: TransactionType;
    accountDestinationId!: string;
    creditCardDestinationId?: string;
    isRecurring: boolean = false;
    totalInstallments?: number;
    currentInstallment?: number;
    installmentAmount?: number;
    parentTransactionId?: string;
    tag?: string;
}
