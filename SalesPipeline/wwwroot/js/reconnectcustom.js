
new MutationObserver((mutations, observer) => {
	if (document.querySelector('#components-reconnect-modal h5 a')) {
		// Now every 1 seconds, see if the server appears to be back, and if so, reload
		async function Reconnection() {
			await fetch(''); // Check the server really is back
			location.reload();
		}
		observer.disconnect();
		Reconnection();
		setInterval(Reconnection, 1000);
	}
}).observe(document.body, { childList: true, subtree: true });