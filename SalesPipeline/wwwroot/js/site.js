window.AfterRenderMainLayout = () => {

	//_baseUriApi = $('#baseUriApi').val()

	setTimeout(function () {
		var sidebarToggleAll = document.querySelectorAll('.sidebarToggleClick');
		for (var i = 0; i < sidebarToggleAll.length; i++) {
			sidebarToggleAll[i].addEventListener('click', event => {
				event.preventDefault();
				document.body.classList.toggle('sb-sidenav-toggled');
				localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
			});
		}
	}, 1000)

}

window.selectPickerInitialize = () => {
	$('.selectpicker').selectpicker();
}

window.selectPickerRender = () => {
	$('.selectpicker').selectpicker('render');
}

window.BootSelectId = (id) => {
	$(`#${id}`).selectpicker('setStyle', 'btn_white');
}

window.BootSelectClass = (className) => {
	$(`.${className}`).selectpicker('setStyle', 'btn_white');
}

window.BootSelectRefreshID = (elm_id, time = 10) => {
	setTimeout(function () {
		$(`#${elm_id}`).selectpicker('refresh');
	}, time)
}

window.BootSelectEmptyID = (elm_id) => {
	$(`#${elm_id}`).selectpicker('destroy');
	$(`#${elm_id}`).selectpicker('setStyle', 'btn_white');
	$(`#${elm_id}`).selectpicker('refresh');
}

window.InitSelectPicker = (dotnetHelper, callbackMethodName, pickerElementName) => {
	// initialize the specified picker element
	$(pickerElementName).selectpicker('setStyle', 'btn_white');

	//console.log($(pickerElementName))
	// setup event to push the selected dropdown value back to c# code
	$(pickerElementName).on('changed.bs.select', function (e, clickedIndex, isSelected, previousValue) {
		//console.log($(pickerElementName).val(), $(pickerElementName + ' option:selected').text())
		dotnetHelper.invokeMethodAsync(callbackMethodName, $(pickerElementName).val(), $(pickerElementName + ' option:selected').text())
			.then(data => {

			});
	});
}

window.scrollToElement = (id) => {
	var element = document.getElementsByClassName(id);
	if (element != null && element.length > 0) {
		var requiredElement = element[0];
		var elementPosition = requiredElement.offsetTop;
		window.scrollTo({
			top: elementPosition - 40, //add your necessary value
			behavior: "smooth"  //Smooth transition to roll
		});
	}
}

$(document).on("keypress", ".numberonly", function (e) {
	return (e.charCode != 8 && e.charCode == 0 || (e.charCode >= 48 && e.charCode <= 57) || e.charCode == 46);
});
$(document).on("paste", ".numberonly", function (e) {
	e.preventDefault();
	let paste = (e.originalEvent.clipboardData || window.clipboardData).getData('text');
	this.value = paste.replace(/\D/g, '');
	this.dispatchEvent(new Event('change'));
});

function enforceMinMax(el) {
	if (el.value != "") {
		if (parseInt(el.value) < parseInt(el.min)) {
			el.value = el.min;
		}
		if (parseInt(el.value) > parseInt(el.max)) {
			el.value = el.max;
		}
	}
}

function SuccessAlert(text = '<i class="fa-regular fa-circle-check me-2"></i> บันทึกสำเร็จ', delay = 3) {
	alertify.set('notifier', 'delay', delay);
	alertify.set('notifier', 'position', 'top-center');
	alertify.success(text);
}

function WarningAlert(text = 'Warning', delay = 4) {
	alertify.set('notifier', 'delay', delay);
	alertify.set('notifier', 'position', 'top-center');
	alertify.warning(`<i class="fa-solid fa-triangle-exclamation me-2"></i> ${text}`);
}

function ConfirmAlert(message) {
	return new Promise(resolve => {
		alertify.confirm(message,
			function () { resolve(true) },
			function () { resolve(false) }
		).set({ title: "Confirm" }).set({ labels: { cancel: 'ยกเลิก', ok: 'ตกลง' } });
	});
}

function loadJs(sourceUrl, pathname = '') {

	return new Promise(resolve => {

		if (sourceUrl.Length == 0) {
			console.error("Invalid source URL")
			return;
		}

		if (!isMyScriptLoaded(pathname)) {
			var tag = document.createElement('script');
			tag.type = "text/javascript";
			tag.charset = "utf-8";
			tag.src = sourceUrl;

			tag.onload = function () {
				resolve(true);
				//console.log("Script loaded successfully", sourceUrl);
			}

			tag.onerror = function () {
				console.error("Failed to load script");
			}

			tag.onsuccess = function () {
				console.error("Failed to load script");
			}
			const loadScript = document.querySelector('#LoadScript');
			loadScript.append(tag);
		} else {
			resolve(true);
		}

	});

}

function isMyScriptLoaded(pathname) {
	//console.log('pathname', pathname)
	var scripts = document.getElementsByTagName('script');
	for (var i = scripts.length; i--;) {
		//console.log('src', scripts[i].src)
		if (scripts[i].src.includes(pathname)) return true;
	}
	return false;
}
