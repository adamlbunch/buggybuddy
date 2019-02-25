setTimeout(function(){ 
    document.getElementById("profile-pic").style.opacity = "1";
    document.getElementById("pic-overlay").style.opacity = "1";
    document.getElementById("profile-name").style.opacity = "1";
    document.getElementById("back-arrow").style.opacity = "1";
     
}, 500);
 
setTimeout(function(){ 
    document.getElementById("profile-info").style.opacity = "1";
}, 700);

setTimeout(function(){ 
    document.getElementById("profile-description").style.opacity = "1";
}, 800);
 
setTimeout(function(){ 
    document.getElementById("search-btn-yes").style.opacity = "1";
    document.getElementById("search-btn-no").style.opacity = "1";
}, 900);

document.getElementById("search-btn-yes").onclick = function() {
    document.getElementById("like-img").style.opacity = "1";

    setTimeout(function(){
        document.getElementById("search-btn-yes").style.top ="0px";
        document.getElementById("search-btn-no").style.top ="0px";
        document.getElementById("search-btn-yes").style.opacity ="0";
        document.getElementById("search-btn-no").style.opacity ="0";
    }, 100);

    setTimeout(function(){
        document.getElementById("profile-info").style.opacity = "0";
        document.getElementById("profile-info").style.top = "0px";
        document.getElementById("profile-description").style.opacity = "0";
        document.getElementById("profile-description").style.top = "0px";
        
    }, 300);

    setTimeout(function(){
        document.getElementById("profile-name").style.opacity = "0";
        document.getElementById("profile-pic").style.width = "100px";
        document.getElementById("profile-pic").style.height = "100px";
        document.getElementById("profile-pic").style.zIndex = "-10";
        document.getElementById("profile-pic").style.boxShadow = "none";
        document.getElementById("pic-overlay").style.width = "100px";
        document.getElementById("pic-overlay").style.height = "100px";
    }, 400);

    setTimeout(function(){
        document.getElementById("like-img").style.opacity = "0";
    }, 500);

    setTimeout(function(){
        document.getElementById("search-buggy").style.opacity = "1";
        document.getElementById("search-buggy").style.right = "-2.2vh";
    }, 500);

    setTimeout(function(){
        document.getElementById("profile-pic").style.top = "31%";
    }, 750);

    setTimeout(function(){
        document.getElementById("profile-pic").style.transition = "0.5s";
        document.getElementById("profile-pic").style.transform = "rotate(-45deg)";
    }, 900);

    setTimeout(function(){
        document.getElementById("profile-pic").style.transition = "0.5s";
        document.getElementById("search-buggy").style.right = "-200vh";
        document.getElementById("profile-pic").style.right = "-450vh";
    }, 1100);

    setTimeout(function(){ 
        document.getElementById("match").submit();
    }, 1300);
};

document.getElementById("search-btn-no").onclick = function() {
    document.getElementById("nope-img").style.opacity = "1";

    setTimeout(function(){
        document.getElementById("search-btn-yes").style.top ="0px";
        document.getElementById("search-btn-no").style.top ="0px";
        document.getElementById("search-btn-yes").style.opacity ="0";
        document.getElementById("search-btn-no").style.opacity ="0";
    }, 100);

    setTimeout(function(){
        document.getElementById("profile-info").style.opacity = "0";
        document.getElementById("profile-info").style.top = "0px";
        document.getElementById("profile-description").style.opacity = "0";
        document.getElementById("profile-description").style.top = "0px";
        
    }, 300);

    setTimeout(function(){
        document.getElementById("profile-name").style.opacity = "0";
        document.getElementById("profile-pic").style.width = "100px";
        document.getElementById("profile-pic").style.height = "100px";
        document.getElementById("profile-pic").style.zIndex = "-10";
        document.getElementById("profile-pic").style.boxShadow = "none";
        document.getElementById("pic-overlay").style.width = "100px";
        document.getElementById("pic-overlay").style.height = "100px";
        //document.getElementById("search-trash").style.top = "90vh";
    }, 400);

    setTimeout(function(){
        document.getElementById("nope-img").style.opacity = "0";
    }, 500);

    /*
    setTimeout(function(){
        document.getElementById("profile-pic").style.top = "60vh";
    }, 700);
    */

    setTimeout(function(){
        document.getElementById("profile-pic").style.top = "100vh";
        document.getElementById("search-trash").style.top = "100vh";
    }, 900);

    setTimeout(function(){ 
        document.getElementById("deny").submit();
    }, 900);
};

