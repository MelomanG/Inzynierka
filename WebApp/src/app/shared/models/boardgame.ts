import { BoardGameCategoryModel } from './boardgamecategory';

export interface CreateBoardGameModel {
    name: string;
    description: string;
    minPlayers: number;
    maxPlayers: number;
    fromAge: number;
    categoryId: string;
}

export interface BoardGameModel {
    id: string;
    name: string;
    description: string;
    minPlayers: number;
    maxPlayers: number;
    fromAge: number;
    categoryId: string;
    category: BoardGameCategoryModel;
}