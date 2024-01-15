
//แก้กรณี moblie คลิกเมนูแลวเมนูไม่ collapse 
$(document).on("click", ".nav_click", function () {
	console.log('nav_click..')
	let portWidth = window.innerWidth;
	if (portWidth >= 200 && portWidth <= 768) {
		$('.sidebarToggleClick').click()
		clear_tooltip();
	}
});

function clear_tooltip() {
	for (var i = 0; i < 3; i++) {
		setTimeout(function () {
			var hastooltip = $("div").hasClass('tooltip');
			if (hastooltip) {
				$('.tooltip').remove()
			}
		}, 200)
	}
}