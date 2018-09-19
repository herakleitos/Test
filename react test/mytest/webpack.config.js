var webpack = require('webpack');
const path = require('path');
module.exports = {
    entry: path.resolve(__dirname, 'src/index.js'), //相对路径
    output: {
        path: path.resolve(__dirname, 'build'), //打包文件的输出路径
        filename: 'bundle.js' //打包文件名
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
                query: {
                    plugins: ['transform-runtime'],
                    presets: ['es2015', 'react', 'stage-2']
                }
            },
            {
                test: /\.css$/,
                use: ['style-loader',
                    {
                        loader: 'css-loader',
                        options: {
                            modules: true
                        }
                    }
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    'css-loader',
                    'sass-loader'
                ]
            },
            {
                test: /\.(png|svg|jpg|gif|mp4)$/,
                use: ['file-loader']
            }
        ]
    },
    devServer: {
        hot: true,
        inline: true,
        port: 9000,
        open:true,
        contentBase: path.resolve(__dirname, 'build'),
        historyApiFallback: true,
    },
    resolve: {
        extensions: ['.js', '.jsx'], //后缀名自动补全
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin()
      ],
};