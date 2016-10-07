export class User {
    id: number;
    user_name: string;
    description: string;
    is_admin: boolean;
    is_expert: boolean;
    is_controller: boolean;

    constructor(
        id?: number,
        user_name?: string,
        description?: string,
        is_admin?: boolean,
        is_expert?: boolean,
        is_controller?: boolean
    ) {
        this.id = id;
        this.user_name = user_name;
        this.description = description;
        this.is_admin = false;
        this.is_expert = false;
        this.is_controller = false;
    }
}