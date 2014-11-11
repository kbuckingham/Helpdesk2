var faded = true;
function pendingOpen() {
    div = document.getElementById('pending');
    div.style.display = "block";
    if (faded == true) {
        fadeIn();
    }
    else {
        fadeOut();
    }
}

function printDiv() {
    window.print();
}

function begin() {
    setOpacity(0);
}

function fadeIn() {
    for (var i = 0; i <= 100; i++)
        setTimeout("setOpacity(" + (i / 10) + ")", (8 * i));
    faded = false;
}

function fadeOut() {
    for (var i = 0; i <= 100; i++)
        setTimeout("setOpacity(" + (10 - i / 10) + ")", (8 * i));
    faded = true;
}

function setOpacity(value) {
    var element = document.getElementById('pending');
    element.style.opacity = value / 10;
    element.style.filter = 'alpha(opacity=' + value * 10 + ')';
}