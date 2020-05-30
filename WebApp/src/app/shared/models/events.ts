import { AddressModel, PubModel } from './pub';
import { UserModel } from './contacts';
import { BoardGameModel } from './boardgame';

export interface CreateEventModel {
    name: string,
    description: string,
    startDate: string,
    address: AddressModel,
    isPublic: boolean,
    pubId: string,
    boardGameId: string
}

export interface EventModel {
    id: string,
    name: string,
    description: string,
    startDate: string,
    address: AddressModel,
    imagePath: string,
    isPublic: boolean,
    owner: UserModel,
    pub: PubModel,
    boardGame: BoardGameModel,
    participants: UserModel[],
    isUserEvent: boolean,
    isUserParticipant: boolean
}