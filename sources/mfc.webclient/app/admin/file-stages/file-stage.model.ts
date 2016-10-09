import { FileStatus } from '../filestatuses/filestatus.service';

export class FileStage {
    constructor(public code: string, public caption: string, public status: FileStatus, public order: number) { }
}