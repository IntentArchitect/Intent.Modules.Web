export interface FileDownloadDto {
  content: Stream;
  filename?: string;
  contentType?: string;
}
