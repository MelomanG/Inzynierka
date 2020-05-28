export abstract class PaginationQuery {
    page: number = 1;
    pageSize: number = 6;
    sort: string = "name";
    search: string = null;
}

export class PubQuery extends PaginationQuery {
    boardGameId?: string;
    city?;
}

export class BoardGameQuery extends PaginationQuery {
    category?: string;
    minPlayers?: number;
    maxPlayers?: number;
    minAge?: number;
    maxAge?: number;
}