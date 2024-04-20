window.avgdeal_bar1 = (_data, _labels) => {

	console.log(_data, _labels)

	const data = {
		labels: _labels,
		datasets: [{
			label: '',
			data: _data,
			barThickness: 80,
			backgroundColor: [
				'#1f4e78',
				'#3971a2',
				'#7dadde',
				'#bbd0eb',
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

	var ctx = document.getElementById("avgdeal_bar1");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = '250px';
			chart.canvas.parentNode.style.width = '100%';
		}
	}
}

window.avgdeal_bar2 = (_data) => {
	const data = {
		datasets: [{
			label: '',
			data: [
				{ x: 'ประเทศ', y: 75000, id: 1 },
				{ x: 'ภูมิภาค A', y: 80000, id: 2 },
				{ x: 'ภูมิภาค B', y: 75000, id: 3 },
				{ x: 'ภูมิภาค C', y: 50000, id: 4 },
				{ x: 'ภูมิภาค D', y: 90000, id: 5 },
				{ x: 'ภูมิภาค E', y: 75000, id: 6 },
			],
			barThickness: 15,
			backgroundColor: [
				'#4471c4',
				'#4471c4',
				'#4471c4',
				'#4471c4',
				'#4471c4',
				'#4471c4',
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

	var ctx = document.getElementById("avgdeal_bar2");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = '200px';
			chart.canvas.parentNode.style.width = '100%';
		}

		ctx.onclick = (evt) => {
			const res = chart.getElementsAtEventForMode(evt, 'nearest', { intersect: true }, true);

			// If didn't click on a bar, `res` will be an empty array
			if (res.length === 0) {
				return;
			}

			var id = chart.data.datasets[0].data[res[0].index].id;
			console.log('id=' + id)
			var title = chart.data.datasets[0].data[res[0].index].x;
			console.log('title=' + title)
			var values = chart.data.datasets[0].data[res[0].index].y;
			console.log('value=' + values)

			const nextURL = `https://localhost:7235/dashboard/avgeperdeal/region?id=${id}`;
			window.location.href = nextURL;
		};
	}
}

window.avgdeal_bar3 = (_data) => {
	const data = {
		labels: ["ประเทศ", "ภูมิภาค A", "สาขา 1", "สาขา 2", "สาขา 3"],
		datasets: [{
			label: '',
			data: [75000, 70000, 80000, 50000, 90000],
			barThickness: 20,
			backgroundColor: [
				'#1f4e78',
				'#3971a2',
				'#7dadde',
				'#bbd0eb',
				"#deeaf6",
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

	var ctx = document.getElementById("avgdeal_bar3");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = '200px';
			chart.canvas.parentNode.style.width = '100%';
		}
	}
}

window.avgdeal_bar4 = (_data) => {
	const data = {
		labels: ["ประเทศ", "สาขา 1", "RM 1", "RM 2", "RM 3"],
		datasets: [{
			label: '',
			data: [75000, 70000, 80000, 50000, 90000],
			barThickness: 20,
			backgroundColor: [
				'#1f4e78',
				'#3971a2',
				'#7dadde',
				'#bbd0eb',
				"#deeaf6",
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

	var ctx = document.getElementById("avgdeal_bar4");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = '200px';
			chart.canvas.parentNode.style.width = '100%';
		}
	}
}

window.dropdown_level = () => {

}
