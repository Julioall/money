import { Component, OnInit } from '@angular/core';
import { Category } from '../../../core/model/entities/category';
import { CategoriesRepositoryService } from '../../../core/repository/categories/categories-repository.service';
import { CategoryType } from '../../../core/model/enums/categoryType.enum';
import { NotificationService } from '../../../core/service/notification/notification.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  isExpenseCategory: boolean = true;
  isIncomeCategory: boolean = false;
  incomeCategories: Category[] = [];
  expenseCategories: Category[] = []; // Lista para despesas
  forms: { [key: string]: boolean } = {}; // Para gerenciar o estado dos formulários de subcategorias

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
}
