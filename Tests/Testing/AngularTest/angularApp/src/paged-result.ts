export class PagedResult<T> {
    constructor(totalCount: number, pageCount: number, pageSize: number, pageNumber: number, data: Array<T>) {
        this.totalCount = totalCount;
        this.pageCount = pageCount;
        this.pageSize = pageSize;
        this.pageNumber = pageNumber;
        this.data = data;
    }

    totalCount: number;
    pageCount: number;
    pageSize: number;
    pageNumber: number;
    data: Array<T>;
}