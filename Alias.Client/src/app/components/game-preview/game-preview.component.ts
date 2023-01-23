import { Component } from '@angular/core';
import { ConnectionService } from '../../services/connection.service';

@Component({
  selector: 'app-game-preview',
  templateUrl: './game-preview.component.html',
  styleUrls: ['./game-preview.component.css']
})
export class GamePreviewComponent {
  messages: string[] = [];

  constructor(private connectionService: ConnectionService) {
    connectionService.listen('Connect', (name: string) => {
      this.messages.push(name + ' s-a conectat');
    });
    connectionService.listen('Disconnect', (name: string) => {
      this.messages.push(name + ' s-a deconectat');
    });
  }
}
