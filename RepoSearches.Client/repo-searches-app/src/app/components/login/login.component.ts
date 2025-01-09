import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router,private route: ActivatedRoute) {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });

  }
  returnUrl: string='';
  ngOnInit() {
    // Get return url from route parameters or default to '/'
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

    // If already logged in, redirect
    if (this.authService.isAuthenticated) {
      this.router.navigate(['/search']);
    }
  }
  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe(
response => {
  const responseBody = response as { token: string, body: any };
  localStorage.setItem('user', JSON.stringify(responseBody.body));
  localStorage.setItem('token', responseBody.token);
  this.router.navigate(['/search']);
  // Handle successful login, e.g., redirect to another page
},
        error => {
          this.errorMessage = 'Invalid username or password';
        }
      );
    }
  }
}
