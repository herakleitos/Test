import React, { Component } from 'react';
import {BrowserRouter as Router,Route,Link} from 'react-router-dom';
import Tellusaboutyou from './components/tellusaboutyou';
import ClaimStatusReport from './components/claimStatusReport';
import Content from './components/content';
import Home from './components/home';

const Menus = ({ match }) => (
  <div>
    {
      match ?
      <ul>
        <li>aaaaaa</li>
        <li>bbbbbb</li>
        <li>cccccc</li>
        <li>dddddd</li>
      </ul> : null
    }
    <Route path={`${match.url}/test`} component={Home}/>
  </div>
) 

class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
      <Router>
           <ul>
             <li>
                <Link to="/home"  style={{ color: 'black' }}>home
                </Link>
              <Route exact path='/' component={Home}></Route>
              <Route path='/home' component={Home}></Route>
             </li>
             <li>
                <Link to="/menus1"  style={{ color: 'black' }}>menus
                </Link>
                <Route match='match' path='/menus1' component={Menus}></Route>
             </li>
             <li>
                <Link to="/tellusaboutyou"  style={{ color: 'black' }}>tell us about you
                </Link>
                <Route path='/tellusaboutyou' component={Tellusaboutyou}></Route>
             </li>
           </ul>
      </Router> 
    );
  }
}

export default App;
