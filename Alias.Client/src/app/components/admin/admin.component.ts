import { Component } from '@angular/core';
import { GameStartOptions } from '../../models/gameStartOptions';
import { Team } from '../../models/team';
import { UserTeam } from '../../models/userTeam';
import { ConnectionService } from '../../services/connection.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  connectedPlayers: string[] = [];
  manualConfiguration = false;
  numberOfTeams = 2;
  teams: UserTeam[] = [];

  constructor(private _connectionService: ConnectionService) {
    this._connectionService.listen('SendConnectedPlayers', (players: string[]) => {
      this.connectedPlayers = players;
    });
    this._connectionService.listen('Connect', (name: string) => {
      this.connectedPlayers.push(name);
    });
    this._connectionService.listen('Disconnect', (name: string) => {
      this.connectedPlayers.splice(this.connectedPlayers.indexOf(name), 1);
      this.configureManualy();
    });
    this.getConnectedPlayers();
  }

  getConnectedPlayers() {
    this._connectionService.getConnectedPlayers();
  }

  configureManualy() {
    this.manualConfiguration = true;
    this.teams = [];
    for (let user of this.connectedPlayers) {
      this.teams.push(new UserTeam(user, 1, this.numberOfTeams));
    }
  }

  configureRandomly() {
    this.manualConfiguration = false;
    this.startGame();
  }

  startGame() {
    let gameStartOptions: GameStartOptions = new GameStartOptions(this._connectionService.user.gameId, true);
    if (this.manualConfiguration == true) {
      gameStartOptions.random = false;
      gameStartOptions.teams = this.teams;
    }
    this._connectionService.startGame(gameStartOptions);
  }
} // class end
