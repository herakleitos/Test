import React, { Component } from 'react'
import { Link } from 'react-router-dom'
class home extends Component {
    constructor(props) {
        super(props)
    }
    render() {
        let styleP = {
            width: '600px',
            height: '250px',
            backgroundColor: '#C0C0C0',
            fontSize: '16px',
            textAlign: 'left'
        }
        let styleC0 = {
            width: '600px',
            height: '25px',
            backgroundColor: '#0000FF',
            fontSize: '16px',
            textAlign: 'left'
        }
        let styleC1 = {
            fontSize: '16px',
            textAlign: 'left',
            color:'#FFFFFF'
        }
        return (<div style={styleP}>
            <li>this.props.location.search ---- {this.props.location.search}</li>
            <li>window.location.pathname ---- {window.location.pathname}</li>
            <li>window.location.href ---- {window.location.href}</li>
            <li>window.location.port ---- {window.location.port}</li>
            <li>window.location.protocol ---- {window.location.protocol}</li>
            <li>window.location.host ---- {window.location.host}</li>
            <li>window.location.search ---- {window.location.search}</li>
            <div style={styleC0}>
                <Link to="/hello"  style={{ color: 'black' }}>
                    <div style={styleC1}>点击跳转到hello</div>
                </Link>
                <Link to="/requesttest" style={{ color: 'black' }}>
                    <div style={styleC1}>点击跳转到requesttest</div>
                </Link>
            </div>
        </div>);
    }
}
export default home;