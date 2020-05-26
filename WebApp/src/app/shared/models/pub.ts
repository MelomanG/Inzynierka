import { RateModel } from './rate';

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
    //pubBoardGames: any[],
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