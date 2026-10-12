import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed, TestModuleMetadata } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { ToastrModule } from 'ngx-toastr';

export function configureCommonTestBed(metadata: TestModuleMetadata = {}): Promise<void> {
  return TestBed.configureTestingModule({
    ...metadata,
    imports: [
      ...(metadata.imports ?? []),
      TranslateModule.forRoot(),
      ToastrModule.forRoot(),
    ],
    providers: [
      ...(metadata.providers ?? []),
      provideHttpClient(),
      provideHttpClientTesting(),
      provideRouter([]),
    ],
  }).compileComponents();
}
