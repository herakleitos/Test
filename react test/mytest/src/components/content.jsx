import React, { Component } from 'react'
import { Link } from 'react-router-dom'
class home extends Component {
    render() {
        return (<div>
                <div style={{ marginTop: '1.5em',marginLeft:'140px' }}>{this.props.children}</div>
        </div>);
    }
}
export default home;