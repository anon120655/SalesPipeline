
new MutationObserver((mutations, observer) => {
	if (document.querySelector('#components-reconnect-modal h5 a')) {
		async function Reconnection() {
			await fetch('');
			location.reload();
		}
		observer.disconnect();
		Reconnection();
		setInterval(Reconnection, 1000);
	}
}).observe(document.body, { childList: true, subtree: true });


//window.addEventListener('offline', () => {
//	console.log('You are now offline.');
//});

//window.addEventListener('online', () => {
//	console.log('You are back online.');
//});

//window.BlazorReconnectHandler = {
//	reconnect: function () {
//		console.log('Attempting to reconnect to the server...');
//		alert('Attempting TryReconnect')
//		setTimeout(() => {
//			DotNet.invokeMethodAsync('SalesPipeline', 'TryReconnect')
//				.then(() => console.log('Reconnected successfully.'))
//				.catch(err => console.error('Reconnection failed:', err));
//		}, 5000); // Try to reconnect every 5 seconds
//	}
//};

//document.addEventListener('blazor:error', (event) => {
//	console.error('Blazor error:', event.detail);
//	window.BlazorReconnectHandler.reconnect();
//});

//window.storage = {
//	saveState: function (key, state) {
//		localStorage.setItem(key, JSON.stringify(state));
//	},
//	loadState: function (key) {
//		return JSON.parse(localStorage.getItem(key));
//	}
//};