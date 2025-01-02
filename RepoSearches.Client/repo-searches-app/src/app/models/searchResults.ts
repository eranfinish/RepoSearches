import { Repository } from './repository';
export interface SearchResults{
  totalCount: number;
  incompleteResults: boolean;
  items: Repository[];

}
