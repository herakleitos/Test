import React,{Component} from 'react'
class requestTest extends Component{
    constructor(props){
        super(props);
        this.state={
            text:'',
            json:''
        }
    }
    getData(){
        fetch('http://127.0.0.1:8081',{method:'GET'}).then(rest=>rest.text()).then(
            data=>{
                this.setState({
                    text:data
                });
            }
        );
        fetch('http://127.0.0.1:8081/json',{method:'GET'}).then(rest=>rest.json()).then(
            data=>{
                this.setState({
                    json:data
                });
            }
        );
    }
    componentWillMount(){
        this.getData();
    }
    render(){
        return(<div>
            <div>{this.state.json.name}</div>
            <div>{this.state.json.age}</div>
            <div>{this.state.json.sex}</div>
            <div>{this.state.text}</div>
            </div>)
    }
}
export default requestTest