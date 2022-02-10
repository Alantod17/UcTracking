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


const uiDataEndPoint = "https://localhost:5001/Tracking/UiData";
const requestDataEndPoint = "https://localhost:5001/Tracking/RequestData";
const eventDataEndPoint = "https://localhost:5001/Tracking/EventData";

//code for logging requests
const originalOpen = XMLHttpRequest.prototype.open;
const originalSend = XMLHttpRequest.prototype.send;

function handleEvent(e) {
    switch (e.type){
        case "loadend":
            let request = e.currentTarget;
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
    this.RequestId = uuidv4();
    createRequestTrackingData(this.RequestId, arguments[1], arguments[0]);
    originalOpen.call(this, ...arguments);
}
XMLHttpRequest.prototype.send = function () {
    let paramStr = JSON.stringify(arguments[0]);
    if  (paramStr && paramStr.length>5000) paramStr = paramStr.substring(0, 5000);
    updateDataRow(requestTableName, {RequestId: this.RequestId}, {Parameter: paramStr});
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
  
function getTKId() {
    let key = "TKID";
    let tk = localStorage.getItem(key);
    if(tk && tk.length>0) return tk;
    tk = uuidv4();
    localStorage.setItem(key, tk);
    return tk;
}

function getNowStr(){
    return (new Date()).toISOString();
}

function getUiDataObj(mouseX, mouseY, imageBase64) {
    let dateTime = getNowStr();
    let pageWidth = $(document).width();
    let pageHeight = $(document).height();
    let href = window.location.href;
    let value = {
        UiDataId: uuidv4(),
        DateTime: dateTime,
        Href: href,
        PageWidth: pageWidth,
        PageHeight: pageHeight,
        MouseX: mouseX,
        MouseY: mouseY,
        ImageBase64: imageBase64,
        TrackingId: getTKId()
    };
    return value;
}


const databaseName = "Tracking";
const uiTableName = "UiTracking";
const eventTableName = "EventTracking";
const requestTableName = "RequestTracking";
let dbConnection = null;
let dbVersion = 16;
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
    let value = getUiDataObj(mouseX, mouseY, imageBase64);
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
    let row = {
        RequestId: requestId,
        EndPoint: endPoint,
        Method: method,
        DateTime: now,
        StartTime: now,
        TrackingId: getTKId(),
        Href: window.location.href
    }
    let db = await getDatabaseConnection();
    if(!db) return;
    let noOfRowsInserted = await db.insert({
        into: requestTableName,
        values: [row],
    });
    if (noOfRowsInserted > 0) {
        console.debugNoLog('Successfully Added a row', requestId);
    }
}
async function createEventTrackingData(type, event){
    let now = getNowStr();
    let eventStr = JSON.stringify(event);
    if(eventStr.length > 5000) eventStr = eventStr.substring(0,5000);
    let row = {
        EventId: uuidv4(),
        Id: uuidv4(),
        DateTime: now,
        TrackingId: getTKId(),
        Href: window.location.href,
        Type: type,
        EventDetail: eventStr
    }
    console.debugNoLog(row);
    let db = await getDatabaseConnection();
    if(!db) return;
    let noOfRowsInserted = await db.insert({
        into: eventTableName,
        values: [row],
    });
    if (noOfRowsInserted > 0) {
        console.debugNoLog('Successfully Added a row');
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
    let value = getUiDataObj(mouseX, mouseY, imageBase64);
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

document.addEventListener('DOMContentLoaded', (event) => {
    setTimeout(async () => {
        let db = await getDatabaseConnection();
    }, 1000);   
  });


document.addEventListener('click', event => {
    const screenshotTarget = document.body;
    html2canvas(screenshotTarget, {
        height:window.innerHeight,
        width:window.innerWidth,
        y:window.pageYOffset,
        x:window.pageXOffset,
    }).then(async (canvas) => {
        const base64image = canvas.toDataURL("image/png");
        await sendDataToHub(uiDataEndPoint, [getUiDataObj(event.pageX, event.pageY, base64image)]);
        // saveDataToDb(base64image, event.pageX, event.pageY);
        // saveDataToLocalStorage(base64image, event.pageX, event.pageY);
    });
});

setInterval(async () => {
    //send Request log
    console.debugNoLog("RequestLog");
    let db = await getDatabaseConnection();
    if (!db) return;
    let results = await db.select({
        from: requestTableName,
        limit: 50
    });
    if(results && results.length>0){
        // await sendDataToHub(requestDataEndPoint, results);
        for(let res in results){
            let rowsDeleted = await db.remove({
                from: requestTableName,
                where: {
                    RequestId: res.RequestId
                }
            });
            console.debugNoLog("Event Row deleted "+rowsDeleted + " RequestId: "+res.RequestId);
        }
    }
}, 5*1000);
setInterval(async ()=>{
    //send Event log
    let db = await getDatabaseConnection();
    if (!db) return;
    let results = await db.select({
        from: eventTableName,
        limit: 50
    });
    if(results && results.length>0){
        // await sendDataToHub(eventDataEndPoint, results);
        for(let res of results){
            console.debugNoLog(res);
            let rowsDeleted = await db.remove({
                from: eventTableName,
                where: {
                    EventId: res.EventId
                }
            });
        }
    }
}, 30*1000);