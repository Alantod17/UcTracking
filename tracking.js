//code for logging console, this need to be loaded at the very begining of the code
const overrideConsole = (function(originConsole){
    return {
        log: function(){
            originConsole.log(...arguments);
            createEventTrackingData("log", arguments);
        },
        info: function () {
            originConsole.info(...arguments);
            createEventTrackingData("info", arguments);
        },
        warn: function () {
            originConsole.warn(...arguments);
            createEventTrackingData("warn", arguments);
        },
        error: function () {
            originConsole.error(...arguments);
            createEventTrackingData("error", arguments);
        },
        debug: function () {
            originConsole.debug(...arguments);
            // createEventTrackingData("debug", arguments);
        },
        debugNoLog: function () {
            originConsole.debug(...arguments);
        }
    };
}(window.console));
window.console = overrideConsole;
window.onerror = function(message, source, lineno, colno, error) { 
    // console.error(message, source, lineno, colno, error);
};

history.pushState = ( f => function pushState(){
    var ret = f.apply(this, arguments);
    window.dispatchEvent(new Event('pushstate'));
    window.dispatchEvent(new Event('locationchange'));
    return ret;
})(history.pushState);

history.replaceState = ( f => function replaceState(){
    var ret = f.apply(this, arguments);
    window.dispatchEvent(new Event('replacestate'));
    window.dispatchEvent(new Event('locationchange'));
    return ret;
})(history.replaceState);

window.addEventListener('popstate',()=>{
    window.dispatchEvent(new Event('locationchange'))
});
//code for loading libs
function include(file) {
    let script = document.createElement('script');
    script.src = file;
    script.type = 'text/javascript';
    script.defer = true;
    document.getElementsByTagName('head').item(0).appendChild(script);
}

include("https://cdn.jsdelivr.net/npm/jquery@3.5.1/dist/jquery.min.js");
include("https://cdn.jsdelivr.net/gh/fancyapps/fancybox@3.5.7/dist/jquery.fancybox.min.js");
include("https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.3.4/html2canvas.min.js");
include("https://cdn.jsdelivr.net/npm/jsstore/dist/jsstore.min.js");
include("https://cdn.jsdelivr.net/npm/jsstore/dist/jsstore.worker.min.js");
include("https://cdn.jsdelivr.net/npm/biri/dist/biri.min.js");

const trackingEndPointUrl = "https://localhost:5001";
const uiDataEndPoint = trackingEndPointUrl + "/Tracking/UiData";
const requestDataEndPoint = trackingEndPointUrl + "/Tracking/RequestData";
const eventDataEndPoint = trackingEndPointUrl + "/Tracking/EventData";
const browser = (function (agent) {
    switch (true) {
        case agent.indexOf("edge") > -1: return "edge";
        case agent.indexOf("edg/") > -1: return "edge";
        case agent.indexOf("opr") > -1 && !!window.opr: return "opera";
        case agent.indexOf("chrome") > -1 && !!window.chrome: return "chrome";
        case agent.indexOf("trident") > -1: return "ie";
        case agent.indexOf("firefox") > -1: return "firefox";
        case agent.indexOf("safari") > -1: return "safari";
        default: return "other";
    }
})(window.navigator.userAgent.toLowerCase());

//code for logging requests
const originalOpen = XMLHttpRequest.prototype.open;
const originalSend = XMLHttpRequest.prototype.send;

function handleEvent(e) {
    switch (e.type){
        case "loadend":
            let request = e.currentTarget;
            if(request.RequestId){
                let requestId = request.RequestId;
                let res = request.responseText;
                if(res && res.length>5000){
                    res = res.substring(0,5000)
                }
                updateDataRow(requestTableName, {RequestId: requestId}, {
                    EndTime: getNowStr(),
                    Result: res,
                    ResponseCode: request.status,
                });
            }
            break;
    }
}

function addListeners(xhr) {
    xhr.addEventListener('loadstart', handleEvent);
    xhr.addEventListener('load', handleEvent);
    xhr.addEventListener('loadend', handleEvent);
    xhr.addEventListener('progress', handleEvent);
    xhr.addEventListener('error', handleEvent);
    xhr.addEventListener('abort', handleEvent);
}
XMLHttpRequest.prototype.open = function () {
    console.debug("Requesr Tracking");
    if(arguments[1].indexOf(trackingEndPointUrl)>-1){
        this.RequestId = null;
    }else{
        this.RequestId = uuidv4();
        createRequestTrackingData(this.RequestId, arguments[1], arguments[0]);
    }
    originalOpen.call(this, ...arguments);
}
XMLHttpRequest.prototype.send = function () {
    let paramStr = JSON.stringify(arguments[0]);
    if  (paramStr && paramStr.length>5000) paramStr = paramStr.substring(0, 5000);
    if(this.RequestId){
        updateDataRow(requestTableName, {RequestId: this.RequestId}, {Parameter: paramStr});
    }
    addListeners(this);
    originalSend.call(this, ...arguments);
}

