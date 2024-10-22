import { Component, OnInit, ViewChild } from '@angular/core';
import { Category } from '../../core/model/entities/category';
import { CategoriesRepositoryService } from '../../core/service/categories/categories-repository.service';
import { NotificationService } from '../../core/service/notification/notification.service';
import { CategoryType } from '../../core/model/enums/categoryType.enum';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IconPickerComponent } from '../../shared/icon-picker/icon-picker.component';
import { ColorPickerComponent } from '../../shared/color-picker/color-picker.component';
import { ModalComponent } from '../../shared/modal/modal.component';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  @ViewChild(ModalComponent) modal!: ModalComponent;
  @ViewChild(IconPickerComponent) iconPicker!: IconPickerComponent;
  @ViewChild(ColorPickerComponent) colorPicker!: ColorPickerComponent;

  isExpenseCategory: boolean = true;
  isIncomeCategory: boolean = false;
  isSubmitted: boolean = false;
  isEditing: boolean = false;
  incomeCategories: Category[] = [];
  expenseCategories: Category[] = [];
  categoryForm: FormGroup;
  selectedColorIncome: string = '';
  selectedColorExpense: string = '';
  selectedIconIncome: string = '';
  selectedIconExpense: string = '';
  idExpenseModal: string =  'expenseModal';
  idIncomeModal: string = 'incomeModal';
  colors: string[] = [
    '#ED1B25', '#A41A44', '#7A2990', '#392B8A', '#2F5AA8', '#018CCD', '#60AF46', '#CDDB26', '#F6ED24', '#F7BA12', '#F9971D', '#F14B21', "#F26A6A", "#C94B67", "#A158B5", "#5D4FB1", "#4B7EC2", "#42B8E1", "#88C876", "#E1ED6C", "#F8F17D", "#F8CD5C"
  ];

  icons: string[] = [
    // Food and Drink
    'fa fa-utensils',
    'fa fa-hamburger',
    'fa fa-pizza-slice',
    'fa fa-coffee',
    'fa fa-ice-cream',
    'fa fa-candy-cane',
    'fa fa-bread-slice',
    'fa fa-wine-glass',
    'fa fa-beer',
    'fa fa-fish',

    // Transportation
    'fa fa-bus',
    'fa fa-car',
    'fa fa-bicycle',
    'fa fa-motorcycle',
    'fa fa-plane',
    'fa fa-subway',
    'fa fa-taxi',
    'fa fa-ship',
    'fa fa-truck',
    'fa fa-bicycle',

    // Finance
    'fa fa-wallet',
    'fa fa-credit-card',
    'fa fa-dollar-sign',
    'fa fa-coins',
    'fa fa-piggy-bank',
    'fa fa-money-bill-wave',
    'fa fa-money-bill',
    'fa fa-cash-register',
    'fa fa-hand-holding-usd',
    'fa fa-balance-scale',

    // Business and Services
    'fa fa-briefcase',
    'fa fa-building',
    'fa fa-hotel',
    'fa fa-store',
    'fa fa-university',
    'fa fa-chart-line',
    'fa fa-chart-bar',
    'fa fa-chart-pie',
    'fa fa-file-invoice-dollar',
    'fa fa-receipt',

    // Technology
    'fa fa-laptop',
    'fa fa-desktop',
    'fa fa-tablet-alt',
    'fa fa-mobile-alt',
    'fa fa-wifi',
    'fa fa-cloud',
    'fa fa-plug',
    'fa fa-cogs',
    'fa fa-database',
    'fa fa-robot',

    // Health and Wellness
    'fa fa-stethoscope',
    'fa fa-user-md',
    'fa fa-heartbeat',
    'fa fa-first-aid',
    'fa fa-prescription-bottle',
    'fa fa-syringe',
    'fa fa-bath',
    'fa fa-user-nurse',
    'fa fa-dumbbell',
    'fa fa-bone',

    // Communication
    'fa fa-envelope',
    'fa fa-comments',
    'fa fa-comment-dots',
    'fa fa-phone',
    'fa fa-headphones',
    'fa fa-paper-plane',
    'fa fa-bullhorn',
    'fa fa-bell',
    'fa fa-sms',
    'fa fa-fax',

    // Nature and Animals
    'fa fa-tree',
    'fa fa-seedling',
    'fa fa-leaf',
    'fa fa-paw',
    'fa fa-dog',
    'fa fa-cat',
    'fa fa-fish',
    'fa fa-bone',
    'fa fa-spa',
    'fa fa-hat-wizard',

    // Household and Miscellaneous
    'fa fa-home',
    'fa fa-hammer',
    'fa fa-tools',
    'fa fa-chair',
    'fa fa-lightbulb',
    'fa fa-plug',
    'fa fa-umbrella',
    'fa fa-fire',
    'fa fa-crown',
    'fa fa-archive',

    // Social and User
    'fa fa-user',
    'fa fa-users',
    'fa fa-user-circle',
    'fa fa-user-tie',
    'fa fa-user-shield',
    'fa fa-user-cog',
    'fa fa-user-graduate',
    'fa fa-baby-carriage',
    'fa fa-heart',
    'fa fa-grin',

    // Events and Time
    'fa fa-calendar-alt',
    'fa fa-calendar-check',
    'fa fa-calendar-minus',
    'fa fa-clock',
    'fa fa-hourglass-half',
    'fa fa-sun',
    'fa fa-moon',
    'fa fa-bell',
    'fa fa-exclamation-triangle',
    'fa fa-hourglass-start',

    // Shapes and Symbols
    'fa fa-star',
    'fa fa-heart',
    'fa fa-circle',
    'fa fa-square',
    'fa fa-plus',
    'fa fa-minus',
    'fa fa-check',
    'fa fa-times',
    'fa fa-arrow-up',
    'fa fa-arrow-down',

    // Miscellaneous Icons
    'fa fa-gavel',
    'fa fa-key',
    'fa fa-lock',
    'fa fa-unlock',
    'fa fa-file-alt',
    'fa fa-file-signature',
    'fa fa-shield-alt',
    'fa fa-handshake',
    'fa fa-graduation-cap',
    'fa fa-puzzle-piece',

    // More Icons
    'fa fa-hat-cowboy',
    'fa fa-guitar',
    'fa fa-music',
    'fa fa-gem',
    'fa fa-diamond',
    'fa fa-basketball-ball',
    'fa fa-football-ball',
    'fa fa-hockey-puck',

    // More Food Icons
    'fa fa-carrot',
    'fa fa-pepper-hot',
    'fa fa-cheese',
    'fa fa-egg',
    'fa fa-apple-alt',
    'fa fa-lemon',


    // More Business and Finance
    'fa fa-file-contract',
    'fa fa-hand-holding-heart',
    'fa fa-comments-dollar',
    'fa fa-sign-out-alt',
    'fa fa-sync',
    'fa fa-tag',
    'fa fa-thumbs-up',
    'fa fa-thumbs-down',
    'fa fa-dollar-sign',
    'fa fa-gift',

    // More Nature and Miscellaneous
    'fa fa-glass-cheers',
    'fa fa-gas-pump',
    'fa fa-rainbow',
    'fa fa-snowflake',
    'fa fa-traffic-light',
    'fa fa-snowman',
    'fa fa-paperclip',
    'fa fa-file-invoice',
    'fa fa-clipboard-list',
    'fa fa-clipboard-check',

    // Additional Miscellaneous
    'fa fa-database',
    'fa fa-certificate',
    'fa fa-tasks',
    'fa fa-hands-helping',
    'fa fa-sack-dollar',
    'fa fa-hourglass-start',
    'fa fa-tasks',
    'fa fa-trophy',
    'fa fa-bullhorn',
    'fa fa-exchange-alt',

    // User Interactions
    'fa fa-hand-point-right',
    'fa fa-handshake-alt',
    'fa fa-hands-helping',
    'fa fa-thumbs-up',
    'fa fa-thumbs-down',
    'fa fa-fingerprint',
    'fa fa-eye',
    'fa fa-eye-slash',
    'fa fa-signal',
    'fa fa-headset',

    // Extra Tech Icons
    'fa fa-camera',
    'fa fa-television',
    'fa fa-video',
    'fa fa-fax',
    'fa fa-desktop',
    'fa fa-file-code',
    'fa fa-file-image',
    'fa fa-file-video',
    'fa fa-file-audio',
    'fa fa-globe',

    // Additional User-related
    'fa fa-certificate',
    'fa fa-user-tag',
    'fa fa-user-secret',
    'fa fa-user-lock',
    'fa fa-user-check',
    'fa fa-user-minus',
    'fa fa-user-plus',
    'fa fa-users-cog',
  ];

  constructor(
    private _fb: FormBuilder,
    private _categoriesRepository: CategoriesRepositoryService,
    private _notificationService: NotificationService,
  ) {
    this.categoryForm = this._fb.group({
      incomeCategoryName: ['', [Validators.required, Validators.minLength(3)]],
      expenseCategoryName: ['', [Validators.required, Validators.minLength(3)]],
    });
  }

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

  private closeModal() {
    const idModal = this.isIncomeCategory ? this.idIncomeModal : this.idExpenseModal;
    const modal = document.getElementById(idModal);

    if (modal != null) {
      const closeButton = modal.querySelector('.btn-close');

      if (closeButton != null) {
        (closeButton as HTMLElement).click();
      }
    }
  }

  onActionModal(){
    if (this.isEditing){
      // Usar uma ação generica e dentro dela tomar as decições de edicao e adição
    }

    
  }
  onAddIncome() {
    try {
      if (this.isValidCategoryData()) {
        this.addCategory();
      } else {
        this._notificationService.showError('Please ensure all fields are filled in correctly for the income category.');
      }
    } catch (error) {
      this._notificationService.showError(`An error occurred while closing the category modal: ${error}`);
    }
  }

  onAddExpense() {
    try {
      if (this.isValidCategoryData()) {
        this.addCategory();
      } else {
        this._notificationService.showError('Please ensure all fields are filled in correctly for the expense category.');
      }
    } catch (error) {
      this._notificationService.showError(`An error occurred while adding the expense category: ${error}`);
    }
  }

  private isValidCategoryData() {
    this.isSubmitted = true;
    const categoryName = this.isIncomeCategory
      ? this.categoryForm.get('incomeCategoryName')?.value
      : this.categoryForm.get('expenseCategoryName')?.value;

    const selectedIcon = this.isIncomeCategory
      ? this.selectedIconIncome
      : this.selectedIconExpense;

    const selectedColor = this.isIncomeCategory
      ? this.selectedColorIncome
      : this.selectedColorExpense;

    const isCategoryNameValid = categoryName && categoryName.length >= 3;
    const isIconSelected = !!selectedIcon;
    const isColorSelected = !!selectedColor;

    return isCategoryNameValid && isIconSelected && isColorSelected;
  }

  addCategory() {
    const categoryName = this.isIncomeCategory ? this.categoryForm.get('incomeCategoryName')?.value : this.categoryForm.get('expenseCategoryName')?.value;
    const selectedIcon = this.isIncomeCategory ? this.selectedIconIncome : this.selectedIconExpense;
    const selectedColor = this.isIncomeCategory ? this.selectedColorIncome : this.selectedColorExpense;
    const categoryType = this.isIncomeCategory ? CategoryType.Income : CategoryType.Expense;

    const newCategory: Category = {
      name: categoryName,
      type: categoryType,
      icon: selectedIcon,
      background: selectedColor,
    };

    this._categoriesRepository.create(newCategory).subscribe({
      next: (category: Category) => {
        if (this.isIncomeCategory) {
          this.incomeCategories.push(category);
        } else {
          this.expenseCategories.push(category);
        }
        this._notificationService.showSuccess(`${this.isIncomeCategory ? 'Income' : 'Expense'} category added successfully!`);
        this.closeModal()
        this.resetSelections();
      },
      error: (error) => {
        this._notificationService.showError(`Error adding ${this.isIncomeCategory ? 'income' : 'expense'} category:`, error);
      }
    });
  }

  resetSelections() {
    this.selectedColorIncome = '';
    this.selectedColorExpense = '';
    this.selectedIconIncome = '';
    this.selectedIconExpense = '';
    this.isSubmitted = false;
    this.categoryForm.reset();
    this.iconPicker.resetSelection();
    this.colorPicker.resetSelection();
  }

  onRemove(categoria: Category) {
    try{

    } catch (error) {
      this._notificationService.showError(`An error occurred while removing the ${this.isIncomeCategory ? 'income' : 'expense'} category: ${error}`);
    }
  }

  onEdit(categoria: Category) {
    try {
      if (!categoria || !categoria.background || !categoria.icon) {
        throw new Error('Invalid category data');
      }

      if (this.isIncomeCategory) {
        this.categoryForm.get('incomeCategoryName')?.setValue(categoria.name)
        this.selectedColorIncome = categoria.background;
        this.selectedIconIncome = categoria.icon;
      } else {
        this.categoryForm.get('expenseCategoryName')?.setValue(categoria.name)
        this.selectedColorExpense = categoria.background;
        this.selectedIconExpense = categoria.icon;
      }

      this.colorPicker.selectedColor = categoria.background;
      this.iconPicker.selectedIcon = categoria.icon;

    } catch (error) {
      this._notificationService.showError(
        `An error occurred while editing the ${this.isIncomeCategory ? 'income' : 'expense'} category: ${error}}`
      );
    }
  }
  
  onSelectIncomeColor(color: string) {
    this.selectedColorIncome = color;
  }

  onSelectExpenseColor(color: string) {
    this.selectedColorExpense = color;
  }

  onSelectExpenseIcon(icon: string) {
    this.selectedIconExpense = icon;
  }

  onSelectIncomeIcon(icon: string) {
    this.selectedIconIncome = icon;
  }
}
