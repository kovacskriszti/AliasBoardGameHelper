import { Component, Output, EventEmitter } from '@angular/core';
import { timer } from 'rxjs';
import { TimerHandle } from 'rxjs/internal/scheduler/timerHandle';
import { User } from '../../models/user';
import { ConnectionService } from '../../services/connection.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  name = '';
  gameId = '';
  errorMessage: string|null = null;

  constructor(private connectionService: ConnectionService) {
    this.connectionService.listen('UserTaken', () => {
      this.errorMessage = 'Numele este deja luat.';
    });
  }

  onSend() {
    this.errorMessage = null;
    let user: User = {
      name: this.name,
      gameId: this.gameId,
      admin: false
    };
    this.connectionService.connect(user);
  }
}
