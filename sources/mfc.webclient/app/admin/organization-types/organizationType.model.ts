export class OrganizationType {
    id:number;
    caption: string;

    constructor(
        id?:number,
        caption?: string
    ) {
        this.id = id;
        this.caption = caption;
    }
}