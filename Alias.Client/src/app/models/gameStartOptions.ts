import { Team } from "./team";
import { UserTeam } from "./userTeam";

export class GameStartOptions {
constructor(public gameId: string , public random: boolean, public numberOfTeams: number, public teams: UserTeam[] | null = null) { }
}
