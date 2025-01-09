import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit{
  constructor(private router: Router, private route: ActivatedRoute, private authService:AuthService) {}
  selected:string = '';
  title = 'repo-searches-app';
  isLoggedIn  = false;



  ngOnInit(): void {
    //this.isLoggedIn.subscribe() = this.authService.isAuthenticated
    this.getCurrentRoutePath();
    this.authService.isAuthenticated$.subscribe(auth =>{
      this.isLoggedIn = auth;
    });

  }

  getCurrentRoutePath(): void {
    // Using the Router service
    const currentPath = this.router.url;
    console.log('Current Route Path (Router):', currentPath);
    let selections = environment.navigator;
    // Accessing ActivatedRoute snapshot
    const activatedPath = this.route.snapshot.url.map(segment => segment.path).join('/');
    console.log('Current Route Path (ActivatedRoute):', selections[activatedPath as keyof typeof selections]);
  }
  logout(){
    this.authService.logout();
    localStorage.removeItem('user');
    this.authService.setLoggedIn(false);

    this.router.navigate(['/login']);
  }
login(){
  this.router.navigate(['/login']);
}



}
