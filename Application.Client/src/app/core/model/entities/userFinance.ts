import { Account } from "./account";
import { Category } from "./category";
import { CreditCard } from "./creditCard";

export class UserFinance {
    accounts: Account[] = [];
    creditCards: CreditCard[] = [];
    categories: Category[] = [];
    tags: string[] = [];
    transactionsIds: string[] = [];
}
