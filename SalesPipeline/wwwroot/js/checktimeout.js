function timeOutCall(dotnethelper) {
	document.onmousemove = resetTimeDelay;
	document.onkeyup = resetTimeDelay;
	document.onclick = resetTimeDelay;
	//document.onkeypress = resetTimeDelay;

	function resetTimeDelay() {
		var _path = window.location.pathname;
		console.log(_path)
		//if (_path == "/user") {
		console.log('resetTimeDelay...')
		dotnethelper.invokeMethodAsync("TimerInterval");
		//}
	}
}

function initializeinactivitytimer(dotnetHelper) {
	var timer;
	//the timer will be reset whenever the user clicks the mouse or presses the keyboard
	document.onmousemove = resetTimer;
	document.onkeyup = resetTimer;
	document.onclick = resetTimer;

	function resetTimer() {
		//milliseconds
		//1000 = 1 วินาที //1000*5 = 5 วินาที
		//60000 = 1 นาที //60000*3 = 3 นาที
		//600000 = 10 minuts //600000*3 = 30 นาที
		var milliseconds = 60000 * 15;
		var _path = window.location.pathname;
		clearTimeout(timer);
		if (_path == "/user") {
			//milliseconds = 10000;
		}

		timer = setTimeout(logout, milliseconds);
	}

	function logout() {
		dotnetHelper.invokeMethodAsync("ConfirmLogout");
	}

}