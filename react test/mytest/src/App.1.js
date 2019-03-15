import React, { Component } from 'react';
import {BrowserRouter as Router,Route} from 'react-router-dom';
import Tellusaboutyou from './components/tellusaboutyou';
import ClaimStatusReport from './components/claimStatusReport';
import Content from './components/content';
import Home from './components/home';
import claimStatusReport from './components/claimStatusReport';

const ClaimStatusReportWithRouter = ({ match }) => (
  <div>
    <ClaimStatusReport></ClaimStatusReport>
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
            <Content>
              <Route exact path='/' component={Home}></Route>
              <Route path='/home' component={Home}></Route>
              <Route match='match' path='/claimstatusreport' component={ClaimStatusReportWithRouter}></Route>
              <Route path='/tellusaboutyou' component={Tellusaboutyou}></Route>
            </Content>
      </Router> 
    );
  }
}

export default App;
