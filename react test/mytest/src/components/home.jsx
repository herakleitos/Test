import React, { Component } from 'react'
import { Route, Link,Redirect } from 'react-router-dom';
import Tellusaboutyou from './tellusaboutyou';
import Page1 from './page1';
import Page2 from './page2';
import Page3 from './page3';
import Page4 from './page4';
import ClaimStatusReport from './claimStatusReport';
import Styles from './comm.css';
const item1 = (({ match }) => {
    debugger
    return (<div>
        {
            match ?
                <ul>
                    <li>
                        <Link to={`${match.url}/claimstatusreport`} style={{ color: 'black' }}>claimstatusreport
                        </Link>
                    </li>
                    <li>
                        <Link to={`${match.url}/page1`} style={{ color: 'black' }}>page1
                        </Link>
                    </li>
                    <li>
                        <Link to={`${match.url}/page2`} style={{ color: 'black' }}>page2
                        </Link>
                    </li>
                </ul> : null
        }
    </div>);
});
const item2 = (({ match }) => {
    debugger
    return (<div>
        {
            match ?
                <ul>
                    <li>
                        <Link to={`${match.url}/tellusaboutyou`} style={{ color: 'black' }}>tellusaboutyou
                        </Link>
                    </li>
                    <li>
                        <Link to={`${match.url}/page3`} style={{ color: 'black' }}>page3
                        </Link>
                    </li>
                    <li>
                        <Link to={`${match.url}/page4`} style={{ color: 'black' }}>page4
                        </Link>
                    </li>
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
                <Route path="/item1/page1" render={props => (<Page1 {...props}></Page1>)}></Route>
                <Route path="/item1/page2" render={props => (<Page2 {...props}></Page2>)}></Route>
                <Route path="/item2/page3" render={props => (<Page3 {...props}></Page3>)}></Route>
                <Route path="/item2/page4" render={props => (<Page4 {...props}></Page4>)}></Route>
                <Route path="/item2/tellusaboutyou" render={props => (<Tellusaboutyou {...props}></Tellusaboutyou>)}></Route>
                <Route path="/item1/claimstatusreport" render={props => (<ClaimStatusReport {...props}></ClaimStatusReport>)}></Route>
            </div>
        </div>);
    }
}
export default home;