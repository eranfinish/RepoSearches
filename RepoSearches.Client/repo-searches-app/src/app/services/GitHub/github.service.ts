// File: src/app/services/github.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GitHubService {
  private apiUrl = `${environment.apiUrl}/github`;

  constructor(private http: HttpClient) {}

  searchRepositories(query: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/search?q=${query}`, {withCredentials: true});
  }
}
