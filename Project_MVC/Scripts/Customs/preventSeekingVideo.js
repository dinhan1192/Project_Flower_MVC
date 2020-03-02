var timeTracking = {
    watchedTime: 0,
    currentTime: 0
};
var lastUpdated = 'currentTime';

video.addEventListener('timeupdate', function () {
    if (!video.seeking) {
        if (video.currentTime > timeTracking.watchedTime) {
            timeTracking.watchedTime = video.currentTime;
            lastUpdated = 'watchedTime';
        }
        //tracking time updated  after user rewinds
        else {
            timeTracking.currentTime = video.currentTime;
            lastUpdated = 'currentTime';
        }
    }


});
// prevent user from seeking
video.addEventListener('seeking', function () {
    // guard against infinite recursion:
    // user seeks, seeking is fired, currentTime is modified, seeking is fired, current time is modified, ....
    var delta = video.currentTime - timeTracking.watchedTime;
    if (delta > 0) {
        video.pause();
        //play back from where the user started seeking after rewind or without rewind
        video.currentTime = timeTracking[lastUpdated];
        video.play();
    } 
});