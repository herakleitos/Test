import React ,{Component} from 'react'
import { Button, Input,Radio,DatePicker,Checkbox} from 'antd';
import Styles from './tellme.css'
import moment from 'moment';
class tellusaboutyou extends Component{
    constructor(props){
        super(props);
        this.firstClick=this.firstClick.bind(this);
        this.secondClick=this.secondClick.bind(this);
        this.goBack=this.goBack.bind(this);
        this.changeSex=this.changeSex.bind(this);
        this.pCodeChange=this.pCodeChange.bind(this);
        this.phoneChange=this.phoneChange.bind(this);
        this.changePlan=this.changePlan.bind(this);
        this.ageChange=this.ageChange.bind(this);
        this.submit=this.submit.bind(this);
        this.state =  {
            showFirstPage:true,
            showSecondPage:false, 
            pCode:'',
            phone:'',
            sex:'',
            age:null,
            plans:[
                {
                    value:'Calm100 Basic',
                    checked:false
                },
                {
                    value:'Calm100 Plus',
                    checked:false
                },
                {
                    value:'Calm100 Complete',
                    checked:false
                },
            ],
        }
    }
    firstClick(){
        console.log('firstClick');
        this.setState({
            showFirstPage:false,
            showSecondPage:true,
        });
    }
    secondClick(){
        console.log('secondClick');
        this.setState({
            showFirstPage:false,
            showSecondPage:false,
        });
    }
    goBack(index){
        console.log('goback',index);
        if(index===1){
            this.setState({
                showFirstPage:true,
                showSecondPage:false,
            });
        }
        if(index===2){
            this.setState({
                showFirstPage:false,
                showSecondPage:true,
            });
        }
    }
    changeSex(sex){
        console.log('changeSex');
        this.setState({
            sex:sex,
        });
    }
    pCodeChange(e){
        console.log('pCodeChange',e.target.value);
        this.setState({
            pCode:e.target.value,
        });
    }
    phoneChange(e){
        console.log('phoneChange',e.target.value);
        this.setState({
            phone:e.target.value,
        });
    }
    changePlan(e){
        console.log('changePlan',e.target.value,e.target.checked);
        this.setState({
            plans:this.state.plans.map(item=>{
                if(item.value!==e.target.value)return item;
                if(item.value===e.target.value){
                    return {value:item.value,checked:e.target.checked};
                }
            }),
        });
    }
    ageChange(moment, dateString){
        console.log('ageChange',dateString);
        this.setState({
            age:dateString,
        });
    }
    submit(){
        console.log(this.state);
    }
    render(){
        return(
            <div>
                <div className={Styles.imageContainer}>
                    <div className={Styles.leftItem} ><img src='../img/calm100_logo.png' /></div>
                    <div className={Styles.rightItem}><img src='../img/calm100_leaf.png' /></div>
                </div>
                {
                    (this.state.showFirstPage||this.state.showSecondPage)&&
                    <div>
                    <div className={Styles.mainTitle}>Tell us about you</div>
                    <div className={Styles.description}>Shopping for dental and vision insurance is easier than you might think. To get started, simply complete the following:</div>
                    <br></br>
                    </div>
                }
                {
                    this.state.showFirstPage&&
                    <div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.narrowTitle}>Postal Code</span>
                        <Input className={Styles.input} value={this.state.pCode} onChange={this.pCodeChange}></Input>
                    </div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.narrowTitle}>Gender</span>
                        <Radio onChange={() => this.changeSex('M')} value='M' checked={this.state.sex==='M'} >Malle</Radio>
                        <Radio onChange={() => this.changeSex('F')} value='F' checked={this.state.sex==='F'}>Female</Radio>
                    </div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.narrowTitle}>Phone</span>
                        <Input className={Styles.input} onChange={this.phoneChange} value={this.state.phone}></Input>
                    </div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.narrowTitle}>Birth date</span>
                        <DatePicker className={Styles.datePicker} format='YYYY-MM-DD' value={this.state.age?new moment(this.state.age):null} onChange={this.ageChange}></DatePicker>
                    </div>
                    <br></br>
                    <div className={Styles.lineContainer}>
                        <Button className={Styles.button} key='button1' onClick={() => this.firstClick()}>Next Step: Select a plan</Button>
                    </div>
                    </div>
                }
                {
                    this.state.showSecondPage&&
                    <div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.title}>Calm100 Basic</span>
                        <Checkbox value='Calm100 Basic' onChange={this.changePlan} 
                        checked={this.state.plans?this.state.plans.filter(item=>item.value==='Calm100 Basic')[0].checked:false} ></Checkbox>
                    </div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.title}>Calm100 Plus</span>
                        <Checkbox value='Calm100 Plus' onChange={this.changePlan} 
                        checked={this.state.plans?this.state.plans.filter(item=>item.value==='Calm100 Plus')[0].checked:false} ></Checkbox>
                    </div>
                    <div className={Styles.lineContainer}>
                        <span className={Styles.title}>Calm100 Complete</span>
                        <Checkbox value='Calm100 Complete' onChange={this.changePlan} 
                        checked={this.state.plans?this.state.plans.filter(item=>item.value==='Calm100 Complete')[0].checked:false} ></Checkbox>
                    </div>
                    <br></br>
                    <div className={Styles.lineContainer}>
                        <Button className={Styles.button}  key='button2' onClick={() => this.secondClick()}>Next Step</Button>
                        <Button className={Styles.button} key='button3' onClick={() => this.goBack(1)}>Go Back</Button>
                    </div>
                    </div>
                }
                {
                    !this.state.showFirstPage&&!this.state.showSecondPage&&
                    <div className={Styles.mainContent}>
                        <div className={Styles.description} >Thanks so much for your submission, one of our representitives will be in contact with you shortly to gather more information.</div>
                        <br></br>
                   {/*      <Button className={Styles.button} key='button5' onClick={()=>this.submit()}>Console Log</Button> */}
                        <Button className={Styles.button} key='button4' onClick={()=>this.goBack(2)}>Go Back</Button>
                    </div>
                }
            </div>
        )
    }
}

export default tellusaboutyou;