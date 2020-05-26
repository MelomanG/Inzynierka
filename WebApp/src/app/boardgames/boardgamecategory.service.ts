import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { CreateBoardGameCategoryModel, BoardGameCategoryModel } from '../shared/models/boardgamecategory';

@Injectable()
export class BoardGameCategoryService {
    
    constructor(private http: HttpClient) {}

    getBoardGameCategories() {
        return this.http.get(`${environment.apiUrl}/BoardGameCategory`);
    }

    createCategory(category: CreateBoardGameCategoryModel) {
        return this.http.post(`${environment.apiUrl}/BoardGameCategory`, category);
    }

    updateCategory(category: BoardGameCategoryModel) {
        return this.http.put(`${environment.apiUrl}/BoardGameCategory/${category.id}`, category);
    }

    deleteCategory(id: BoardGameCategoryModel) {
        return this.http.delete(`${environment.apiUrl}/BoardGameCategory/${id}`);
    }
}