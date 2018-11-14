import React,{Component} from 'react';
import PropsTest from './propsTest'
import {Modal, Button} from 'antd'
class stateTest extends Component
{
    constructor(props){
        super(props);
        this.state={
            text:'',
            visible:false,
        }
        this.onButtonClick=this.onButtonClick.bind(this);
        this.showModal=this.showModal.bind(this);
        this.handleCancel=this.handleCancel.bind(this);
    }
    onButtonClick(){
        this.setState({
            text : new Date().toString(),
        })
    }
    showModal() {
        this.setState({
          visible: true
        });
      }
    handleCancel() {
        this.setState({
          visible: false
        });
    }
    render(){
        return(
        <div>Please Click To Get Current Time
            <Button type="primary" onClick={this.showModal}>显示对话框</Button>
            <Modal title='Test'
            visible={this.state.visible}
            onCancel={this.handleCancel}>
                <PropsTest text={this.state.text} ButtonClick={this.onButtonClick}></PropsTest>
            </Modal>
        </div>);
    }
}
export default stateTest;