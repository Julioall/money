import { Component, OnInit } from '@angular/core';
import { Category } from '../../core/model/entities/category';
import { CategoriesRepositoryService } from '../../core/service/categories/categories-repository.service';
import { NotificationService } from '../../core/service/notification/notification.service';
import { CategoryType } from '../../core/model/enums/categoryType.enum';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  isExpenseCategory: boolean = true;
  isIncomeCategory: boolean = false;
  incomeCategories: Category[] = [];
  expenseCategories: Category[] = [];
  forms: { [key: string]: boolean } = {};
  selectedColorIncome: string = '';
  selectedColorExpense: string = '';
  customizedColors: string[] = [
    'afafaf', '787878', 'fca52d', 'ffa490', 'f98159',
    'fb6467', 'ff494d', '94cd7a', '86bb5d', 'ec61a1',
    'ff5692', 'e454ed', '82c8f1', '5096de', '5161b9',
    '8976bf', '7253c8', 'd9aa6a', '855f2a'
  ];

  constructor(
    private _categoriesRepository: CategoriesRepositoryService,
    private _notificationService: NotificationService) { }

  ngOnInit() {
    this.onGetAllCategories();
  }

  toggleCategory() {
    this.isExpenseCategory = !this.isExpenseCategory;
    this.isIncomeCategory = !this.isIncomeCategory;
    this.onGetAllCategories();
  }

  onGetAllCategories() {
    this._categoriesRepository.getAll().subscribe({
      next: (data: Category[]) => {
        this.incomeCategories = data.filter(category => category.type === CategoryType.Income);
        this.expenseCategories = data.filter(category => category.type === CategoryType.Expense);
      },
      error: (error) => {
        this._notificationService.showError('Error fetching categories:', error);
      }
    });
  }

  onAddIncome() {
    this._notificationService.showWarning(`TODO - Ainda precisa ser implementado!!!`);
  }

  onAddExpense() {
    this._notificationService.showWarning(`TODO - Ainda precisa ser implementado!!!`);
  }

  onRemove(_t27: Category) {
    this._notificationService.showWarning(`TODO - Ainda precisa ser implementado!!!`);
  }

  onEdit(_t27: Category) {
    this._notificationService.showWarning(`TODO - Ainda precisa ser implementado!!!`);
  }

  onColorSelectedIncome(color: string): void {
    this.selectedColorIncome = color;
  }
  onColorSelectedExpense(color: string): void {
    this.selectedColorExpense = color;
  }
}
