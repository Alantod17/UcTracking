
function include(file) {
    var script  = document.createElement('script');
    script.src  = file;
    script.type = 'text/javascript';
    script.defer = true;    
    document.getElementsByTagName('head').item(0).appendChild(script);    
}

include("https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.3.4/html2canvas.min.js");
document.addEventListener('click', event => {
    console.log("button clicked");
});