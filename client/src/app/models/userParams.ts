import { PaginationParams } from './pagination';
import { User } from './user';

export class UserParams extends PaginationParams {
  gender: string;
  minAge = 18;
  maxAge = 99;
  orderBy = 'lastActive';

  constructor(user: User) {
    super();
    this.gender = user.gender === 'female' ? 'male' : 'female';
  }
u
  key() {
    return Object.values(this).join('-');
  }
}
