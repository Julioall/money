import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { AuthComponent } from './page/auth/auth.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoadingInterceptor } from './core/service/loading/loading.interceptor';
import { NgxSpinnerModule } from "ngx-spinner";
import { SettingsModule } from './page/settings/settings.module';
import { NavBarComponent } from './shared/components/nav-bar/nav-bar.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthComponent,
    NavBarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SettingsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgxSpinnerModule,
    ToastrModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: LoadingInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
