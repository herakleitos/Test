import React, { Component } from 'react';
import { Modal, Button, Input } from 'antd';
import Styles from './comm.css';
import DraftStyles from './draft.css'
import {
    CompositeDecorator,
    Editor,
    EditorState,
    convertToRaw,
    RichUtils
} from 'draft-js';
import { debug } from 'util';
const styleMap = {
    'RED': {
        color: 'red'
    }
}
class draftBox extends Component {
    constructor(props) {
        super(props);
        const decorator = new CompositeDecorator([
            {
                strategy: findLinkEntities,
                component: Link
            }
        ]);
        this.state = {
            editorStates: [
                EditorState.createEmpty(decorator),
                EditorState.createEmpty(decorator),
            ],
            showModal: false,
            url: ''
        };
        this.currentEditor = 0;
        console.log('init', this.state);
        this.addLink = this.addLink.bind(this);
        this.showModal = this.showModal.bind(this);
        this.handleCancel = this.handleCancel.bind(this);
        this.urlChange = this.urlChange.bind(this);
        this.toggleInlineStyle = this.toggleInlineStyle.bind(this);
        this.toggleBlockType = this.toggleBlockType.bind(this);
        this.onChange = this.onChange.bind(this);
        this.editorSelected = this.editorSelected.bind(this);
        this.showLog = this.showLog.bind(this);
        this.removeLink = this.removeLink.bind(this);
    }
    onChange(editorState) {
        console.log('onChange',this.currentEditor);
        this.setState({
            editorStates: this.state.editorStates.map((item, index) => {
                if (index === this.currentEditor) {
                    return editorState;
                } else {
                    return item;
                }
            }),
        })
    };
    editorSelected(e) {
        console.log('editorSelected',e.target);
        const index = e.target.parentNode.parentNode.parentNode.getAttribute('index')?
        parseInt(e.target.parentNode.parentNode.parentNode.getAttribute('index')):0;
        this.currentEditor = index;
    }
    /**
     * 添加链接
     */
    addLink() {
        this.setState({
            showModal: false,
        });
        //debugger
        const editorState = this.state.editorStates[this.currentEditor];
        console.log(this.state.editorStates);
        console.log(this.currentEditor);
        const url = this.state.url;
        // 获取contentState
        const contentState = editorState.getCurrentContent();
        // 在contentState上新建entity
        const contentStateWithEntity = contentState.createEntity(
            'LINK',
            // 'MUTABLE',
            // 'IMMUTABLE',
            'SEGMENTED',
            { url }
        );
        // 获取到刚才新建的entity
        const entityKey = contentStateWithEntity.getLastCreatedEntityKey();
        // 把带有entity的contentState设置到editorState上
        const newEditorState = EditorState.set(editorState, { currentContent: contentStateWithEntity });
        const newState = this.state.editorStates.map((item, index) => {
            if (index === this.currentEditor) {
                return RichUtils.toggleLink(
                    newEditorState,
                    newEditorState.getSelection(),
                    entityKey)
            } else {
                return item;
            }
        });
        // 把entity和选中的内容对应
        this.setState({
            editorStates: newState,
            showModal: false,
            url: '',
        }, () => {
            setTimeout(() => this.currentEditor===0 ? this.refs.draftBox0.focus() : this.refs.draftBox1.focus(), 0);
        });
    }
    //删除链接
    removeLink(e) {
        e.preventDefault();
        //debugger
        const editorState = this.state.editorStates[this.currentEditor];
        const selection = editorState.getSelection();
        const newState = this.state.editorStates.map((item, index) => {
            if (index === this.currentEditor) {
                return RichUtils.toggleLink(editorState, selection, null)
            } else {
                return item;
            }
        });
        // 疑问:如果选中的是部分链接内容会怎样?
        if (!selection.isCollapsed()) {
            this.setState({
                // 借助 RichUtils 删除链接
                editorStates: newState,
            });
        }
    }
    showLog(){
        console.log('state0',convertToRaw(this.state.editorStates[0].getCurrentContent()));
        console.log('state1',convertToRaw(this.state.editorStates[1].getCurrentContent()));
    }
    /**
     * 展示弹窗
     */
    showModal() {
        this.setState({
            showModal: true
        })
    }

    /**
     * 取消动作
     */
    handleCancel = () => {
        console.log('Clicked cancel button');
        this.setState({
            showModal: false,
        });
    }

    /**
     * 链接改变
     *
     * @param {Object} event 事件
     */
    urlChange(event) {
        const target = event.target;
        this.setState({
            url: target.value
        });
    }
    toggleInlineStyle(inlineStyle) {
         this.onChange(
            RichUtils.toggleInlineStyle(
                this.state.editorStates[this.currentEditor],
                inlineStyle
            )
        );
    }
    toggleBlockType(blockType) {
         this.onChange(
            RichUtils.toggleBlockType(
                this.state.editorStates[this.currentEditor],
                blockType
            )
        );
    }
    render() {
        return (
            <div  className={Styles.richTexts}>
                <Button onClick={this.showModal}>LINK</Button>
                <Button onClick={this.removeLink}>REMOVE LINK</Button>
                <Button onClick={() => this.toggleInlineStyle('UNDERLINE')}>UNDERLINE</Button>
                <Button onClick={() => this.toggleInlineStyle('BOLD')}>Bold</Button>
                <Button onClick={() => this.toggleInlineStyle('RED')}>RED</Button>
                <Button onClick={() => this.toggleBlockType('header-one')}>H1</Button>
                <Button onClick={() => this.toggleBlockType('unordered-list-item')}>UL</Button>
                <Button onClick={() => this.toggleBlockType('ordered-list-item')}>OL</Button>
                <Button onClick={() => this.toggleBlockType('blockquote')}>BLOCKQUOTE</Button>
                <div key='editorContainer0' 
                    index='0'
                    className={Styles.draftBox}>
                    <Editor
                        key='editor0'
                        customStyleMap={styleMap}
                        blockStyleFn={getBlockStyle}
                        editorState={this.state.editorStates[0]}
                        onChange={this.onChange}
                        onFocus={this.editorSelected}
                        ref='draftBox0' />
                </div>
                <br></br>
                <div key='editorContainer1'
                    index='1'
                    className={Styles.draftBox}>
                    <Editor
                        key='editor1'
                        customStyleMap={styleMap}
                        blockStyleFn={getBlockStyle}
                        editorState={this.state.editorStates[1]}
                        onFocus={this.editorSelected}
                        onChange={this.onChange}
                        ref='draftBox1' />
                </div>
                <Modal title="Title"
                    visible={this.state.showModal}
                    onOk={this.addLink}
                    onCancel={this.handleCancel}
                >
                    <Input value={this.state.url} onChange={this.urlChange}></Input>
                </Modal>
                <Button onClick={() => this.showLog()}>ConsoleLog</Button>
            </div>
        )
    }
}
function findLinkEntities(contentBlock, callback, contentState) {
    contentBlock.findEntityRanges(
        (character) => {
            const entityKey = character.getEntity();
            return (
                entityKey !== null &&
                contentState.getEntity(entityKey).getType() === 'LINK'
            );
        },
        function () {
            console.log(arguments);
            callback(...arguments);
        }

    );
}
function getBlockStyle(block) {
    switch (block.getType()) {
        case 'blockquote': return DraftStyles.blockquote;
        default: return null;
    }
}
const Link = (props) => {
    const { url } = props.contentState.getEntity(props.entityKey).getData();
    return (
        <a href={url}>
            {props.children}
        </a>
    );
};

export default draftBox;