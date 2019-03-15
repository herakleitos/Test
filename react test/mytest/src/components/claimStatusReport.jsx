import React, { Component } from 'react'
import Styles from './claimStatusReport.css'
import {BrowserRouter as Router,Route,Link} from 'react-router-dom';
import Home from './home';
class claimStatusReport extends Component {
    constructor(props) {
        super(props);
    }
    render() {
        return (
            <div>
                <div className={Styles.imageContainer}>
                    <div className={Styles.leftItem} >
                        <span className={Styles.mainTitle}>Claim Status Report</span>
                        <br />
                        <span className={Styles.description}>08 Dec 2018</span>
                    </div>
                    <div className={Styles.rightItem}><img src={require('../img/calm100_logo.png')} /></div>
                </div>
                <div className={Styles.split}>
                </div>
                <br />
                <br />
                <div>
                    <table className={Styles.table}>
                        <thead>
                            <tr className={Styles.tableHeader}>
                                <th>
                                    Name/ID
                                </th>
                                <th>
                                    Date
                                </th>
                                <th>
                                    Injury Description
                                </th>
                                <th>
                                    Claim Status
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <span className={Styles.name}>Ken Adams</span>
                                    <br />
                                    <span className={Styles.id}>123456</span>
                                </td>
                                <td>
                                    <span>01-23-19</span>
                                </td>
                                <td>
                                    <span>Stubbed Toe</span>
                                </td>
                                <td className={Styles.status}>
                                    <div className={Styles.div1}>
                                        Accepted Claim Medical only
                                </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span className={Styles.name}>Regina Felangi</span>
                                    <br />
                                    <span className={Styles.id}>654321</span>
                                </td>
                                <td>
                                    <span>02-14-19</span>
                                </td>
                                <td>
                                    <span>Broken Heart</span>
                                </td>
                                <td className={Styles.status}>
                                    <div className={Styles.div2}>
                                        Pending Acceptance
                                </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <span className={Styles.name}>John Lennon</span>
                                    <br />
                                    <span className={Styles.id}>357913</span>
                                </td>
                                <td>
                                    <span>03-06-19</span>
                                </td>
                                <td>
                                    <span>Injured leg</span>
                                </td>
                                <td className={Styles.status}>
                                    <div className={Styles.div3}>
                                    Information pending
                                </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        )
    }
}

export default claimStatusReport;