import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GitHubService } from 'src/app/services/GitHub/github.service';
import { Repository } from '../../models/repository';
import { AuthService } from 'src/app/services/auth.service';
@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent  {
  searchQuery: string = '';
repositories: Repository[] = [];
  constructor(private router: Router, private githubService:GitHubService, private authService:AuthService) {}
//ngOnInit(): void {
  //let isAuthenticated = (localStorage.getItem('isLoggedIn')=='true'?true:false);
//if(!isAuthenticated){
 // this.router.navigate(['/login']);
//}
//else{
//  console.log("SearchComponent",'User is authenticated');
//}
//}

  searchRepositories() {

     // Make API call to GitHub API
      this.githubService.searchRepositories(this.searchQuery).subscribe((response) => {
          this.repositories = response.items;
        console.log('API Response', response);
      });


  }
}
