import React, { Component } from 'react';
import {BrowserRouter as Router,Route} from 'react-router-dom';
import Home from './components/home';
import Tellusaboutyou from './components/tellusaboutyou';
import Bundle from './Bundle';

const Hello = (props) => (
  <Bundle load={() => import('./components/hello')}>
  { (Child) => <Child {...props}></Child>}
  </Bundle>
);
const Post = (props) => (
  <Bundle load={() => import('./components/post')}>
  { (Child) => <Child {...props}></Child>}
  </Bundle>
);
const StateTest = (props) => (
  <Bundle load={() => import('./components/stateTest')}>
  { (Child) => <Child {...props}></Child>}
  </Bundle>
);
const DraftBox = (props) => (
  <Bundle load={() => import('./components/draftBox')}>
  { (Child) => <Child {...props}></Child>}
  </Bundle>
);
class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
      <Router>
        <Home path='/home' component={Home}>
          <Route path='/hello' component={Hello}></Route>
          <Route path='/post' component={Post}></Route>
          <Route path='/tell-us-about-you' component={Tellusaboutyou}></Route>
          <Route path='/stateTest' component={StateTest}></Route>
          <Route path='/draftBox' component={DraftBox}></Route>
        </Home>
      </Router> 
    );
  }
}

export default App;
