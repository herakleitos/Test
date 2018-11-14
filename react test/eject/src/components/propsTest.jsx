import React,{Component} from 'react'
class propsTest extends Component
{
    constructor(props){
        super(props);
        this.state={
            text : this.props.text?'':this.props.text
        };
    }
    componentWillReceiveProps(nextProps){
        const t = this.state.text;
        const newt= nextProps.text;
        if(t!==newt)
        {
            this.setState({
                text : newt
            })
        }
        console.log('componentWillReceiveProps '+this.state.text);
    }
    componentDidUpdate(){
        console.log('componentDidUpdate '+this.state.text);
    }
    componentWillMount(){
        console.log('componentWillMount '+this.state.text);
    }
    shouldComponentUpdate(props){
        console.log('shouldComponentUpdate '+this.state.text);
        return true;
    }
    render(){
        const style={
            width:'100px'
        };
        return (<div><div>{this.state.text}</div><div><input type='button' value='Get time' onClick={this.props.ButtonClick} style={style}/></div></div>);
    }
}
export default propsTest;