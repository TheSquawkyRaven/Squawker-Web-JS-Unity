document.addEventListener('DOMContentLoaded', function() {
    var startButton = document.getElementById('startButton');
    startButton.addEventListener('click', function() {
        chrome.tabs.executeScript(null,{
            file: 'readchat.js'
        },receiveText);
        document.getElementById("status").src = "activeStatus.png";
        document.getElementById("statusText").innerHTML = "ACTIVE";
    }, false);

    var stopButton = document.getElementById("stopButton");
    stopButton.addEventListener('click', function(){
        chrome.tabs.executeScript(null,{
            code: `
            if (typeof intervalRunning !== 'undefined') {
                clearInterval(interval);
            }
            intervalRunning = false;`
        },receiveText);
        document.getElementById("status").src = "inactiveStatus.png";
        document.getElementById("statusText").innerHTML = "INACTIVE";
    }, false);



    var getListButton = document.getElementById("getList");
    getListButton.addEventListener('click', function(){
        chrome.tabs.executeScript(null,{
            file: 'readparticipants.js'
        },receiveText);
    }, false)






    
}, false);
chrome.tabs.executeScript(null, {
    code: `
    ret = [];
    ret.push(false);
    if (typeof intervalRunning !== 'undefined') {
        ret[0] = intervalRunning;
    }
    ret;`
}, function(result){
    if (result[0][0]){
        document.getElementById("status").src = "activeStatus.png";
        document.getElementById("statusText").innerHTML = "ACTIVE";
    }
    else{
        document.getElementById("status").src = "inactiveStatus.png";
        document.getElementById("statusText").innerHTML = "INACTIVE";
    }
})

function receiveText(results){

}

