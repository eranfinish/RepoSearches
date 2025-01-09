import { Repository } from "./repository";
import { Owner } from "./owner";
import { User } from "./user";

export interface Bookmark{
  isBookmarked: boolean;
  createdAt: Date;
  repositoryId: number;
  repository:Repository;
  //user: User;
  userId: number;
}
