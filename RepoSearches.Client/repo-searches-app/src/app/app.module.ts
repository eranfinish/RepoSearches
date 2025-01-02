import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './components/search/search.component';

// Angular Material modules
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { GitHubService } from './services/GitHub/github.service';
import { ReactiveFormsModule } from '@angular/forms';
import { BookmarksComponent } from './components/bookmarks/bookmarks.component';
import { GalleryComponent } from './components/gallery/gallery.component';
import { RepositoryComponent } from './components/repository/repository.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './services/auth.guard';
import { BookmarksService } from './services/bookmarks.service';
import { AuthService } from './services/auth.service';
// Define routes
const routes: Routes = [];

@NgModule({
  declarations: [
    AppComponent,SearchComponent, BookmarksComponent, GalleryComponent, RepositoryComponent, LoginComponent, RegisterComponent
  ],
  imports: [
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserModule,
    FormsModule,
    HttpClientModule,
    RouterModule.forRoot(routes),
    BrowserAnimationsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatListModule,
    MatIconModule,
    MatCardModule,
  ],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
     AuthInterceptor, AuthGuard, GitHubService,AuthService, BookmarksService,
     { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'outline' } }],
  bootstrap: [AppComponent]
})
export class AppModule { }
