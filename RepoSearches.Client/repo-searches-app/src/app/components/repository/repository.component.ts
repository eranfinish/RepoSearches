import { Component, Input ,forwardRef } from '@angular/core';
import { Repository } from 'src/app/models/repository';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { BookmarksService } from '../../services/bookmarks.service';
import { User } from '../../models/user';
import { Bookmark } from 'src/app/models/bookmark';

@Component({
  selector: 'app-repository',
  templateUrl: './repository.component.html',
  styleUrls: ['./repository.component.scss'],
    providers: [
      {
        provide: NG_VALUE_ACCESSOR,
        useExisting: forwardRef(() => RepositoryComponent),
        multi: true
      }
    ]
})
export class RepositoryComponent {
  @Input() repository!: Repository;
  userId:number =0
bookmark: Bookmark | undefined;

constructor(private bookmarksService: BookmarksService) {
let userString = localStorage.getItem('user') ?? '';
if(userString != ''){
   let user: User = JSON.parse(userString);
  this.userId = user.id;
}
 }

toggleBookmark(): void {

    this.repository.bookmarked = this.repository.bookmarked == null? true: !this.repository.bookmarked;
    this.repository.language = this.repository.language == null? "": this.repository.language;
this.bookmark={
  createdAt: new Date(),
  repositoryId: this.repository.id,
  repository: this.repository,
  userId: this.userId,

  isBookmarked: this.repository.bookmarked
}
    if (this.repository.bookmarked){
    this.bookmarksService.addBookmark(this.bookmark).subscribe({
  next: () => {
    console.log('Bookmark added');
  },
  error: (err) => {
    this.repository.bookmarked = true;
    console.error('Error adding bookmark:', err);
  }
    });
  }

  else{
    this.bookmarksService.removeBookmark(this.userId, this.repository.id).subscribe({
  next: () => {
    console.log('Bookmark removed');

  },
  error: (err) => {
    this.repository.bookmarked = false;
    console.error('Error removing bookmark:', err);
  }
    })
  }
  }
}
