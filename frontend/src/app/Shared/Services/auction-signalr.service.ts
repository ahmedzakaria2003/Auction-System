import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class AuctionSignalRService {
  private hubConnection: signalR.HubConnection | null = null;

public async startConnection(auctionId?: string) {
  if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
    console.log('Already connected');
    if (auctionId) this.joinAuctionGroup(auctionId);
    return;
  }

  this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:7108/auctionhub')
    .withAutomaticReconnect()
    .build();

  try {
    await this.hubConnection.start();
    console.log('SignalR Connected!');
    if (auctionId) this.joinAuctionGroup(auctionId);
  } catch (err) {
    console.error('Error while starting connection: ', err);
  }
}

  // join group
  public joinAuctionGroup(auctionId: string) {
    if (!this.hubConnection) return;
    this.hubConnection.invoke('JoinAuctionGroup', auctionId)
      .catch(err => console.error('Join group error: ', err));
  }

  // leave group
  public leaveAuctionGroup(auctionId: string) {
    if (!this.hubConnection || this.hubConnection.state !== signalR.HubConnectionState.Connected) {
      console.warn('Hub is not connected. Skip leave group.');
      return;
    }
    this.hubConnection.invoke('LeaveAuctionGroup', auctionId)
      .catch(err => console.error('Leave group error: ', err));
  }

  // stop connection (optional cleanup)
  public stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop()
        .then(() => console.log('SignalR Disconnected!'))
        .catch(err => console.error('Error while stopping connection: ', err));
    }
  }

  public onNewBid(callback: (amount: number, bidder: string) => void) {
    this.hubConnection?.on('ReceiveBidNotification', (amount, bidder) => {
      callback(amount, bidder);
    });
  }

  public onAuctionCreated(callback: (id: string, title: string) => void) {
    this.hubConnection?.on('AuctionCreated', (id, title) => {
      callback(id, title);
    });
  }

  public onAuctionUpdated(callback: (id: string, title: string) => void) {
    this.hubConnection?.on('AuctionUpdated', (id, title) => {
      callback(id, title);
    });
  }

  public onAuctionDeleted(callback: (id: string, title: string) => void) {
    this.hubConnection?.on('AuctionDeleted', (id, title) => {
      callback(id, title);
    });
  }

  public onAuctionCanceled(callback: (id: string, title: string) => void) {
    this.hubConnection?.on('AuctionCanceled', (id, title) => {
      callback(id, title);
    });
  }

  public onWinnerDeclared(callback: (winnerName: string, amount: number) => void) {
    this.hubConnection?.on('AuctionWinnerDeclared', (winnerName, amount) => {
      callback(winnerName, amount);
    });
  }
}
