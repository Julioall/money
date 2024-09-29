import { UserAccount } from "./userAccount";
import { UserFinance } from "./userFinance";
import { UserProfile } from "./userProfile";

export class User {
    id?: string;
    account?: UserAccount;
    profile?: UserProfile;
    finance?: UserFinance;
}