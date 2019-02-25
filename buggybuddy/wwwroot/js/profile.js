setTimeout(function(){ 
    document.getElementById("profile-pic").style.opacity = "1";
    document.getElementById("pic-overlay").style.opacity = "1";
    document.getElementById("profile-name").style.opacity = "1";
    document.getElementById("profile-title").style.opacity = "1";
    document.getElementById("view-matches-btn").style.opacity = "1";
    document.getElementById("logout-arrow").style.opacity = "1";
}, 500);
 
setTimeout(function(){ 
    document.getElementById("profile-info").style.opacity = "1";
}, 700);

setTimeout(function(){ 
    document.getElementById("profile-description").style.opacity = "1";
}, 800);
 
setTimeout(function(){ 
    document.getElementById("match-btn").style.opacity = "1";
}, 900);

document.getElementById("view-matches-btn").onclick = function() {
    document.getElementById("matches-overlay").style.opacity = "1";
    document.getElementById("profile-pic").style.opacity = "0";
    document.getElementById("pic-overlay").style.opacity = "0";
    document.getElementById("profile-name").style.opacity = "0";
    document.getElementById("profile-title").style.opacity = "0";
    document.getElementById("profile-info").style.opacity = "0";
    document.getElementById("profile-description").style.opacity = "0";
    document.getElementById("view-matches-btn").style.opacity = "0";
    document.getElementById("view-matches-btn").style.visibility = "hidden";
    document.getElementById("match-btn").style.opacity = "0";
    document.getElementById("match-btn").style.visibility = "hidden";
};

document.getElementById("close-matches-btn").onclick = function(){
    document.getElementById("matches-overlay").style.opacity="0";
    document.getElementById("profile-pic").style.opacity = "1";
    document.getElementById("pic-overlay").style.opacity = "1";
    document.getElementById("profile-name").style.opacity = "1";
    document.getElementById("profile-title").style.opacity = "1";
    document.getElementById("profile-info").style.opacity = "1";
    document.getElementById("profile-description").style.opacity = "1";
    document.getElementById("view-matches-btn").style.opacity = "1";
    document.getElementById("view-matches-btn").style.visibility = "visible";
    document.getElementById("match-btn").style.opacity = "1";
    document.getElementById("match-btn").style.visibility = "visible";
};