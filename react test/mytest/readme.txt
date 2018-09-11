1. 安装 visual studio code,并安装必要的插件
2. 安装node.js,安装完成后，打开cmd命令行,输入 node -v 和 npm -v 来查看版本号，如果显示则安装完成。
3. 现在我们成功安装了node和npm，然后我们来用npm创建新的项目，首先用npm 安装 create-react-app工具，其可以自动地在本地目录中创建react项目。
在cmd命令行中输入：
npm install -g create-react-app
等待其安装，意思是全局安装create-react-app脚手架工具，然后就可以使用命令创建新的react项目。
4. 在cmd中输入：
create-react-app my-new-app
（默认安装在用户目录下，想更换目录可以使用CD命令）
5. 转移到工程目录下,输入如下命令,部署npm，以及导入react-dom依赖包.
cmd中输入：
npm init   （然后一路回车）
npm install --save react react-dom  ( 在该目录下导入react和react-dom）
npm install --save  react-router-dom   （react路由，以后会用到）
6. npm start 启动项目，Ctrl+C 终止




ps. 引入jquery npm i jquery -S , import $ from  'jquery'