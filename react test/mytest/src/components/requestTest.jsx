import React,{Component} from 'react'
class requestTest extends Component{
    constructor(props){
        super(props);
        this.state={
            text:''
        }
    }
    getData(){
        fetch('http://127.0.0.1:8081',{method:'GET'}).then(rest=>rest.text()).then(
            data=>{
                this.setState({
                    text:data
                });
            }
        )
    }
    componentWillMount(){
        this.getData();
    }
    render(){
        return(<div>{this.state.text}</div>)
    }
}
export default requestTest