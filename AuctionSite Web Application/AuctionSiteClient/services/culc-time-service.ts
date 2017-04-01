 class CulTimeService
 {
     public static $inject: string[] = ['$interval'];

     constructor(private $interval: ng.IIntervalService) {
         
     }
     public start(auction: Auction, callBack: Function ){
         this.$interval(() => {
             let time = this.culcTimeToEnd(auction);
             callBack(time);
         }, 60000);
     }

     public culcTimeToEnd(auction: Auction):string {

         let timeLeft = this.timeLeftCount(auction);
         if (timeLeft.daysLeft === 0) {
             return  +timeLeft.hoursLeft + 'h ' + timeLeft.minutsLeft + 'm';

         } else {
             return +timeLeft.daysLeft + 'd ' + timeLeft.hoursLeft + 'h';
         }

     }

     public timeLeftCount(auction: Auction): TimeLeft {
         var timeLeft = new TimeLeft();
         var d = Date.parse(auction.endTime.toString()) - +new Date();
         var minutes = 1000 * 60;
         var hours = minutes * 60;
         var days = hours * 24;

         timeLeft.daysLeft = Math.floor(d / days);
         var hoursInMillLeft = d - (timeLeft.daysLeft * days);
         timeLeft.hoursLeft = Math.floor(hoursInMillLeft / hours);
         var minuteInMillLeft = hoursInMillLeft - (timeLeft.hoursLeft * hours);
         timeLeft.minutsLeft = Math.floor(minuteInMillLeft / minutes);
         return timeLeft;
     }
}

app.service('culTimeService', CulTimeService)