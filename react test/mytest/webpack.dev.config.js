const path = require("path");
const webpack = require('webpack');
const merge = require("webpack-merge");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const webpackConfigBase = require("./webpack.base.config");
 
const webpackConfigDev = {
    devtool: 'cheap-module-eval-source-map',
    mode:'development',
    module: {
        rules: [
             {
                test: /\.bundle\.js$/,
                loader: 'bundle-loader',
                options: {
                    lazy: true,
                    name: '[name]'
                }
            },
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                loader: 'babel-loader',
                query: {
                    presets: ['es2015', 'react', 'stage-2'],
                    plugins:[
                        ["import", {
                            "libraryName" : "antd",
                            "libraryDirectory": "es",
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
        contentBase: path.join(__dirname,"./dist"),
        historyApiFallback: true,
    },
    plugins: [
        new webpack.HotModuleReplacementPlugin(),
        new webpack.EvalSourceMapDevToolPlugin(),
      ],
}
module.exports = merge(webpackConfigBase, webpackConfigDev);