
var StopObserver = function() {
    if (observer != undefined){
        observer.disconnect();
    }
}
StopObserver();


let lastName = ""
var observer;
function StartObserver(){
    observer = new MutationObserver(function(mutations) {

        let text = ""
        mutations.forEach(function(mutation) {
            let nodes = mutation.addedNodes;

            for (var i = 0; i < nodes.length; i++){
                var node = nodes[i];
                if (!(node instanceof HTMLElement)){
                    continue;
                }

                if (node.classList.contains("YTbUzc")){
                    console.log("Name: " + node.textContent)
                    lastName = node.textContent
                }
                if (node.classList.contains("oIy2qc")){
                    console.log("Text: " + node.textContent)
                    text = node.textContent
                }
            }
        });

        ReadChat(lastName, text)
    });

    let obj = {childList: true, subtree: true};
    let findObservingClass = document.querySelector(".z38b6");
    observer.observe(findObservingClass, obj);
}

function ReadChat(name, text) {
    if (text == "") {
        return
    }
    if (text == "/loadppl"){
        text = getList();
    }
    console.log("Sending: " + `${name}{:}${text}`)
    SendSocket(`${name}{:}${text}`);
}

StartObserver();




function getList(){

    
    var content = document.querySelector(".AE8xFb.OrqRRb.GvcuGe.goTdfd");

    divs = content.getElementsByClassName("cxdMu KV1GEc");

    text = "/loadppl ";

    for (i = 0; i < divs.length; i++){


        // presentation = divs[i].getElementsByClassName("jcGw9c");
        // if (presentation.length != 0){
        //     console.log("- (Skipped) - Presentation");
        //     continue;
        // }

        let person = divs[i].querySelector(".zWGUib").textContent;
        //console.log(person);

        text += person + ((i == divs.length - 1) ? "" : "\t");

    }

    return text;


}










if (typeof interval !== 'undefined'){
    clearInterval(interval);
}


var intervalCount = 1;
var content = document.getElementsByClassName("z38b6 CnDs7d hPqowe");
var xhr = new XMLHttpRequest();
send = {
    'values' : [[]]
};
var sectionAt = 0;
var chatAt = 0;

var intervalRunning = true;

//var interval = setInterval(execute, 100);

if (typeof connection == 'undefined'){
    var connected = false;
    var tryingToConnect = false;
    var ws = null;
}
function ConnectToServer(attempt = 0){
    ws = new WebSocket("ws://127.0.0.1:8080/Reader");
    connected = false;
    tryingToConnect = true;
    ws.onopen = function(){
        connected = true;
        tryingToConnect = false;
    };
    ws.onerror = function(){
    };
    ws.onclose = function(){
        setTimeout(function(){
            ConnectToServer(attempt + 1);
        }, 150);
        connected = false;
        tryingToConnect = true;
    };
}
if (!connected && !tryingToConnect){
    ConnectToServer(0);
}

function SendSocket(text){
    if (!connected){
        return;
    }
    //console.log("Sending: " + text);
    ws.send(text);
}

// function execute(){

//     send = {
//         'values' : [[]]
//     }

//     //console.log(intervalCount)
//     //console.log(sectionAt);
//     //console.log(chatAt);

//     if (content.length > 0){
//         var sections = content[0].getElementsByClassName("GDhqjd");
//         var jsonLeft = 0;
//         var jsonRight = 0;
    
//         var i = sectionAt;
//         for (; i < sections.length; i++){

//             var person = sections[i].getElementsByClassName("YTbUzc")[0].innerHTML;    
//             var chats = sections[i].getElementsByClassName("oIy2qc");
    
//             if (sectionAt != i){
//                 sectionAt = i;
//                 chatAt = 0;
//             }


//             var chatLoopRan = false;
//             required = true;
//             var j = chatAt;
//             for (; j < chats.length; j++){

//                 targetText = chats[j].textContent;  //Moved from below

//                 if (targetText == "/loadppl"){
//                     targetText = getList();
//                 }


//                 chatLoopRan = true;
//                 //If loop runs once, add person
//                 if (required){
//                     send.values[jsonLeft] = [];
//                     send.values[jsonLeft][0] = person;
//                     required = false;
//                 }

//                 /* if (targetText.includes("\n")){
//                     targetText = targetText.replaceAll("\n", "{\\n}");
//                 } */
//                 send.values[jsonLeft][jsonRight+1] = targetText;



//                 //Post run
//                 chatAt = j + 1;
//                 jsonRight++;
    
//             }
//             if (chatLoopRan){
//                 jsonLeft++;
//             }
//             jsonRight = 0;
//         }
//     }
    
//     if (send.values[0].length != 0){
//         for (let row = 0; row < send.values.length; row++){
//             for (let item = 1; item < send.values[row].length; item++){
//                 SendSocket(`${send.values[row][0]}{:}${send.values[row][item]}`);
//             }
//         }
//         /* var stringyfied = JSON.stringify(send);
//         xhr.open("POST", "http://localhost:8080/", true);
//         xhr.setRequestHeader('Content-Type', 'application/json');
//         xhr.send(stringyfied); */
//     }

//     intervalCount++;

// }