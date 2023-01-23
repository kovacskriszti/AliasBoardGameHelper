import { Component, HostListener } from '@angular/core';
import { User } from './models/user';
import { async, connect } from 'rxjs';
import { ConnectionService } from './services/connection.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  connected: boolean = false;
  started: boolean = false;

  constructor(public _connectionService: ConnectionService) {
    this._connectionService.listen('SuccessConnection', (user: User) => {
      this.connected = true;
      this._connectionService.user = user;
    });
  }
}
