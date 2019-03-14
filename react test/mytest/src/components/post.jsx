import React, { Component } from 'react'
import Styles from './comm.css'
class post extends Component {
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
    render(){
        return(<div>
            <span>latitude </span>
            <input className={Styles.input} onChange={this.text1Change}></input>
            <span>longitude</span>
            <input className={Styles.input} onChange={this.text2Change}></input>
            <button className={Styles.button} onClick={() => this.buttonClick()}>Post</button>
            </div >);
    }
}
export default post;