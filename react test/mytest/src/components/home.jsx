import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import Styles from './comm.css'
class home extends Component {
    constructor(props) {
        super(props)
        this.buttonClick = this.buttonClick.bind(this);
    }
    buttonClick(){
        let postData = {a:'b'};
        fetch('http://localhost:51598/sendMesageBaseOnLocation.ashx', {
            method: 'POST',
            mode: 'cors',
            credentials: 'include',
            headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: JSON.stringify(postData)
            }).then(function(response) {
            console.log(response);
        })
    }   
    render() {
        let link = {
            fontSize: '26px',
            textAlign: 'left',
            color:'#FFFFFF'
        }
        return (<div>
                <Link to="/home"  style={{ color: 'black' }}>home
                </Link>
                <br></br>
                <Link to="/hello"  style={{ color: 'black' }}>hello
                </Link>
                <br></br>
                <Link to="/tell-us-about-you"  style={{ color: 'black' }}>tell us about you
                </Link>
                <br></br>
                <Link to="/stateTest" style={{ color: 'black' }}>
                stateTest
                </Link>  
                <br></br>   
                <Link to="/draftBox" style={{ color: 'black' }}>
                draftBox
                </Link>  
                <br></br>
                <button className={Styles.button} onClick={() => this.buttonClick()}>Post</button>
                <div style={{ marginTop: '1.5em',marginLeft:'140px' }}>{this.props.children}</div>
        </div>);
    }
}
export default home;