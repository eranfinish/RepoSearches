// services/bookmarks.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Repository } from '../models/repository';

@Injectable({
  providedIn: 'root'
})
export class BookmarksService {
  private apiUrl = 'https://localhost:7159/api/bookmarks';
  private bookmarksSubject = new BehaviorSubject<Repository[]>([]);
  bookmarks$ = this.bookmarksSubject.asObservable();

  constructor(private http: HttpClient) { }

  getBookmarks(): Observable<Repository[]> {
    return this.http.get<Repository[]>(this.apiUrl, { withCredentials: true })
      .pipe(
        tap(bookmarks => this.bookmarksSubject.next(bookmarks))
      );
  }

  addBookmark(repository: Repository): Observable<any> {
    return this.http.post(this.apiUrl, repository, { withCredentials: true })
      .pipe(
        tap(() => {
          const currentBookmarks = this.bookmarksSubject.value;
          this.bookmarksSubject.next([...currentBookmarks, repository ]);
        })
      );
  }

  removeBookmark(id: number, repositoryId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/bookmarks/${id}/${repositoryId}`,
      { withCredentials: true })
      .pipe(
        tap(() => {
          const currentBookmarks = this.bookmarksSubject.value;
          this.bookmarksSubject.next(
            currentBookmarks.filter(b => b.id !== id)
          );
        })
      );
  }

  isBookmarked(repositoryId: number): boolean {
    return this.bookmarksSubject.value.some(b => b.id === repositoryId);
  }
}
