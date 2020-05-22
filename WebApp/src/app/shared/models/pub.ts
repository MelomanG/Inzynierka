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
    //pubRates: any[],
    //pubBoardGames: any[],
    isLikedByUser: boolean,
    amountOfLikes: number
}

export interface AddressModel {
    street: string,
    buildingNumber: string,
    localNumber: string,
    postalCode: string,
    city: string
}