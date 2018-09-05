import React,{Component} from 'react';
import PropsTest from './propsTest'
class stateTest extends Component
{
    constructor(props){
        super(props);
        this.state={
            text:'Null',
        }
        this.onButtonClick=this.onButtonClick.bind(this);
    }
    onButtonClick(){
        this.setState({
            text : new Date().toString(),
        })
    }
    render(){
        return(
        <div>Please Click To Get Current Time
            <PropsTest text={this.state.text} ButtonClick={this.onButtonClick}></PropsTest>
        </div>);
    }
}
export default stateTest;