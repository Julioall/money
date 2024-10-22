import { CategoryType } from "../enums/categoryType.enum";

export class Category {
    id?: string;
    name!: string;
    description?: string;
    type!: CategoryType;
    icon?: string;
    background?: string;
}
