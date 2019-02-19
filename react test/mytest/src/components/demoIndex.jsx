import React, { Component } from 'react'
import { Link } from 'react-router-dom'
import Styles from './comm.css'
class demoIndex extends Component {
    constructor(props) {
        super(props)
    }
    render() {
        let link = {
            fontSize: '26px',
            textAlign: 'left',
            color:'#FFFFFF'
        }
        return (
                <div style={{ marginTop: '1.5em',marginLeft:'140px' }}>{this.props.children}</div>
                );
    }
}
export default demoIndex;