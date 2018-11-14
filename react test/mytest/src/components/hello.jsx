import React, { Component } from 'react';
import Styles from './comm.css'
let itemArray = [];
class hello extends Component {
    constructor(props) {
        super(props),
            this.richEditAreaFocus = this.richEditAreaFocus.bind(this)
        this.richEditAreaBlur = this.richEditAreaBlur.bind(this)
        this.buttonClick = this.buttonClick.bind(this)
        this.edit = this.edit.bind(this)
        this.add = this.add.bind(this);
        this.state = {
            editList: [
                { id: '1', message: <span>this is a <a href='www.baidu.com'>test</a></span>, isEdit: false },
                { id: '2', message: '初始数据', isEdit: false },
                { id: '3', message: '初始数据', isEdit: false },
                { id: '4', message: '初始数据', isEdit: false },
            ],
        }
        this.state.editList.forEach(item=>{
            itemArray.push(item);
        });
        this.selection = document.getSelection();
        this.range =document.createRange();
        this.selectTextNode='';
    }
    richEditAreaFocus(id) {

        this.setState({
            editList: this.state.editList.map(f => {
                if (f.id === id) {
                    return {
                        ...f,
                        isEdit: true
                    }
                }
                return f;
            }),
        });
    }
    richEditAreaBlur(id) {
        this.setState({
            editList: this.state.editList.map(f => {
                if (f.id === id) {
                    return {
                        ...f,
                        isEdit: false
                    }
                }
                return f;
            }),
        });
    }
    edit(e, id) {
        this.setState({
            editList: this.state.editList.map(f => {
                if (f.id === id) {
                    return {
                        ...f,
                        message: e.target.innerHTML,
                    }
                }
                return f;
            }),
        });
    }
    add(){
        console.log('add before',this.state);
        let count = this.state.editList.length;
        var newItem = { id: (count+1).toString(), message: '初始数据', isEdit: false }
        console.log('add after',newItem);
        this.setState({
            editList: this.state.editList.concat(newItem)
        });
    }
    mouseDown(id){
/*         console.log('selection', this.selection);
        this.selectTextNode =this.selection.getRangeAt(0).startContainer;
        console.log('selection', this.selection.getRangeAt(0)); */
    }
    buttonClick() {
        this.props.history.push('home');
    }
    render() {
        return (
                <div className={Styles.container}>
                    <div className={Styles.head}>
                    </div>
                    <div className={Styles.content}>
                    {
                    this.state.editList.map(item => (
                        <div id={item.id} key={item.id}
                            className={
                                item.isEdit ? Styles.inputAreaEdit : Styles.inputArea
                            }
                            onFocus={() => this.richEditAreaFocus(item.id)}
                            onBlur={() => this.richEditAreaBlur(item.id)}
                            onInput={e => this.edit(e, item.id)}
                            onMouseDown={()=>this.mouseDown(item.id)}
                            contentEditable >
                            {item.message}
                        </div>
                    ), this)
                    }
                    </div>
                    <div className={Styles.bottom}>
                    <input type='button' onClick={this.add} value='Add'></input>
                    </div>
                </div>);
    }
}
export default hello;