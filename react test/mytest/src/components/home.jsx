import React, { Component } from 'react'
import { Link } from 'react-router-dom'
class index extends Component {
    render() {
        return (<div>
                Hello, how are you?
                <div style={{ marginTop: '1.5em',marginLeft:'140px' }}>{this.props.children}</div>
        </div>);
    }
}
export default index;