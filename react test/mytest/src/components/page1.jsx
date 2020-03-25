import React, { Component } from 'react'
import { Button, Input, Alert } from 'antd';
import Styles from './tellme.css'

class page1 extends Component {
    constructor(props) {
        super(props);
        this.login = this.login.bind(this);
        this.logout = this.logout.bind(this);
        this.userNameChange = this.userNameChange.bind(this);
        this.passWordChange = this.passWordChange.bind(this);
        this.state = {
            userName: "",
            passWord: "",
            phone:"",
            userId:"",
            isLogin: 0
        }
    }
    userNameChange(e) {
        console.log('nameChange', e.target.value);
        this.setState({
            userName: e.target.value,
        });
    }
    passWordChange(e) {
        console.log('passWordChange', e.target.value);
        this.setState({
            passWord: e.target.value,
        });
    }
    login() {
        console.log('Login', this.state);
        var obj = {
            userName: this.state.userName,
            passWord: this.state.passWord
        }
        console.log("request-body", JSON.stringify(obj))
        fetch('http://localhost:8080/demo_war/User/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=UTF-8'
            },
            mode: 'cors',
            cache: 'default',
            body: JSON.stringify(obj)
        }).then((res) => {
            return res.json();
        }).then((data) => {
            if (data.success == true) {
                this.setState({
                    isLogin: 1,
                    userName : data.data.userName,
                    userId : data.data.userId,
                    phone : data.data.phone
                });
            } else {
                alert(data.message);
            }
        });
    }
    logout() {
        if (this.state.isLogin == 0) {
            alert('当前未登录');
        }
        this.setState({
            isLogin: 0
        })
    }
    render() {
        return (<div>
            {
                this.state.isLogin == 0 &&
                <div>
                    <div className={Styles.title}>用户登录</div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.narrowTitle}>用户名</span>
                        <Input placeholder="请输入用户名" className={Styles.input} value={this.state.userName} onChange={this.userNameChange}></Input>
                    </div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.narrowTitle}>密码</span>
                        <Input type="password" placeholder="请输入密码" className={Styles.input} onChange={this.passWordChange} value={this.state.passWord}></Input>
                    </div>
                    <div className={Styles.lineContainer}>
                        <div className={Styles.buttonContainer}>
                            <Button className={Styles.button} onClick={() => this.login()}>登录</Button>
                        </div>
                    </div>
                </div>
            }
            {
                this.state.isLogin != 0 &&
                <div>
                    <div className={Styles.title} >欢迎您：{this.state.userName}</div>
                    <div>用户ID: {this.state.userId}</div>
                    <div>电话号码: {this.state.phone}</div>
                    <div className={Styles.lineContainer}>
                        <div className={Styles.buttonContainer}>
                            <Button className={Styles.button} onClick={() => this.logout()}>注销</Button>
                        </div>
                    </div>
                </div>
            }
        </div>)
    }
}
export default page1;