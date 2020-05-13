import { Injectable } from "@angular/core";
import { CreateBoardGameModel } from '../shared/models/boardgame';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';

@Injectable()
export class BoardGameService {
    constructor(private http: HttpClient) {}

    createBoardGame(boardGame: CreateBoardGameModel) {
        this.http.post(`${environment.apiUrl}/BoardGame`, boardGame)
            .subscribe(res => {
                console.log(res)
        })
    }

    getBoardGames() {
        return this.http.get(`${environment.apiUrl}/BoardGame`);
    }

    getBoardGame(id: string) {
        return this.http.get(`${environment.apiUrl}/BoardGame/${id}`);
    }
}