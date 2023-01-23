import { Team } from "./team";
import { UserTeam } from "./userTeam";

export class GameStartOptions {
  constructor(public id: string, public random: boolean, public teams: UserTeam[] | null=null) { }
}
