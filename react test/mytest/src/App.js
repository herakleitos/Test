import React, { Component } from 'react';
import {HashRouter as Router,Switch} from 'react-router-dom';
import Home from './components/home';
import Page from './components/home'
class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
      <Router>
          <Page path='/home' component={Page}>
          </Page>
      </Router> 
    );
  }
}

export default App;
