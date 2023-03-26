import { Component, Output, EventEmitter } from '@angular/core';
import { timer } from 'rxjs';
import { TimerHandle } from 'rxjs/internal/scheduler/timerHandle';
import { User } from '../../models/user';
import { ConnectionService } from '../../services/connection.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  user: User = {
    name: '',
    gameId: '',
    admin: false
  }
  errorMessage: string|null = null;

  constructor(private _userService: UserService) {
  }

  onSend() {
    this.errorMessage = null;
    this._userService.login(this.user);
  }
}
