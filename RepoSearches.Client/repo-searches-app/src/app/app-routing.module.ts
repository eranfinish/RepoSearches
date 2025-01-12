import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchComponent } from './components/search/search.component';
import { BookmarksComponent } from './components/bookmarks/bookmarks.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { AuthGuard } from './services/auth.guard';
const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  //{ path: 'search', component: SearchComponent },
  //{ path: 'bookmarks', component: BookmarksComponent },
 { path: 'search', component: SearchComponent, canActivate: [AuthGuard] },
  { path: 'bookmarks', component: BookmarksComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route goes to login
  { path: '**', redirectTo: '/login' } // Wildcard route also goes to login
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
  })

export class AppRoutingModule {}
