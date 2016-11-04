import { BaseModel } from './base.model';
import { FileStatus } from './file-status.model';

export class FileStage extends BaseModel {
    constructor(id: number, public code: string, public caption: string, public status: FileStatus, public order: number) {
        super(id, caption);
    }
}