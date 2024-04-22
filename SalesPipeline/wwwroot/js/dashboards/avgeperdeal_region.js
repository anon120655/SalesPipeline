window.avgdeal_region_bar = (_data) => {
	//console.log(_data)
	let chartId = "avgdeal_region_bar";
	const canvas = document.getElementById(chartId);
	if (canvas != null && canvas != undefined) {
		let chartStatus = Chart.getChart(chartId);
		if (chartStatus != undefined) {
			chartStatus.destroy();
		}

		const data = {
			datasets: [{
				data: _data,
				//data: [
				//	{ x: 'มกราคม', y: 75000, id: 1 },
				//	{ x: 'กุมภาพันธ์', y: 80000, id: 2 },
				//	{ x: 'มีนาคม', y: 75000, id: 3 },
				//	{ x: 'เมษายน', y: 50000, id: 4 },
				//	{ x: 'พฤษภาคม', y: 90000, id: 5 },
				//	{ x: 'มิถุนายน', y: 75000, id: 6 },
				//	{ x: 'กรกฎาคม', y: 75000, id: 7 },
				//	{ x: 'สิงหาคม', y: 80000, id: 8 },
				//	{ x: 'กันยายน', y: 75000, id: 9 },
				//	{ x: 'ตุลาคม', y: 50000, id: 10 },
				//	{ x: 'พฤศจิกายน', y: 90000, id: 11 },
				//	{ x: 'ธันวาคม', y: 75000, id: 12 },
				//],
				barThickness: 40,
				backgroundColor: [
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
					'#3375b1',
				],
				borderWidth: 1
			}]
		};

		const config = {
			type: 'bar',
			data: data,
			options: {
				responsive: true,
				maintainAspectRatio: false,
				scales: {
					y: {
						beginAtZero: true
					}
				},
				plugins: {
					legend: {
						display: false
					}
				}
			},
		};

		const ctx = canvas.getContext('2d');
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = '200px';
			chart.canvas.parentNode.style.width = '100%';
		}
	}
}
