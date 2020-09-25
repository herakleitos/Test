import React, { Component } from 'react'
import { Route, Link, Redirect } from 'react-router-dom';
import Page1 from './page1';
import Page2 from './page2';
import Page3 from './page3';
import Page4 from './page4';
import Styles from './comm.css';

//两种不同的路由写法
const item1 = (({ match }) => {
    return (<div>
        <Link to="/item1" style={{ color: 'black' }}>用户登录
        </Link>
        {
            match ?
                <ul>
                    <Route path="/item1" children={subMatch => (
                        <li>
                            <Link to={`${match.url}/page1`} style={{ color: 'black' }}>用户登录
                            </Link>
                        </li>
                    )}></Route>
                </ul> : null
        }
    </div>);
});

//两种不同的路由写法
const item2 = (({ match }) => {
    return (<div>
        {
            match ?
                <ul>
                    <li>
                        <Link to={`${match.url}/page3`} style={{ color: 'black' }}>用户列表
                        </Link>
                    </li>
                    <li>
                        <Link to={`${match.url}/page2`} style={{ color: 'black' }}>新增用户
                        </Link>
                    </li>
                    <li>
                        <Link to={`${match.url}/page4`} style={{ color: 'black' }}>修改用户
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
                        <Route path="/item1" component={item1}></Route>
                    </li>
                    <li>
                        <Link to="/item2/page3" style={{ color: 'black' }}>用户维护
                        </Link>
                        <Route path="/item2" children={item2}></Route>
                    </li>
                </ul>
            </div>
            <div className={Styles.content}>
                <Route path="/item1/page1" render={props => (<Page1 {...props}></Page1>)}></Route>
                <Route path="/item2/page2" render={props => (<Page2 {...props}></Page2>)}></Route>
                <Route path="/item2/page3" render={props => (<Page3 {...props}></Page3>)}></Route>
                <Route path="/item2/page4" render={props => (<Page4 {...props}></Page4>)}></Route>
            </div>
        </div>);
    }
}
export default home;