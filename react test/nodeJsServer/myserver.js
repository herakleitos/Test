var express = require('express');
var app = express();
app.all('*', function(req, res, next) {
res.header("Access-Control-Allow-Origin", "*");
res.header("Access-Control-Allow-Headers", "X-Requested-With");
res.header("Access-Control-Allow-Methods","PUT,POST,GET,DELETE,OPTIONS");
res.header("X-Powered-By",' 3.2.1')
res.header("Content-Type", "application/json;charset=utf-8");
next();
});

app.get('/', function (req, res) {
res.status(500).send(error);
})
const error={
    code : "500001",
    message : "this is an error"
}
app.get('/json', function (req, res) { //添加的代码
let myjson = {
name : '张三',
age : '36',
sex : '男'
}
res.send(myjson);
})
var server = app.listen(8081, function () {
var host = server.address().address
var port = server.address().port
console.log("应用实例，访问地址为 http://%s:%s", host, port)
})