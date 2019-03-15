import React, { Component } from 'react'
import { Route, Link } from 'react-router-dom';
import Tellusaboutyou from './tellusaboutyou';
import ClaimStatusReport from './claimStatusReport';

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
                </ul> : null
        }
    </div>);
});
class home extends Component {
    render() {
        return (<div>
            <ul>
                <li>
                    <Link to="/item1" style={{ color: 'black' }}>111
                    </Link>
                    <Route path="/item1" component={item1}></Route>
                </li>
                <li>
                    <Link to="/item2" style={{ color: 'black' }}>222
                    </Link>
                    <Route path="/item2" component={item2}></Route>
                </li>
            </ul>
            <Route path="/item2/tellusaboutyou" render={props=>(<Tellusaboutyou {...props}></Tellusaboutyou>)}></Route>
            <Route path="/item1/claimstatusreport" render={props=>(<ClaimStatusReport {...props}></ClaimStatusReport>)}></Route>
        </div>);
    }
}
export default home;