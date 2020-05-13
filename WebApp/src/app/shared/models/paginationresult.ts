export interface PaginationResult<T>{
    page: number;
    pageCount: number;
    pageSize: number;
    results: T[];
    totalCount: number;
}