import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';

@Injectable()
export class BoardGameCategoryService {
    
    constructor(private http: HttpClient) {}

    getBoardGameCategories() {
        return this.http.get(`${environment.apiUrl}/BoardGameCategory`);
    }
}