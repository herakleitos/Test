import React, { Component } from 'react';
import {BrowserRouter as Router,Route} from 'react-router-dom'
import Hello from './components/hello'
import Home from './components/home'
import PropsTest from './components/propsTest'
import RequestTest from './components/requestTest'
class App extends Component {
  render() {
    return (
      <Router>
        <div>
        <Route exact path='/' component={Home}></Route>
        <Route path='/hello' component={Hello}></Route>
        <Route path='/requesttest' component={RequestTest}></Route>
        </div>
      </Router>
    );
  }
}

export default App;
