import React, { Component } from 'react';
import {BrowserRouter as Router,Route} from 'react-router-dom'
import Hello from './components/hello'
import Home from './components/home'
import StateTest from './components/stateTest'
import DraftBox from './components/draftBox'
class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
      <Router>
        <Home path='/home' component={Home}>
          <Route path='/hello' component={Hello}></Route>
          <Route path='/stateTest' component={StateTest}></Route>
          <Route path='/draftBox' component={DraftBox}></Route>
        </Home>
      </Router> 
    );
  }
}

export default App;
