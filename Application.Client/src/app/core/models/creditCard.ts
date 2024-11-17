export class CreditCard {
    id?: string;
    userId!: string;
    cardName!: string;
    limit!: number;
    closingDate!: Date;
    dueDate!: Date;
    paymentAccountId!: string;
    annualFee?: number;
    iconUrl!: string;
}
