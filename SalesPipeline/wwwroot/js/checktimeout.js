function timeOutCall(dotnethelper) {
	document.onmousemove = resetTimeDelay;
	document.onkeyup = resetTimeDelay;
	document.onclick = resetTimeDelay;
	//document.onkeypress = resetTimeDelay;

	function resetTimeDelay() {
		var _path = window.location.pathname;
		console.log(_path)
		if (_path == "/loans/user") {
			console.log('resetTimeDelay...')
			dotnethelper.invokeMethodAsync("TimerInterval");
		}
	}
}

function initializeinactivitytimer(dotnetHelper) {
	var timer;
	//the timer will be reset whenever the user clicks the mouse or presses the keyboard
	document.onmousemove = resetTimer;
	document.onkeyup = resetTimer;
	document.onclick = resetTimer;

	function resetTimer() {
		var milliseconds = 600000; //600,000 milliseconds = 10 minuts
		var _path = window.location.pathname;
		clearTimeout(timer);
		if (_path == "/loans/user") {
			//milliseconds = 10000;
		}
		//console.log(milliseconds);

		timer = setTimeout(logout, milliseconds);
	}

	function logout() {
		dotnetHelper.invokeMethodAsync("ConfirmLogout");
	}

}