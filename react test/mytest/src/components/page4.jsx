import React, { Component } from 'react'
import { Button, Input } from 'antd';
import Styles from './tellme.css'

class page4 extends Component {
    constructor(props) {
        super(props);
        this.modify = this.modify.bind(this);
        this.userNameChange = this.userNameChange.bind(this);
        this.phoneChange = this.phoneChange.bind(this);
        this.int = this.int.bind(this);
        this.userId = this.props.location.state;
        this.state = {
            userName: "",
            passWord: "",
            phone: "",
            userId: this.userId
        }
    }
    userNameChange(e) {
        console.log('nameChange', e.target.value);
        this.setState({
            userName: e.target.value,
        });
    }
    phoneChange(e) {
        console.log('phoneChange', e.target.value);
        this.setState({
            phone: e.target.value,
        });
    }
    componentDidMount() {
        this.int();
    }
    int() {

        if(this.userId==undefined || this.userId<=0){
            alert("请选择用户!");
            this.props.history.push("/item2/page3");
        }
        fetch('http://localhost:8080/demo_war/UserInfo/User?userId='+this.userId, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json;charset=UTF-8'
            },
            mode: 'cors',
            cache: 'default',
        }).then((res) => {
            return res.json();
        }).then((data) => {
            if (data.success == true) {
                console.log(data.data);
                this.setState({
                    userId: data.data.userId,
                    userName: data.data.userName,
                    phone: data.data.phone
                });
            } else {
            }
        });
    };
    modify(){
        console.log('add', this.state);
        var obj = {
            userName: this.state.userName,
            phone: this.state.phone,
            userId:this.state.userId
        }
        console.log("request-body", JSON.stringify(obj))
        fetch('http://localhost:8080/demo_war/UserInfo/User', {
            method: 'PUT',
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
                alert("修改成功");            
                this.props.history.push( '/item2/page3')
            } else {
                alert(data.message);
            }
        });
    }
    render() {
        return (<div>
            <div>
                <div className={Styles.title}>修改用户</div>
                <div className={Styles.lineContainer}>
                    <span className={Styles.narrowTitle}>用户名</span>
                    <Input placeholder="请输入用户名" className={Styles.input} value={this.state.userName} onChange={this.userNameChange}></Input>
                </div>
                <div className={Styles.lineContainer}>
                    <span className={Styles.narrowTitle}>电话号码</span>
                    <Input placeholder="请输入电话号码" className={Styles.input} onChange={this.phoneChange} value={this.state.phone}></Input>
                </div>
                <div className={Styles.lineContainer}>
                    <div className={Styles.buttonContainer}>
                        <Button className={Styles.button} onClick={() => this.modify()}>确认修改</Button>
                    </div>
                </div>
            </div>
        </div>)
    }
}
export default page4;