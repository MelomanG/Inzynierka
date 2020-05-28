import { Injectable } from "@angular/core";
import { CreateBoardGameModel, BoardGameModel } from '../shared/models/boardgame';
import { HttpClient, HttpParams } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CreateRateModel, RateModel } from '../shared/models/rate';
import { BoardGameQuery } from '../shared/models/query';

@Injectable()
export class BoardGameService {
    boardGameQuery = new BoardGameQuery();

    constructor(private http: HttpClient) {}

    createBoardGame(boardGame: CreateBoardGameModel, image: File) {
        return this.http.post(`${environment.apiUrl}/BoardGame`, boardGame)
            .pipe(switchMap(resp => {
                const data = <BoardGameModel>resp;
                return this.uploadImage(data.id, image)
            }));
    }

    getBoardGames() {
        let params = new HttpParams();

        if(this.boardGameQuery.category)
            params =params.append("category", this.boardGameQuery.category.toString())
        if(this.boardGameQuery.minPlayers)
            params =params.append("min_players", this.boardGameQuery.minPlayers.toString())
        if(this.boardGameQuery.maxPlayers)
            params =params.append("max_players", this.boardGameQuery.maxPlayers.toString())
        if(this.boardGameQuery.minAge)
            params =params.append("min_age", this.boardGameQuery.minAge.toString())
        if(this.boardGameQuery.maxAge)
            params =params.append("max_age", this.boardGameQuery.maxAge.toString())
        if(this.boardGameQuery.search)
            params =params.append("search", this.boardGameQuery.search.toString())
    
        params = params.append("sort", this.boardGameQuery.sort.toString())
        params = params.append("page", this.boardGameQuery.page.toString())
        params = params.append("page_size", this.boardGameQuery.pageSize.toString())

        return this.http.get(`${environment.apiUrl}/BoardGame`, { params: params });
    }

    setBoardGameQuery(query: BoardGameQuery) {
      this.boardGameQuery = query;
    }
  
    getBoardGameQuery() {
      return this.boardGameQuery;
    }
    
    getLikedBoardGames() {
        return this.http.get(`${environment.apiUrl}/BoardGame/like`)
    }

    getBoardGame(id: string) {
        return this.http.get(`${environment.apiUrl}/BoardGame/${id}`);
    }

    getPubsWithBoardGame(id: string) {
        return this.http.get(`${environment.apiUrl}/BoardGame/${id}/pubs`);
    }

    updateBoardGame(boardGame: BoardGameModel, image: File) {
        return this.http.put(`${environment.apiUrl}/BoardGame/${boardGame.id}`, boardGame)
            .pipe(switchMap(resp => {
                const data = <BoardGameModel>resp;
                if(image) 
                    return this.uploadImage(data.id, image)
                else 
                    return new Observable(observer => observer.next(data));
            }));
    }

    deleteBoardGame(id: string) {
        return this.http.delete(`${environment.apiUrl}/BoardGame/${id}`);
    }

    likeBoardGame(id: string) {
        return this.http.post(`${environment.apiUrl}/BoardGame/${id}/like`, {})
    }

    unLikeBoardGame(id: string) {
        return this.http.delete(`${environment.apiUrl}/BoardGame/${id}/unlike`)
    }

    rateBoardGame(id: string, rate: CreateRateModel) {
        return this.http.post(`${environment.apiUrl}/BoardGame/${id}/rate`, rate)
    }

    getUserBoardGameRate(id: string) {
        return this.http.get(`${environment.apiUrl}/BoardGame/${id}/rate`)
    }

    updateBoardGameRate(id: string, rate: RateModel) {
        return this.http.put(`${environment.apiUrl}/BoardGame/${id}/rate/${rate.id}`, rate)
    }

    private uploadImage(id: string, image: File) {
        let formData = new FormData();
        formData.append('image', image, image.name);
        return this.http.post(`${environment.apiUrl}/BoardGame/${id}/image`, formData);
    }
}