import { BoardGameCategoryModel } from './boardgamecategory';
import { RateModel } from './rate';

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
    rates: RateModel[]
    imagePath: string;
    isLikedByUser: boolean,
    amountOfLikes: number
}
