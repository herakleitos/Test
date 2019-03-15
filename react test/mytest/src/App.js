import React, { Component } from 'react';
import {HashRouter as Router,Switch} from 'react-router-dom';
import Home from './components/home';

class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
      <Router>
          <Home path='/home' component={Home}>
          </Home>
      </Router> 
    );
  }
}

export default App;
