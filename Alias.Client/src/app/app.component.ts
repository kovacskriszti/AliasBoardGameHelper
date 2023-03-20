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
  constructor(public _connectionService: ConnectionService) {
    this._connectionService.listen('SuccessConnection', () => {
      this._connectionService.connected=true;
    });
    this._connectionService.listen('SuccessConnectionAdmin', () => {
      this._connectionService.connected = true;
      this._connectionService.user.admin = true;
    });
  }
}
