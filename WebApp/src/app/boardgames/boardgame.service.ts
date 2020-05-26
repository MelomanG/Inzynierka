import { Injectable } from "@angular/core";
import { CreateBoardGameModel, BoardGameModel } from '../shared/models/boardgame';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CreateRateModel, RateModel } from '../shared/models/rate';

@Injectable()
export class BoardGameService {
    constructor(private http: HttpClient) {}

    createBoardGame(boardGame: CreateBoardGameModel, image: File) {
        return this.http.post(`${environment.apiUrl}/BoardGame`, boardGame)
            .pipe(switchMap(resp => {
                const data = <BoardGameModel>resp;
                return this.uploadImage(data.id, image)
            }));
    }

    getBoardGames() {
        return this.http.get(`${environment.apiUrl}/BoardGame`);
    }
    
    getLikedBoardGames() {
        return this.http.get(`${environment.apiUrl}/BoardGame/like`)
    }

    getBoardGame(id: string) {
        return this.http.get(`${environment.apiUrl}/BoardGame/${id}`);
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