import React ,{Component} from 'react'
import {BrowserRouter as Router,Route} from 'react-router-dom';
import DemoIndex from './components/demoIndex';
import ClaimStatusReport from './components/claimStatusReport';
import TellUsAboutYou from './components/tellusaboutyou'
class App extends Component {
  constructor(props){
    super(props);
} 
  render() {
    return (
      <TellUsAboutYou path='/tell-us-about-you' component={TellUsAboutYou}>
      </TellUsAboutYou> 
    );
  }
}

export default App;