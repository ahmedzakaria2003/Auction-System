export interface PaginatedResult<T> {
  data: T[];
  count: number;
  pageNumber: number;
  pageSize: number;
}
