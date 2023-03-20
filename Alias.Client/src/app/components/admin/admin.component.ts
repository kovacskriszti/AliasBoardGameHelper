import { Component } from '@angular/core';
import { range } from 'rxjs';
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
  errorMessage: string ='';

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
    this._connectionService.listen('GameStartError', (error: string) => {
      this.displayGameStartError(error);
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
    let gameStartOptions: GameStartOptions = new GameStartOptions(this._connectionService.user.gameId, true, this.numberOfTeams);
    if (this.manualConfiguration == true) {
      if (!this.validateTeams()) {
        window.confirm('Trebuie să fie cel puțin doi membri într-o echipă!');
        return;
      }
      gameStartOptions.random= false;
      gameStartOptions.teams = this.teams;
    }
    this._connectionService.startGame(gameStartOptions);
  }

  private validateTeams(): boolean {
    let membersInEachTeam: number[] = Array<number>(this.numberOfTeams + 1).fill(0);
    for (let userTeam of this.teams) {
      membersInEachTeam[userTeam.Team]++;
    }
    for (let i = 1; i < membersInEachTeam.length; i++) {
      if (membersInEachTeam[i]<2) {
        return false;
      }
    }
    return true;
  }

  displayGameStartError(error: string) {

  }
} // class end
