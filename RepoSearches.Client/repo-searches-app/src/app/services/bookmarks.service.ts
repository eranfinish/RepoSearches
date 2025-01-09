// services/bookmarks.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Repository } from '../models/repository';
import { Bookmark } from '../models/bookmark';
@Injectable({
  providedIn: 'root'
})
export class BookmarksService {
  private apiUrl = 'https://localhost:7159/api/bookmarks';
  private bookmarksSubject = new BehaviorSubject<Bookmark[]>([]);
  bookmarks$ = this.bookmarksSubject.asObservable();

  constructor(private http: HttpClient) { }

  getBookmarks(): Observable<Bookmark[]> {

  const bookmarks: Bookmark[] = JSON.parse(localStorage.getItem('bookmarks') ?? '[]');
 if(bookmarks.length > 0){
  this.bookmarksSubject.next(bookmarks);
  return this.bookmarks$;
}
else{

     return this.http.get<Bookmark[]>(this.apiUrl, { withCredentials: true })
      .pipe(
        tap(bookmarks => this.bookmarksSubject.next(bookmarks))
      );
}
  }

  addBookmark(bookmark: Bookmark): Observable<any> {
    return this.http.post(this.apiUrl, bookmark, { withCredentials: true })
      .pipe(
        tap((response) => {
          localStorage.setItem('bookmarks', JSON.stringify(response));
          const currentBookmarks = this.bookmarksSubject.value;
          this.bookmarksSubject.next([...currentBookmarks, bookmark ]);
        })
      );
  }

  removeBookmark(userId: number, repositoryId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/bookmarks/${userId}/${repositoryId}`,
      { withCredentials: true })
      .pipe(
        tap((response) => {
          localStorage.setItem('bookmarks', JSON.stringify(response));
          const currentBookmarks = this.bookmarksSubject.value;
          this.bookmarksSubject.next(
            currentBookmarks.filter(b => b.userId !== userId)
          );
        })
      );
  }

  isBookmarked(repositoryId: number): boolean {
    return this.bookmarksSubject.value.some(b => b.repositoryId === repositoryId);
  }
}
