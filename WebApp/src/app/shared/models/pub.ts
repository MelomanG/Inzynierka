import { RateModel } from './rate';
import { BoardGameModel } from './boardgame';

export interface CreatePubModel {
    name: string,
    description: string,
    address: AddressModel
}

export interface PubModel {
    id: string,
    name: string,
    description: string,
    address: AddressModel,
    imagePath: string,
    rates: RateModel[],
    pubBoardGames: BoardGameModel[],
    isLikedByUser: boolean,
    isUserPub: boolean;
    amountOfLikes: number
}

export interface AddressModel {
    street: string,
    buildingNumber: string,
    localNumber: string,
    postalCode: string,
    city: string
}