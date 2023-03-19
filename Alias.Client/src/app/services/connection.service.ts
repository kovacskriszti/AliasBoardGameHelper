import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr'
import { Observable } from 'rxjs';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
import { User } from '../models/user';
import { isDevMode } from '@angular/core'
import { GameStartOptions } from '../models/gameStartOptions';

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {
  private connection: signalR.HubConnection;
  user: User = { name: '', gameId: '', admin: false };

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.getHubUrl())
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Error)
      .build();
    this.listen('TransferAdmin', () => {
      this.user.admin = true;
    });
    this.connection.start();
  }

  async connect(user: User) {
    this.user = user;
    await this.connection.invoke('Connect', user);
  }

  getConnectedPlayers() {
    this.connection.invoke('GetConnectedPlayers', this.user);
  }

  startGame(options: GameStartOptions) {
    this.connection.invoke('StartGame', options);
  }

  listen(methodName: string, newMethod: (...args: any[]) => any) {
    this.connection.on(methodName, newMethod);
  }

  private getHubUrl(): string {
    return isDevMode() ? 'http://localhost:5183/game' : '/game';
  }
} // class end
