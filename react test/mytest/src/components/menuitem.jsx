
import React, { Component } from 'react'
import { Link } from 'react-router-dom'
class menuitem extends Component {
    render() {
        return (
            <div>
                <h1>App</h1>
                <ul>
                    <li><Link to="/menus0">menus0</Link></li>
                    <li><Link to="/menus1">menus1</Link></li>
                </ul>
                {this.props.children}
            </div>);
    }
}
export default menuitem;