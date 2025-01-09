import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { BookmarksService } from '../../services/bookmarks.service';
import { Observable } from 'rxjs';
import { Repository } from 'src/app/models/repository';
import { Bookmark } from 'src/app/models/bookmark';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-bookmarks',
  templateUrl: './bookmarks.component.html',
  styleUrls: ['./bookmarks.component.scss']
})
export class BookmarksComponent implements OnInit {
  constructor(private authService: AuthService,
    private router: Router,
    private bookmarksService: BookmarksService) { }
  loading = false;
  error: string | null = null;
  bookmarks$: Observable<any> | undefined; // Declare as class property
bookmarks: Bookmark[] = [];

  ngOnInit() {
    this.bookmarks$ = this.bookmarksService.bookmarks$;
    this.loadBookmarks();

  }

  loadBookmarks() {
    this.loading = true;
    this.error = null;

    this.bookmarksService.getBookmarks().subscribe({
      next: (response) => {
       this.bookmarks = response;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load bookmarks';
        console.error('Error loading bookmarks:', err);
      }
    });
  }

  removeBookmark(id: number, repositoryId: number) {
    userId: Number;
    let user  = JSON.parse(localStorage.getItem('user') ?? '');
    this.bookmarksService.removeBookmark(user.id, repositoryId).subscribe({
      next: () => {
        // Bookmark removed successfully
      },
      error: (err) => {
        this.error = 'Failed to remove bookmark';
        console.error('Error removing bookmark:', err);
      }
    });
  }
}
