const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
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
/*             {
                test: /\.css$/,
                exclude:/node_modules/,
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
                test: /\.css$/,
                exclude:/src/,
                use: ['style-loader',
                    {
                        loader: 'css-loader',
                    }
                ]
            }, */
            {
                test: /\.css$/,
                exclude:/node_modules/,
                use: ExtractTextPlugin.extract({
                  fallback: "style-loader",
                  use: [
                    {
                      loader: 'css-loader',
                      options: {
                        modules: true,
                        sourceMap: true,
                        minimize: true,
                        localIdentName: '[name]_[local]--[hash:base64:5]',
                      },
                    }
                  ],
                }),
            },
            {
                test: /\.css$/,
                exclude:/src/,
                use: ExtractTextPlugin.extract({
                  fallback: "style-loader",
                  use: [
                    {
                      loader: 'css-loader',
                      options: {
                        modules: false,
                        sourceMap: true,
                        minimize: true,
                      },
                    }
                  ],
                }),
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
        new ExtractTextPlugin("styles.css"),
      ],
}
module.exports= configs