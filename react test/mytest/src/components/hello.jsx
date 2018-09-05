import React, { Component } from 'react';
import StateTest from './stateTest'
class hello extends Component{
    render(){
        return(
        <div>Hello Word
        <StateTest text='Time is :'></StateTest>
        </div>);
    }
}
export default hello;