function updateQueryStringParameter(uri, key, value) {
    let re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    let separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      let r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  
async function getTKId() {
    let key = "TKID";
    let tk = localStorage.getItem(key);
    if(tk && tk.length>0) return tk;
    if(["safari","edge","chrome"].indexOf(browser)>-1){
        tk = await biri();
    }else{
        tk = uuidv4();
    }
    localStorage.setItem(key, tk);
    return tk;
}

function getNowStr(){
    return (new Date()).toISOString();
}

async function getUiDataObj(mouseX, mouseY, imageBase64, eventType = "Unknown") {
    let dateTime = getNowStr();
    let pageWidth = $(document).width();
    let pageHeight = $(document).height();
    let href = window.location.href;
    let tkId = await getTKId();
    let value = {
        UiDataId: uuidv4(),
        DateTime: dateTime,
        Href: href,
        EventType: eventType,
        PageWidth: pageWidth,
        PageHeight: pageHeight,
        MouseX: mouseX,
        MouseY: mouseY,
        ImageBase64: imageBase64,
        TrackingId: tkId
    };
    return value;
}


const databaseName = "Tracking";
const uiTableName = "UiTracking";
const eventTableName = "EventTracking";
const requestTableName = "RequestTracking";
let dbConnection = null;
let dbVersion = 17;
async function getDatabaseConnection() {
    if(!window.JsStore) return null;
    if (dbConnection) return dbConnection;
    dbConnection = new window.JsStore.Connection();
    if(dbConnection.version && dbConnection.version !== dbVersion){
        await dbConnection.dropDb();
    }
    let tblUiTracking = {
        name: uiTableName,
        columns: {
            UiDataId: { primaryKey: true , dataType: "string" },
            DateTime: { notNull: true, dataType: "string" },
            Href: { notNull: true, dataType: "string" },
            EventType: { notNull: false, dataType: "string" },
            TrackingId: { notNull: true, dataType: "string" },
            PageWidth: { notNull: true, dataType: "number" },
            PageHeight: { notNull: true, dataType: "number" },
            MouseX: { notNull: true, dataType: "number" },
            MouseY: { notNull: true, dataType: "number" },
            ImageBase64: { notNull: true, dataType: "string" },
        }
    };
    let tblRequestTracking = {
        name: requestTableName,
        columns: {
            RequestId: { primaryKey: true, dataType: "string" },
            DateTime: { notNull: true, dataType: "string" },
            Href: { notNull: true, dataType: "string" },
            TrackingId: { notNull: true, dataType: "string" },
            EndPoint: { notNull: false, dataType: "string" },
            Method: { notNull: false, dataType: "string" },
            StartTime: { notNull: false, dataType: "string" },
            Parameter: { notNull: false, dataType: "string" },
            EndTime: { notNull: false, dataType: "string" },
            Result: { notNull: false, dataType: "string" },
            ResponseCode: { notNull: false, dataType: "number" },
        }
    };
    let tblEventTracking = {
        name: eventTableName,
        columns: {
            DateTime: { primaryKey: true, dataType: "string" },
            EventId: { notNull: true, dataType: "string" },
            Href: { notNull: true, dataType: "string" },
            TrackingId: { notNull: true, dataType: "string" },
            Type: { notNull: false, dataType: "string" },
            EventDetail: { notNull: false, dataType: "string" },
        }
    };
    let db = {
        name: databaseName,
        tables: [tblUiTracking, tblRequestTracking, tblEventTracking],
        version: dbVersion 
    }
    let isDbCreated = await dbConnection.initDb(db);
    // isDbCreated will be true when database will be initiated for first time

    if (isDbCreated) {
        console.debugNoLog('Db Created & connection is opened');
    }
    else {
        console.debugNoLog('Connection is opened');
    }
    return dbConnection;
}
async function createUiTrackingData(imageBase64, mouseX, mouseY) {
    let value = await getUiDataObj(mouseX, mouseY, imageBase64);
    let db = await getDatabaseConnection();
    if(!db) return;
     let noOfRowsInserted = await db.insert({
            into: uiTableName,
            values: [value], //you can insert multiple values at a time
        });
        if (noOfRowsInserted > 0) {
            console.debugNoLog('Successfully Added a row');
        }
}
async function getUiDataFromDb(){
    let db = await getDatabaseConnection();
    if(!db) return;
    let results = await db.select({
        from: uiTableName,
        limit: 50
    });
    return results;
}
async function createRequestTrackingData(requestId, endPoint, method) {
    let now = getNowStr();
    let tkId = await getTKId();
    let row = {
        RequestId: requestId,
        EndPoint: endPoint,
        Method: method,
        DateTime: now,
        StartTime: now,
        TrackingId: tkId,
        Href: window.location.href
    }
    let db = await getDatabaseConnection();
    if(!db) return;
    let noOfRowsInserted = await db.insert({
        into: requestTableName,
        values: [row],
    });
    if (noOfRowsInserted > 0) {
        console.debugNoLog('Successfully Added a request row', requestId);
    }
}
async function createEventTrackingData(type, event){
    let now = getNowStr();
    let eventStr = JSON.stringify(event);
    if(eventStr.length > 5000) eventStr = eventStr.substring(0,5000);
    let tkId = await getTKId();
    let row = {
        EventId: uuidv4(),
        Id: uuidv4(),
        DateTime: now,
        TrackingId: tkId,
        Href: window.location.href,
        Type: type,
        EventDetail: eventStr
    }
    let db = await getDatabaseConnection();
    if(!db) return;
    let noOfRowsInserted = await db.insert({
        into: eventTableName,
        values: [row],
    });
    if (noOfRowsInserted > 0) {
        console.debugNoLog('Successfully Added a event row', row);
    }
}
async function updateDataRow(tableName, keyObj, updateObj) {
    let db = await getDatabaseConnection();
    if(!db) return;
    let noOfRowsUpdated = await db.update({
        in: tableName,
        set: updateObj,
        where: keyObj
    });

    if (noOfRowsUpdated > 0) {
        console.debugNoLog('Successfully update a row', keyObj);
    }
}



async function saveDataToLocalStorage(imageBase64, mouseX, mouseY) {
    //this will not work as max localstorage only 10m for chrome and 5m for others
    let value = await getUiDataObj(mouseX, mouseY, imageBase64);
    localStorage.setItem("TKData_"+value.DateTime, JSON.stringify(value));
}

async function GetDataFromLocalStorage(){
    let results = [];
    for (i = 0; i < window.localStorage.length; i++) {
        key = window.localStorage.key(i);
        if (key.startsWith('TKData_')) {
            results.push(window.localStorage.getItem(key));
        }
        if(results.length >= 50){
            return results;
        }
    }
    return results;
}

async function sendDataToHub(endPoint, data){
    if(typeof data != 'string') data = JSON.stringify(data);
    return $.ajax({
        url: endPoint,
        contentType: "application/json",
        type: "post",
        data: data});
}

async function saveUiData(event){
    const screenshotTarget = document.body;
    html2canvas(screenshotTarget, {
        height:window.innerHeight,
        width:window.innerWidth,
        y:window.pageYOffset,
        x:window.pageXOffset,
        letterRendering: true
    }).then(async (canvas) => {
        if(event.type == "click" ){
            const context = canvas.getContext('2d');
            context.beginPath();
            context.arc(event.pageX, event.pageY, 15, 0, 2 * Math.PI, false);
            context.fillStyle = 'rgba(67, 232, 78, 0.5)';
            context.fill();
            context.lineWidth = 1;
            context.strokeStyle = '#003300';
            context.stroke();
        }
        const base64image = canvas.toDataURL("image/png");
        let uiData = await getUiDataObj(event.pageX, event.pageY, base64image,event.type);
        await sendDataToHub(uiDataEndPoint, [uiData]);
        // saveDataToDb(base64image, event.pageX, event.pageY);
        // saveDataToLocalStorage(base64image, event.pageX, event.pageY);
    });
}

document.addEventListener('DOMContentLoaded', (event) => {
    setTimeout(async () => {        
        let tkId = await getTKId();
    }, 3000);   
    setTimeout(async () => {
        let db = await getDatabaseConnection();
    }, 1000);
  });


document.addEventListener('click', event => {
    saveUiData(event);
});
window.addEventListener('locationchange', event=>{
    event.pageX = 0;
    event.pageY = 0;
    saveUiData(event);
})
setInterval(async () => {
    //send Request log
    let db = await getDatabaseConnection();
    if (!db) return;
    let results = await db.select({
        from: requestTableName,
        limit: 50
    });
    if(results && results.length>0){
        await sendDataToHub(requestDataEndPoint, results);
        for(let res of results){
            let rowsDeleted = await db.remove({
                from: requestTableName,
                where: {
                    RequestId: res.RequestId
                }
            });
        }
    }
}, 60*1000);

setInterval(async ()=>{
    //send Event log
    let db = await getDatabaseConnection();
    if (!db) return;
    let results = await db.select({
        from: eventTableName,
        limit: 50
    });
    if(results && results.length>0){
        await sendDataToHub(eventDataEndPoint, results);
        for(let res of results){
            let rowsDeleted = await db.remove({
                from: eventTableName,
                where: {
                    EventId: res.EventId
                }
            });
        }
    }
}, 30*1000);