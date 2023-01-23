export class UserTeam {
  constructor(public User: string, public Team: number, public MaxTeam: number) { }

  teamUp() {
    this.Team = (this.Team < this.MaxTeam) ? this.Team + 1 : 1;
  }

  teamDown() {
    this.Team = (this.Team > 1) ? this.Team - 1 : this.MaxTeam;
  }
}
