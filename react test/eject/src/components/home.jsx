import React, { Component } from 'react'
import { Link } from 'react-router-dom'
class home extends Component {
    constructor(props) {
        super(props)
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
                <Link to="/stateTest" style={{ color: 'black' }}>
                stateTest
                </Link>  
                <br></br>   
                <Link to="/draftBox" style={{ color: 'black' }}>
                draftBox
                </Link>  
                <br></br>
                <div style={{ marginTop: '1.5em' }}>{this.props.children}</div>
        </div>);
    }
}
export default home;