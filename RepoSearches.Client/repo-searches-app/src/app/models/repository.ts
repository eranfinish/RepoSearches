
import { Owner } from "./owner";

export interface Repository{
  id: number;
  name: string;
  fullName: string;
  owner: Owner;
  htmlUrl: string;
  description: string;
  language: string;
  stargazersCount: number;
  forksCount: number;
  openIssuesCount: number;
  topics: string[];
  bookmarked: boolean;
}
