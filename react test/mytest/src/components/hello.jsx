import React, { Component } from 'react';
import StateTest from './stateTest';
import Styles from './comm.css';
import ReactTooltip from 'react-tooltip'
class hello extends Component {
    render() {
        return (
            <div>Hello Word
            <p data-for='aa' data-multiline scrollHide={false} data-tip="hello thank you">Tooltip</p>
           <span>text area1</span>
                <div  data-tip='aaaaaaaa<br></br>aaaaaaaaaaaaaaa'>
                    <span>text area2</span>
                    <div className={Styles.inputArea} contentEditable ></div>
                </div>
                <StateTest text='Time is :'></StateTest>
                <ReactTooltip  offset="{'top': 10, 'left': 10}"   id='aa' />
            </div>);
    }
}
export default hello;