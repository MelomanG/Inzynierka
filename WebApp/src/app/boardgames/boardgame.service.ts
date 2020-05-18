import { Injectable } from "@angular/core";
import { CreateBoardGameModel, BoardGameModel } from '../shared/models/boardgame';
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

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

    getBoardGame(id: string) {
        return this.http.get(`${environment.apiUrl}/BoardGame/${id}`);
    }

    updateBoardGame(boardGame: BoardGameModel, image: File) {
        console.log(boardGame);
        return this.http.put(`${environment.apiUrl}/BoardGame/${boardGame.id}`, boardGame)
            .pipe(switchMap(resp => {
                const data = <BoardGameModel>resp;
                if(image) 
                    return this.uploadImage(data.id, image)
                else 
                    return new Observable(observer => observer.next(data));
            }));
    }

    private uploadImage(id: string, image: File) {
        let formData = new FormData();
        formData.append('image', image, image.name);
        return this.http.post(`${environment.apiUrl}/BoardGame/${id}/image`, formData);
    }
}