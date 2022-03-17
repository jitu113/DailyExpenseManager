$(window).scroll(function () {
    //set scroll position in session storage
    sessionStorage.scrollPos = $(window).scrollTop();
   
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        $("#scroltopId").css('display', 'block')
    } else {
        $("#scroltopId").css('display', 'none')

    }
});
var init = function () {
    if (window.location.href.indexOf('Register') != -1 || window.location.href.indexOf('Login') != -1) {

        $("#navdid").css('visibility', 'hidden');

    }
    //get scroll position in session storage
    $(window).scrollTop(sessionStorage.scrollPos || 0)
};
var initTop = function () {

    //get scroll position in session storage
    $(window).scrollTop(0)
};
window.onload = init;
forMateDate = function (unformattedDate) {

    var strArray = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var formattedDate = new Date(unformattedDate);
    var d = formattedDate.getDate();
    var m = formattedDate.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = formattedDate.getFullYear();
    return strArray[m - 1] + " " + d + "," + y;

}
revertDate = function (formattedDate) {
    var strArray = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    var slicedDate = formattedDate.split(",");
    //var formattedDate = new Date(unformattedDate);
    var m = strArray.indexOf(slicedDate[0].split(" ")[0]) + 1;
    var d = slicedDate[0].split(" ")[1];
    var y = slicedDate[1];
    // JavaScript months are 0-11
    if (d < 10) {
        d = "0" + d;
    }
    if (m < 10) {
        m = "0" + m;
    }
    return y + "-" + m + "-" + d;

}
GetCurrentDate = function () {
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    m += 1;  // JavaScript months are 0-11
    var y = date.getFullYear();
    // JavaScript months are 0-11
    if (d < 10) {
        d = "0" + d;
    }
    if (m < 10) {
        m = "0" + m;
    }
    return y + "-" + m + "-" + d;

}
openNav = function () {

    document.getElementById("mySidenav").style.width = "250px";
}

closeNav = function () {
    document.getElementById("mySidenav").style.width = "0";
}
showabout = function () {

    $("#ResultMsg").text('contact @jayanta.d');

    $("#ResultModal").modal('show');
}
if (window.location.href.toLowerCase().indexOf('register') != -1 || window.location.href.toLowerCase().indexOf('Login') != -1) {

    $("#navdid").css('visibility', 'hidden');

}