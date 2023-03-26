import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { ConnectionService } from './connection.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private _connectionService: ConnectionService) { }

  login(user: User): boolean {
    return this._connectionService.connect;
  }
}
