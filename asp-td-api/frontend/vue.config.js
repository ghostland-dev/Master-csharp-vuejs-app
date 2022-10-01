module.exports = {
	publicPath: '',
	lintOnSave: false,
	configureWebpack: {
		devServer: {
		  watchOptions: {
			ignored: /node_modules/,
			aggregateTimeout: 1000,
			poll: 500,
		  },
		}
	}
}