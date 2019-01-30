import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import Styles from './comm.css'
class home extends Component {
    constructor(props) {
        super(props)
        this.buttonClick = this.buttonClick.bind(this);
        this.state ={
            text1:'0',
            text2:'0',
        }
        this.text1Change= this.text1Change.bind(this);
        this.text2Change= this.text2Change.bind(this);
    }
    buttonClick(){
        debugger
        let postData = {
            Visitor:{
                    Longitude:parseFloat(this.state.text2),
                    Latitude:parseFloat(this.state.text1)
                }
            };
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
    text1Change(event){
        this.setState({
            text1: event.target.value,
        });
    }   
    text2Change(event){
        this.setState({
            text2: event.target.value,
        });
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
                <span>latitude</span>
                <input className={Styles.input} onChange={this.text1Change}></input>
                <span>longitude</span>
                <input className={Styles.input} onChange={this.text2Change}></input>
                <button className={Styles.button} onClick={() => this.buttonClick()}>Post</button>
                <div style={{ marginTop: '1.5em',marginLeft:'140px' }}>{this.props.children}</div>
        </div>);
    }
}
export default home;