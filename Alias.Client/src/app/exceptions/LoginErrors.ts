

export class AlreadyAuthenticatedError implements Error {
    stack?: string | undefined;
  cause?: unknown;

  constructor(public name: string, public message: string) { }
}
