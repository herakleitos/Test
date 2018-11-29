const path = require("path");
const webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const webpackConfigBase = require("./webpack.base.config");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const UglifyJSPlugin = require('uglifyjs-webpack-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const merge = require("webpack-merge");
const webpackConfigProd = {
    devtool: false,
    module: {
        rules: [
             {
                test: /\.bundle\.js$/,
                loader: 'bundle-loader',
                options: {
                    lazy: true,
                    name: '[name]',
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
                            "style" : "css",
                        }]
                    ]
                },
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
                        sourceMap: false,
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
                        sourceMap: false,
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
    plugins:[
        new  CleanWebpackPlugin(["dist"],{
            root: path.join(__dirname,"../")
            },
        ),
        new UglifyJSPlugin(),
        new BundleAnalyzerPlugin(),
    ]
};
module.exports = merge(webpackConfigBase, webpackConfigProd);