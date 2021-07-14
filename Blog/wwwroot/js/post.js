/*Count amount of symbols in the textarea*/
function countSymbolsOfTitle(val) {
    var len = val.value.length;
    if (len > 100) {
        val.value = val.value.substring(0, 200);
    } else {
        document.getElementById('amountOfSymbolsOfTitle').innerHTML = (200 - len);
    }
};

function countSymbolsOfDescription(val) {
    var len = val.value.length;
    if (len > 800) {
        val.value = val.value.substring(0, 800);
    } else {
        document.getElementById('amountOfSymbolsOfDescription').innerHTML = (800 - len);
    }
};

/*Add markdown specific elements to the post content*/
function addElementOnPage(val) {
    var cursorPos = document.getElementById("content").selectionStart;
    var textareaContent = document.getElementById("content").value;
    var output = [textareaContent.slice(0, cursorPos), val, textareaContent.slice(cursorPos)].join('');
    document.getElementById("content").value = output;
}



