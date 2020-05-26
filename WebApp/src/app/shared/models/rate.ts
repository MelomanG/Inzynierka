export interface CreateRateModel {
    userRate: number,
    comment: string
}

export interface RateModel {
    id: string,
    userRate: number,
    comment: string,
    userName: string
}