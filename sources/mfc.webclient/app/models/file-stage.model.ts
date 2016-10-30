import { FileStatus } from './file-status.model';

export class FileStage {
    constructor(public code: string, public caption: string, public status: FileStatus, public order: number) { }
}