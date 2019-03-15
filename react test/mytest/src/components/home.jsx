import React, { Component } from 'react'
import { Route, Link } from 'react-router-dom';
import Tellusaboutyou from './tellusaboutyou';
import ClaimStatusReport from './claimStatusReport';
import Styles from './comm.css';
const item1 = (({ match }) => {
    debugger
    return (<div>
        {
            match ?
                <ul>
                    <li>
                        <Link to={`/item1/claimstatusreport`} style={{ color: 'black' }}>claimstatusreport
                        </Link>
                    </li>
                    <li><a href='http://www.baidu.com' target="_blank">11111</a></li>
                    <li><a href='http://www.baidu.com' target="_blank">22222</a></li>
                    <li><a href='http://www.baidu.com' target="_blank">33333</a></li>
                    <li><a href='http://www.baidu.com' target="_blank">44444</a></li>
                </ul> : null
        }
    </div>);
});
const item2 = (({ match }) => {
    return (<div>
        {
            match ?
                <ul>
                    <li>
                        <Link to={`/item2/tellusaboutyou`} style={{ color: 'black' }}>tellusaboutyou
                        </Link>
                    </li>
                    <li><a href='http://www.baidu.com' target="_blank">aaaaa</a></li>
                    <li><a href='http://www.baidu.com' target="_blank">bbbbb</a></li>
                    <li><a href='http://www.baidu.com' target="_blank">ccccc</a></li>
                    <li><a href='http://www.baidu.com' target="_blank">ddddd</a></li>
                </ul> : null
        }
    </div>);
});
class home extends Component {
    render() {
        return (<div>
            <div className={Styles.menu}>
                <ul>
                    <li>
                        <Link to="/item1/claimstatusreport" style={{ color: 'black' }}>item1
                    </Link>
                        <Route path="/item1" component={item1}></Route>
                    </li>
                    <li>
                        <Link to="/item2/tellusaboutyou" style={{ color: 'black' }}>item2
                    </Link>
                        <Route path="/item2" component={item2}></Route>
                    </li>
                </ul>
            </div>
            <div className={Styles.content}>
                <Route path="/item2/tellusaboutyou" render={props => (<Tellusaboutyou {...props}></Tellusaboutyou>)}></Route>
                <Route path="/item1/claimstatusreport" render={props => (<ClaimStatusReport {...props}></ClaimStatusReport>)}></Route>
            </div>
        </div>);
    }
}
export default home;