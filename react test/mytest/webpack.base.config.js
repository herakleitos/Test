const path = require('path');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const configs = {
    entry: path.resolve(__dirname, 'src/index.js'), //相对路径
    output: {
        path: path.resolve(__dirname, 'dist'), //打包文件的输出路径
        filename: 'bundle.js',
    },
    resolve: {
        extensions: ['.js', '.jsx'], //后缀名自动补全
    },
    plugins: [
        new ExtractTextPlugin({
            filename: 'styles.css',
            allChunks: true,
         })
      ],
}
module.exports= configs