import { User } from "./user";

export class Team {
  id: string|null;
  users: User[];

  constructor(users: User[]) {
    this.users = users;
    this.id = users[0].gameId + '/';
  }

  generateId() {
    let id= '';
    this.id = id;
  }
}
