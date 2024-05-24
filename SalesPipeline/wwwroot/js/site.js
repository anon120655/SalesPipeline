
//`````````````````````````````````````````````

//new PDF
window.jsPDF = window.jspdf.jsPDF;
var baseUriWeb = '';
var _baseUriApi = '';

window.AfterRenderMainLayout = () => {

	baseUriWeb = $('#baseUriWeb').val()
	_baseUriApi = $('#baseUriApi').val()

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

window.ChangeCursorPointer = () => {
	document.documentElement.style.cursor = 'pointer';
}

window.AddCursorWait = () => {
	$("body").addClass("waiting");
	//document.documentElement.style.cursor = 'wait';
}

window.RemoveCursorWait = () => {
	$("body").removeClass("waiting");
	//document.documentElement.style.cursor = 'wait';
}

window.ChangeUrl = function (url) {
	history.pushState(null, '', url);
}

window.selectPickerInitialize = () => {
	$('.selectpicker').selectpicker();
}

window.selectPickerRender = () => {
	$('.selectpicker').selectpicker('render');
}

window.BootSelectRefreshClass = (className) => {
	setTimeout(function () {
		$(`.${className}`).selectpicker('destroy');
		$(`.${className}`).selectpicker('render');
		//$(`.${className}`).selectpicker('setStyle', 'btn_white');
		//$(`.${className}`).selectpicker('refresh');
	}, 100)
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

window.BootSelectRefreshClass = (elm_class, time = 10) => {
	setTimeout(function () {
		$(`.${elm_class}`).selectpicker('refresh');
	}, time)
}

window.BootSelectDestroy = (elm_id) => {
	$(`#${elm_id}`).selectpicker('destroy');
}

window.BootSelectDestroyClass = (elm_class) => {
	$(`.${elm_class}`).selectpicker('destroy');
}

window.BootSelectEmptyID = (elm_id) => {
	$(`#${elm_id}`).selectpicker('destroy');
	$(`#${elm_id}`).selectpicker('setStyle', 'btn_white');
	$(`#${elm_id}`).selectpicker('refresh');
}

window.BootSelectEmptyClass = (elm_class) => {
	$(`.${elm_class}`).selectpicker('destroy');
	$(`.${elm_class}`).selectpicker('setStyle', 'btn_white');
	$(`.${elm_class}`).selectpicker('refresh');
}

window.InitSelectPicker = (dotnetHelper, callbackMethodName, pickerElementName, dataName = null) => {
	// initialize the specified picker element
	$(pickerElementName).selectpicker('setStyle', 'btn_white');

	//console.log($(pickerElementName))
	// setup event to push the selected dropdown value back to c# code
	$(pickerElementName).on('changed.bs.select', function (e, clickedIndex, isSelected, previousValue) {
		//console.log($(pickerElementName).val(), $(pickerElementName + ' option:selected').text())
		if (dataName == null) {
			dotnetHelper.invokeMethodAsync(callbackMethodName, $(pickerElementName).val(), $(pickerElementName + ' option:selected').text())
				.then(data => {

				});
		} else {
			dotnetHelper.invokeMethodAsync(callbackMethodName, $(pickerElementName).val(), $(pickerElementName + ' option:selected').text(), $(pickerElementName + ' option:selected').attr(`data-${dataName}`))
				.then(data => {

				});
		}
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

window.saveAsFile = function (fileName, byteBase64) {
	var link = this.document.createElement('a');
	link.download = fileName;
	link.href = "data:application/octet-stream;base64," + byteBase64;
	this.document.body.appendChild(link);
	link.click();
	this.document.body.removeChild(link);
}

window.captureDashboard = (name) => {
	var capture = document.getElementById("capture");
	$(".hide_export").addClass("d-none")
	$(".el_box_con").removeClass("box_content").addClass("box_content_n_shadow")
	//PDF
	if (capture != null) {
		html2canvas(capture).then(canvas => {
			let currentDate = GetCurrentDate()
			var imgData = canvas.toDataURL('image/png');
			var doc = new jsPDF('p', 'mm')

			const imgProps = doc.getImageProperties(imgData);
			const pdfWidth = doc.internal.pageSize.getWidth();
			const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;
			doc.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
			doc.save(`${name}_${currentDate}.pdf`)
			$(".hide_export").removeClass("d-none")
			$(".el_box_con").removeClass("box_content_n_shadow").addClass("box_content")
		});
	}

	//if (capture != null) {
	//	html2canvas(capture).then(canvas => {
	//		var myImage = canvas.toDataURL();
	//		let currentDate = GetCurrentDate()
	//		downloadURI(myImage, `Dashboard_${currentDate}.png`);
	//		$(".hide_export").removeClass("d-none")
	//	});
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

function downloadURI(uri, name) {
	var link = document.createElement("a");

	link.download = name;
	link.href = uri;
	document.body.appendChild(link);
	link.click();
	//after creating link you should delete dynamic link
	//clearDynamicLink(link); 
}

function GetCurrentDate() {
	const date = new Date();

	let day = date.getDate();
	let month = date.getMonth() + 1;
	let year = date.getFullYear();

	if (day < 10) day = '0' + day;
	if (month < 10) month = '0' + month;

	let currentDate = `${day}-${month}-${year}`;
	return currentDate;
}

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
