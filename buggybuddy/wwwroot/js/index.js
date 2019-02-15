document.getElementById("signup-link").onclick = function(){
	document.getElementsByClassName("flipper")[0].style.transform = "rotateY(180deg)";
	document.getElementsByClassName("heart-logo")[0].style.animationName = "bounce-back";
}

document.getElementById("register-link").onclick = function(){
	document.getElementsByClassName("flipper")[0].style.transform = "rotateY(0deg)";
	document.getElementsByClassName("heart-logo")[0].style.animationName = "bounce-front";
}

document.getElementById("login-btn").onmousedown = function() {
	document.getElementById("login-btn").style.transform = "scale(0.95)";
};


document.getElementById("login-btn").onmouseup = function() {
	document.getElementById("login-btn").style.transform = "scale(1)";
};


document.getElementById("register-btn").onmousedown = function() {
	document.getElementById("register-btn").style.transform = "scale(0.95)";
};


document.getElementById("register-btn").onmouseup = function() {
	document.getElementById("register-btn").style.transform = "scale(1)";
};

//Front end validation, unfinished
function validateForm() {
    var x = document.forms["form"]["login-username"].value;
	var y = document.forms["form"]["login-password"].value;
    if (x == "") {
        document.getElementById("username-validate").style.opacity = "1";
		document.getElementById("username-validate-triangle").style.opacity = "1";

		if (y == ""){
			document.getElementById("password-validate").style.opacity = "1";
			document.getElementById("password-validate-triangle").style.opacity = "1";
		}
    } 

	else if (x != ""){
		document.getElementById("username-validate").style.opacity = "0";
		document.getElementById("username-validate-triangle").style.opacity = "0";

		if (y != ""){
			document.getElementById("password-validate").style.opacity = "0";
			document.getElementById("password-validate-triangle").style.opacity = "0";

		}
	}
}

//Preview img
  var loadFile = function(event) {
    var output = document.getElementById('output');
	document.getElementById("register-picture-default").style.visibility = "hidden";
	document.getElementById("register-picture-label").style.visibility = "hidden";
    output.src = URL.createObjectURL(event.target.files[0]);
  };