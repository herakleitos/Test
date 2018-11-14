const  webpack = require('webpack');
const  ExtractTextPlugin = require('extract-text-webpack-plugin');
const path = require('path');
const getPath = p => path.resolve(__dirname, p);
const configs = {
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
                    presets: ['es2015', 'react', 'stage-2'],
                    plugins:[
                        ["import", {
                            "libraryName" : "antd",
                            "style" : "css",
                        }]
                    ]
                }
            },
            {
                test: /\.css$/,
                use:ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: [
                        {
                            loader: 'css-loader',
                            options: {
                                module: true,
                                sourceMap:true,
                                minimize: true,
                                localIdentName: '[name]_[local]--[hash:base64:5]',
                            }
                        }
                    ]
                  }),
                  include:[
                      getPath('./src'),
                      getPath('./node_modules/antd'),
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
        new webpack.HotModuleReplacementPlugin(),
        new webpack.EvalSourceMapDevToolPlugin(),
        //这里会按照output的路径打包到css文件夹下面对应的css的名字
        // new ExtractTextPlugin({
        //     filename : 'style.css',
        //     allChunks : true,
        // }),
        new ExtractTextPlugin("styles.css"),
      ],
}
module.exports= configs