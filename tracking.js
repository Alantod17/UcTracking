
function include(file) {
    var script = document.createElement('script');
    script.src = file;
    script.type = 'text/javascript';
    script.defer = true;
    document.getElementsByTagName('head').item(0).appendChild(script);
}
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
  
function getTKId() {
    var key = "TKID";
    var tk = localStorage.getItem(key);
    if(tk && tk.length>0) return tk;
    tk = uuidv4();
    localStorage.setItem(key, tk);
    return tk;
}

function getDataObj(mouseX, mouseY, imageBase64) {
    var dateTime = (new Date()).toISOString();
    var pageWidth = $(document).width();
    var pageHeight = $(document).height();
    var href = window.location.href;
    var value = {
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
const tableName = "Event";
var dbConnection = null;
async function getDatabaseConnection() {
    if (dbConnection) return dbConnection;
    dbConnection = new window.JsStore.Connection();
    var tblEvent = {
        name: tableName,
        columns: {
            DateTime: { primaryKey: true },
            Href: { notNull: true, dataType: "string" },
            TrackingId: { notNull: true, dataType: "string" },
            PageWidth: { notNull: true, dataType: "number" },
            PageHeight: { notNull: true, dataType: "number" },
            MouseX: { notNull: true, dataType: "number" },
            MouseY: { notNull: true, dataType: "number" },
            ImageBase64: { notNull: true, dataType: "string" },
        }
    };
    var db = {
        name: databaseName,
        tables: [tblEvent]
    }
    var isDbCreated = await dbConnection.initDb(db);
    // isDbCreated will be true when database will be initiated for first time

    if (isDbCreated) {
        console.debug('Db Created & connection is opened');
    }
    else {
        console.debug('Connection is opened');
    }
    return dbConnection;
}
async function saveDataToDb(imageBase64, mouseX, mouseY) {
    var value = getDataObj(mouseX, mouseY, imageBase64);
    var db = await getDatabaseConnection();
     var noOfRowsInserted = await db.insert({
            into: tableName,
            values: [value], //you can insert multiple values at a time
        });
        if (noOfRowsInserted > 0) {
            console.debug('Successfully Added a row');
        }
}
async function getDataFromDb(){
    var db = await getDatabaseConnection();
    var results = await db.select({
        from: tableName,
        limit: 50
    });    
    return results;
}


async function saveDataToLocalStorage(imageBase64, mouseX, mouseY) {
    //this will not work as max localstorage only 10m for chrome and 5m for others
    var value = getDataObj(mouseX, mouseY, imageBase64);
    localStorage.setItem("TKData_"+value.DateTime, JSON.stringify(value));
}

async function GetDataFromLocalStorage(){
    var results = [];
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

async function sendDataToHub(data){    
    if(typeof data === 'object') data = JSON.stringify([data]);
    return $.ajax({
        url: "https://localhost:5001/Tracking/UiData",
        contentType: "application/json",
        type: "post",
        data: data});
}


include("https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.3.4/html2canvas.min.js");
include("https://cdn.jsdelivr.net/npm/jsstore/dist/jsstore.min.js");
include("https://cdn.jsdelivr.net/npm/jsstore/dist/jsstore.worker.min.js");

$(document).ready(async function () {
    setTimeout(async () => {
        var db = await getDatabaseConnection();
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
        sendDataToHub(getDataObj(event.pageX, event.pageY, base64image));
        // saveDataToDb(base64image, event.pageX, event.pageY);
        // saveDataToLocalStorage(base64image, event.pageX, event.pageY);
    });
});

setInterval(function(){
    // var localStorageData = await getDataFromLocalStorage();
    // var dbData = await getDataFromDb();
    // console.debug("LocalData", localStorageData);
    console.debug("DbData");
}, 1000);