import React, { Component } from 'react';
import {BrowserRouter as Router,Route} from 'react-router-dom';
import Tellusaboutyou from './components/tellusaboutyou';

class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
       <Router>
          <Route path='/tell-us-about-you' component={Tellusaboutyou}></Route>
      </Router>  
    );
  }
}

export default App;
