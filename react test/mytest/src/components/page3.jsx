import React, { Component } from 'react'
import { Table,Popconfirm } from 'antd'
import { Link } from 'react-router-dom'
import Styles from './tellme.css'
class page3 extends Component {
    constructor(props) {
        super(props);
        this.int = this.int.bind(this);
        this.delete =this.delete.bind(this);
        this.state = {
            data: []
        }
    }
    columns = [
        {
            title: '用户ID',
            dataIndex: 'userId',
            key: 'userId',
            render: text => <a>{text}</a>,
            width: "100px",
        },
        {
            title: '用户名',
            dataIndex: 'userName',
            key: 'userName',
            width: "300px",
        },
        {
            title: 'phone',
            dataIndex: 'phone',
            key: 'phone',
        },
        {
            title: '操作',
            key: 'action',
            render: (text, record) => (
                <div>
                    <Link to={{ pathname: '/item2/page4', state: record.userId }}>修改</Link>
                    <Popconfirm title="确定删除改记录么?" okText="确定" cancelText="取消" onConfirm={() => this.delete(record.userId)}>
                        <a className={Styles.deleteButton}>删除</a>
                    </Popconfirm>
                </div>
            ),
            width: "200px",
        },
    ];
    componentDidMount() {
        this.int();
    }
    delete(userId) {
        fetch('http://localhost:8080/demo_war/UserInfo/User?userId='+userId, {
            method: 'DELETE',
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
                alert("删除成功！");
                this.int();
            } else {
                alert("删除失败！");
            }
        });
    };
    int() {
        fetch('http://localhost:8080/demo_war/UserInfo/Users', {
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
                    data: data.data
                });
            } else {
            }
        });
    };
    render() {
        return (
            <div>
                <div className={Styles.header}>
                    <div className={Styles.title}>用户列表</div>
                    <div><Link to="/item2/page2">新增用户</Link></div>
                </div>
                <Table className={Styles.table} columns={this.columns} dataSource={this.state.data} rowKey={row=>row.userId} />
            </div>)
    }
}
export default page3;