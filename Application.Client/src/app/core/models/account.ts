export class Account {
    id?: string;
    accountName?: string;
    balance: number = 0.00;
    hideBalance?: boolean = false;
    iconUrl?: string;
    bankName?: string;
    bankIspb?: string | null;
    accountType?: 'checking' | 'savings' | 'digital' | 'wallet' | 'investment' | 'other';
}
