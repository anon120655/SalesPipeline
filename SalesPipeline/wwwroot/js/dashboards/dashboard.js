var colorsSuccess = [
	"#dcdcdc",
	"#d7e7cd",
	"#d4e4ca",
	"#cedec3",
	"#cbdbbf",
	"#c6d7ba",
	"#c4d5b8",
	"#bdcfb1",
	"#b5c8a8",
	"#afc2a1",
	"#a8bc9a",
	"#a0b491",
	"#9db18d",
	"#98ad88",
	"#93a883",
	"#8ea47e",
	"#8ba17a",
	"#839971",
	"#7e946c",
	"#788e65",
	"#738a60",
	"#6e865b",
	"#647d51",
	"#5f784c",
	"#5a7447",
	"#556f42",
	"#4d6839",
	"#486334",
	"#425d2d",
	"#3b5726"
];

var colorsWarning = [
	"#dcdcdc",
	"#f1d9c7",
	"#f0cfb9",
	"#efc9b1",
	"#efc4aa",
	"#efbea2",
	"#efb89a",
	"#efb595",
	"#efb190",
	"#efaf8d",
	"#efab88",
	"#efa985",
	"#efa782",
	"#efa57f",
	"#efa17a",
	"#ef9e75",
	"#ef9b70",
	"#ef986b",
	"#ef9566",
	"#ef9363",
	"#ef8e5c",
	"#ef8b57",
	"#ef8954",
	"#ef844d",
	"#ef8148",
	"#ef7e43",
	"#ef7b3e",
	"#ef7839",
	"#ef7432",
	"#ef6f2b"
];

