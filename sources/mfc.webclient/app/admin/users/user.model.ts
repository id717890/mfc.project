import {BaseModel} from './../../infrastructure/base.component/base-model';

export class User extends BaseModel {
    constructor(
        public id: number,
        public caption: string,
        public user_name: string,
        public description: string,
        public is_admin: boolean,
        public is_expert: boolean,
        public is_controller: boolean
    ) {
        super(id, caption);
    }
}