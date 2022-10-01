<template>
	<Chart :data="chartData" :type="chartType" :options="options" />    
</template>
<script>
	import Chart from 'primevue/chart';
    export default {
		components: {
			'Chart': Chart,
		},
		props: {
			chartType: {
				type: String,
				default: 'bar'
			},
			responsive: {
				type: Boolean,
				default: true
			},
			gridlines: {
				type: Boolean,
				default: true
			},
			ticks: {
				type: Boolean,
				default: true
			},
			beginzero: {
				type: Boolean,
				default: true
			},
			width: {
				type: String,
				default: '400'
			},
			height: {
				type: String,
				default: '300'
			},
			xlabel: {
				type: String,
				default: ''
			},
			ylabel: {
				type: String,
				default: ''
			},
			chartDesign: {
				type: Array,
				default: []
			},
			chartData: null,
		},
		data: function() {
			return {
				options: {
					scales: {
						yAxes: [{
							gridLines:{ display: true },
							ticks: {
								beginAtZero: this.beginzero,
								display:true
							},
							scaleLabel: {
								display: true,
								labelString: ''
							}
						}],
						xAxes: [{
							gridLines:{ display: true},
							ticks: { display: true },
							scaleLabel: {
								display: true,
								labelString: ''
							}
						}],
					},
					maintainAspectRatio : true,
					responsive : this.responsive,
				},
				loading: false,
			};
		},
	}
</script>