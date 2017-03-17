class Auction {
    public id: string = "";
    public title: string = "";
    public description: string = "";
    public startTime: Date = undefined;
    public endTime: Date = undefined;
    public startBid: number = undefined;
    public isItemNew: boolean = true;
    public picture1: string = "";
    public picture2: string = "";
    public picture3: string = "";
    public picture4: string = "";
    public bidCount: number = 0;
    public user: User = undefined;
    public category: Category = undefined;
    public highestBid: HighestBid = undefined; //  higest bid id
}