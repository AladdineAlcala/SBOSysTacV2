$(document).ready(function() {

    /** add active class and stay opened when selected */
    var url = window.location;
    
    //var action = url.split('/')[3];


    $('ul.sidebar-menu a').filter(function () {
        return this.href != url;
    }).parent().removeClass('active');


    // for sidebar menu entirely but not cover treeview
    $('ul.sidebar-menu a').filter(function () {
        return this.href == url;
    }).parent().addClass('active');
    //Top bar
    $('ul.navbar-nav a').filter(function () {
        return this.href == url;
    }).parent().addClass('active');

    // for treeview
    $('ul.treeview-menu a').filter(function () {
        return this.href == url;
    }).parentsUntil(".sidebar-menu > .treeview-menu").addClass('active');


   // console.log(url);


    //var test = window.location.href.substr(window.location.href.indexOf(parts) > -1);

    //if (window.location.href.indexOf(action) > -1) {
        
    //    console.log('true');
    //}

   



});