window.closesale = (indata) => {
	var height = 150;
	const data = {
		labels: ["สำเร็จ ", "ไม่สำเร็จ "],
		datasets: [
			{
				data: [40, 10],
				backgroundColor: [
					"#1A68AF",
					"#88C9FF",
				],
				borderColor: [
					"#1A68AF",
					"#88C9FF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			//console.log(chart.data.datasets[0].data[2])
			//console.log(chart.data.datasets[0].data[3])
			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("closesale");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = height + 'px';
		}
	}
}

window.reasonnotloan = (indata) => {
	var height = 150;
	const data = {
		labels: ["ใช้เวลานาน", "ขาดการติดต่อ", "กู้ธนาคารอื่นแล้ว", "ดอกเบี้ยสูง "],
		datasets: [
			{
				data: [10, 20, 30, 40],
				backgroundColor: [
					"#88C9FF",
					"#bbd0eb",
					"#8BAAF9",
					"#1A68AF",
				],
				borderColor: [
					"#88C9FF",
					"#bbd0eb",
					"#8BAAF9",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			//console.log(chart.data.datasets[0].data[2])
			//console.log(chart.data.datasets[0].data[3])
			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 30,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("reasonnotloan");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = height + 'px';
		}
	}
}

window.targetsales = (indata) => {
	var width = 100;
	var height = 150;
	const data = {
		labels: [""],
		datasets: [{
			label: '150,000,000',
			//backgroundColor: "#CCE8FF",
			backgroundColor: 'rgba(46, 117, 183, 0.2)',
			data: [150000000],
			barThickness: 50,
			xAxisID: "bar-x-axis1",
			borderWidth: 0,
		}, {
			label: '135,000,000',
			backgroundColor: "#1378D5",
			//backgroundColor: 'rgba(255, 206, 86, 0.2)',
			data: [135000000],
			barThickness: 50,
			borderWidth: 0,
			//xAxisID: "bar-x-axis2",
		}],
	};

	const config = {
		type: "bar",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			scales: {
				y: {
					display: false,
					border: {
						display: false,
					},
					stacked: true,
					beginAtZero: true,
					grid: {
						drawBorder: false,
						display: false
					}
				},
				x: {
					display: false,
					border: {
						display: false,
					},
					stacked: true,
					grid: {
						drawBorder: false,
						display: false
					}
				},
			},
			//plugins: [ChartDataLabels]
			plugins: {
				legend: {
					display: true,
					labels: {
						textAlign: 'center',
						usePointStyle: true,
						boxHeight: 7,
						color: "black",
						font: {
							family: 'prompt-semibold',
							size: 12
						}
					}
				}
			},
		}
	};

	var ctx = document.getElementById("targetsales");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = height + 'px';
			chart.canvas.parentNode.style.width = width + 'px';
		}
	}
}

window.numcussizebusiness = (indata) => {

	const data = {
		labels: ["ขนาดเล็ก ", "ขนาดกลาง ", "ขนาดใหญ่ "],
		datasets: [
			{
				data: [10, 40, 90],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			//console.log(chart.data.datasets[0].data[2])
			//console.log(chart.data.datasets[0].data[3])
			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("numcussizebusiness");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.numcustypebusiness = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("numcustypebusiness");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.numcusisiccode = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("numcusisiccode");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.numcusloantype = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("numcusloantype");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.valuesizebusiness = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("valuesizebusiness");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.valuetypebusiness = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("valuetypebusiness");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.valueisiccode = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("valueisiccode");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.valueloantype = (indata) => {
	const data = {
		labels: ["บริการ ", "การพาณิชย์ ", "เกษตรกรรม "],
		datasets: [
			{
				data: [20, 9, 75],
				backgroundColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
				borderColor: [
					"#bbd0eb",
					"#97C7FF",
					"#1A68AF",
				],
			},
		],
	};

	// pieLabelsLine plugin
	const pieLabelsLine = {
		id: "pieLabelsLine",
		afterDraw(chart) {
			const {
				ctx,
				chartArea: { width, height },
			} = chart;

			const cx = chart._metasets[0].data[0].x;
			const cy = chart._metasets[0].data[0].y;

			const sum = chart.data.datasets[0].data.reduce((a, b) => a + b, 0);

			chart.data.datasets.forEach((dataset, i) => {
				chart.getDatasetMeta(i).data.forEach((datapoint, index) => {
					const { x: a, y: b } = datapoint.tooltipPosition();

					const x = 2 * a - cx;
					const y = 2 * b - cy;

					// draw line
					const halfwidth = width / 2;
					const halfheight = height / 2;
					const xLine = x >= halfwidth ? x + 10 : x - 10;
					const yLine = y >= halfheight ? y + 10 : y - 10;

					const extraLine = x >= halfwidth ? 5 : -5;

					ctx.beginPath();
					ctx.moveTo(x, y);
					//ctx.arc(x, y, 2, 0, 2 * Math.PI, true);
					ctx.fill();
					ctx.moveTo(x, y);
					ctx.lineTo(xLine, yLine);
					ctx.lineTo(xLine + extraLine, yLine);
					// ctx.strokeStyle = dataset.backgroundColor[index];
					ctx.strokeStyle = "black";
					ctx.stroke();

					// text
					const textWidth = ctx.measureText(chart.data.labels[index]).width;
					ctx.font = "9px prompt-regular";
					// control the position
					const textXPosition = x >= halfwidth ? "left" : "right";
					const plusFivePx = x >= halfwidth ? 5 : -5;
					ctx.textAlign = textXPosition;
					ctx.textBaseline = "middle";
					// ctx.fillStyle = dataset.backgroundColor[index];
					ctx.fillStyle = "black";

					ctx.fillText(
						chart.data.labels[index] + ((chart.data.datasets[0].data[index] * 100) / sum).toFixed(1) + "%",
						xLine + extraLine + plusFivePx,
						yLine
					);
				});
			});
		},
	};
	// config
	const config = {
		type: "pie",
		data,
		options: {
			responsive: true,
			maintainAspectRatio: false,
			layout: {
				padding: 20,
			},
			scales: {
				y: {
					display: false,
					beginAtZero: true,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
				x: {
					display: false,
					ticks: {
						display: false,
					},
					grid: {
						display: false,
					},
				},
			},
			plugins: {
				legend: {
					display: false,
				},
			},
		},
		plugins: [pieLabelsLine],
	};

	var ctx = document.getElementById("valueloantype");
	if (ctx != null) {
		const chart = new Chart(ctx, config);
		if (chart != null) {
			chart.canvas.parentNode.style.height = 150 + 'px';
		}
	}
}

window.topsalescenter = (indata) => {
	setTimeout(function () {
		$(".maptop10 .path_province").each(function (k, v) {
			var dname = $(this).attr("data-name");
			var code = $(this).attr("data-code");
			//console.log(dname, code)
			//$(this).attr("data-value", "0");
			//$(this).attr("data-percent", "0");
		});
	}, 10)

	setTimeout(function () {
		$(".maptop10 .path_province").each(function (k, v) {
			var code = $(this).attr("data-code");
			var dname = $(this).attr("data-name");
			var dvalue = parseFloat($(this).attr("data-value"));
			var dper = parseFloat($(this).attr("data-percent"));
			//console.log(code, dname, dvalue);
			//$(this).attr('style', "opacity:1;fill:" + "#cae0bc");
			var colorIdx = percentToRange(dper);
			$(this).attr('style', "opacity:1;fill:" + colorsSuccess[colorIdx]);
			var moveLeft = 20;
			var moveDown = 10;
			$(this).hover(function (e) {
				$('div#pop-up #mapPopText1').text(dname);
				$('div#pop-up #mapPopText2').text(dvalue.toLocaleString("en-US"));
				$('div#pop-up').show();
			}, function () {
				$('div#pop-up').hide();
			});
			$(this).mousemove(function (e) {
				$("div#pop-up").css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft);
			});
		});
	}, 300)
}

window.centerlost = (indata) => {
	setTimeout(function () {
		$(".mapcenterlost10 .path_province").each(function (k, v) {
			var dname = $(this).attr("data-name");
			var code = $(this).attr("data-code");
			//console.log(dname, code)
			//$(this).attr("data-value", "0");
			//$(this).attr("data-percent", "0");
		});
	}, 10)

	setTimeout(function () {
		$(".mapcenterlost10 .path_province").each(function (k, v) {
			var code = $(this).attr("data-code");
			var dname = $(this).attr("data-name");
			var dvalue = parseFloat($(this).attr("data-value"));
			var dper = parseFloat($(this).attr("data-percent"));
			//console.log(code, dname, dvalue);
			//$(this).attr('style', "opacity:1;fill:" + "#cae0bc");
			var colorIdx = percentToRange(dper);
			$(this).attr('style', "opacity:1;fill:" + colorsWarning[colorIdx]);
			var moveLeft = 20;
			var moveDown = 10;
			$(this).hover(function (e) {
				$('div#pop-up #mapPopText1').text(dname);
				$('div#pop-up #mapPopText2').text(dvalue.toLocaleString("en-US"));
				$('div#pop-up').show();
			}, function () {
				$('div#pop-up').hide();
			});
			$(this).mousemove(function (e) {
				$("div#pop-up").css('top', e.pageY + moveDown).css('left', e.pageX + moveLeft);
			});
		});
	}, 300)
}

window.durationonstage = (indata) => {
	var barOptions_stacked = {
		responsive: true,
		maintainAspectRatio: false,
		indexAxis: 'y',
		tooltips: {
			enabled: false
		},
		hover: {
			animationDuration: 0
		},
		scales: {
			x: {
				stacked: true
			},
			y: {
				stacked: true
			}
		},
		plugins: {
			legend: {
				display: false
			}
		}
	};

	var ctx = document.getElementById("durationonstage");
	if (ctx != null) {
		var chart = new Chart(ctx, {
			type: 'bar',
			data: {
				labels: ["ติดต่อ", "เข้าพบ", "ยื่นเอกสาร", "ผลลัพธ์การกู้", ['ปิดการขาย', 'ไม่สำเร็จ']],
				datasets: [{
					data: [0, 20, 40, 45, 0],
					backgroundColor: "#d5dce4",
					hoverBackgroundColor: "#d5dce4",
					datalabels: {
						display: false
					}
				}, {
					data: [20, 20, 10, 30, 45],
					backgroundColor: "#1378D5",
					hoverBackgroundColor: "#4587c4",
					datalabels: {
						display: true,
						color: "#ffffff",
						font: {
							family: 'prompt-regular',
							size: 9
						}
					}
				}, {
					data: [80, 60, 50, 25, 55],
					backgroundColor: "#d5dce4",
					hoverBackgroundColor: "#d5dce4",
					datalabels: {
						display: false
					}
				}]
			},

			options: barOptions_stacked,
			plugins: [ChartDataLabels]
		});
		if (chart != null) {
			chart.canvas.parentNode.style.height = 350 + 'px';
		}
	}
}

function percentToRange(v) {
	return v > 95 ? 29 : v > 90 ? 28 : v > 85 ? 27 : v > 80 ? 26 : v > 75 ? 25 : v > 70 ? 24 : v > 65 ? 23 : v > 60 ? 22 : v > 55 ? 21 : v > 50 ? 20 : v > 45 ? 19 : v > 40 ? 18 : v > 35 ? 17 : v > 30 ? 16 : v > 25 ? 15 : v > 20 ? 14 : v > 15 ? 13 : v > 10 ? 12 : v > 5 ? 11 : v > 4.5 ? 10 : v > 4 ? 9 : v > 3.5 ? 8 : v > 3 ? 7 : v > 2.5 ? 6 : v > 2 ? 5 : v > 1.5 ? 4 : v > 1 ? 3 : v > 0.5 ? 2 : v > 0.1 ? 1 : 0;
}

function trimSvgWhitespace() {
	var svgs = document.getElementsByTagName("svg");
	for (var i = 0, l = svgs.length; i < l; i++) {
		var svg = svgs[i],
			box = svg.getBBox(),
			viewBox = [box.x, box.y, box.width, box.height].join(" ");
		svg.setAttribute("viewBox", viewBox);
	}
}