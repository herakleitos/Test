using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Sql.DataApi;
using Chaint.Instock.Core;
using Chaint.Common.Core;
using Chaint.Common.Core.Log;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Utils;
using Chaint.Common.Core.Enums;
using Chaint.Common.ServiceHelper;
using System.Data;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace Chaint.Instock.Business.PlugIns
{
    partial class StockInAutoScanPlugIn
    {
        public string MatchStockArea(string barCode)
        {
            bool isRoll = false;
            string paperType = "1";
            if (barCode.Substring(2, 1) == "1")
            {
                isRoll = true;
                paperType = "1";
            }
            else if (barCode.Substring(2, 1) == "2")
            {
                isRoll = false;
                paperType = "2";
            }
            string material =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FMaterial);
            //根据物料名称找到物料编码
            string materialNumber = GetMaterialNumber(material);
            var operatorField = this.View.GetControl<SearchLookUpEdit>(Const_StockInAutoScan.Head_Field_FOperator);
            string Operator = operatorField.Text;
            string grade =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FGrade);
            string specification =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FSpecification);
            decimal diameterOrSlides =
                this.View.GetValue<decimal>(Const_StockInAutoScan.Head_Field_FDiameterOrSlides);
            decimal coreDiameterOrReam =
                this.View.GetValue<decimal>(Const_StockInAutoScan.Head_Field_FCoreDiameterOrReam);
            decimal length =
               this.View.GetValue<decimal>(Const_StockInAutoScan.Head_Field_FLength);
            decimal planlength =
                this.View.GetValue<decimal>(Const_StockInAutoScan.Head_Field_FPlanLength);
            decimal planDiameter =
                this.View.GetValue<decimal>(Const_StockInAutoScan.Head_Field_FPlanDiameter);
            string spCustomer =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FSpCustomer);
            string color =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FColor);
            decimal weight =
                this.View.GetValue<decimal>(Const_StockInAutoScan.Head_Field_FWeight);
            string weightMode =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FWeightMode);
            string certification =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FCertification);
            string specProdName =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FSpecProdName);
            string specCustName =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FSpecCustName);
            string poNumber =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FPoNumber);
            string remark =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FRemark);
            string trademarkStyle =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FTrademarkStyle);
            string packType =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FPackType);
            string transportType =
                this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FTransportType);
            string productInfo
                 = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12} {13} {14} {15} {16}",
                                    specification, packType, grade, color, spCustomer, 
                                    coreDiameterOrReam==0? "":Convert.ToString(coreDiameterOrReam),
                                    diameterOrSlides == 0 ? "" : Convert.ToString(diameterOrSlides),
                                    planDiameter == 0 ? "" : Convert.ToString(planDiameter),
                                    planlength == 0 ? "" : Convert.ToString(planlength),
                                    weightMode,certification, specProdName,
                                    specCustName, poNumber, trademarkStyle, transportType, remark);
            string stockArea = string.Empty;
            string stock = string.Empty;
            string stockAreaPlanName = string.Empty;
            DataRow dataPlan = null;
            if (stockAreaPlan != null && stockAreaPlan.Rows.Count > 0)
            {
                foreach (DataRow plan in stockAreaPlan.Rows)
                {
                    string dbSpCustomer = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpCustomer);
                    if (!spCustomer.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!spCustomer.Equals(dbSpCustomer))
                            continue;
                    }
                    else if (spCustomer.IsNullOrEmptyOrWhiteSpace() && !dbSpCustomer.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbColor = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FColor);
                    if (!color.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!color.Equals(dbColor))
                            continue;
                    }
                    else if (color.IsNullOrEmptyOrWhiteSpace() && !dbColor.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string product = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FProduct);
                    if (!materialNumber.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!materialNumber.Equals(product))
                            continue;
                    }
                    else if (materialNumber.IsNullOrEmptyOrWhiteSpace() && !product.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbSpecification = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecification);
                    if (!specification.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!specification.Equals(dbSpecification))
                            continue;
                    }
                    else if (specification.IsNullOrEmptyOrWhiteSpace() && !dbSpecification.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbGrade = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FPaperGrade);
                    if (!grade.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!grade.Equals(dbGrade))
                            continue;
                    }
                    else if (grade.IsNullOrEmptyOrWhiteSpace() && !dbGrade.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    decimal dbCoreDiameterOrReam = plan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FCoreDiameterOrReam);
                    if (coreDiameterOrReam > 0)
                    {
                        if (dbCoreDiameterOrReam != coreDiameterOrReam)
                            continue;
                    }
                    else if (coreDiameterOrReam <= 0 && dbCoreDiameterOrReam > 0)
                    {
                        continue;
                    }
                    decimal dbDiameterOrSlides = plan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FDiameterOrSlides);
                    decimal dbLength =
                        plan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FLength);
                    if (isRoll)
                    {
                        if (planDiameter > 0)
                        {
                            if (dbDiameterOrSlides != planDiameter)
                                continue;
                        }
                        else if (planDiameter <= 0 && dbDiameterOrSlides > 0)
                        {
                            continue;
                        }
                        if (planlength > 0)
                        {
                            if (dbLength != planlength)
                                continue;
                        }
                        else if (planlength <= 0 && dbLength > 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (diameterOrSlides > 0 && diameterOrSlides != 500 && diameterOrSlides != 250)
                        {
                            if (dbDiameterOrSlides != diameterOrSlides)
                                continue;
                        }
                        else if (diameterOrSlides <= 0 && dbDiameterOrSlides > 0)
                        {
                            continue;
                        }
                    }
                    string dbWeightMode = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FWeightMode);
                    if (!weightMode.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!weightMode.Equals(dbWeightMode))
                            continue;
                    }
                    else if (weightMode.IsNullOrEmptyOrWhiteSpace() && !dbWeightMode.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbCertification = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FCertification);
                    if (!certification.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!certification.Equals(dbCertification))
                            continue;
                    }
                    else if (certification.IsNullOrEmptyOrWhiteSpace() && !dbCertification.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbSpecCustName = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecCustName);
                    if (!specCustName.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!specCustName.Equals(dbSpecCustName))
                            continue;
                    }
                    else if (specCustName.IsNullOrEmptyOrWhiteSpace() && !dbSpecCustName.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbSpecProdName = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecProdName);
                    if (!specProdName.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!specProdName.Equals(dbSpecProdName))
                            continue;
                    }
                    else if (specProdName.IsNullOrEmptyOrWhiteSpace() && !dbSpecProdName.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbPoNumber = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FPoNumber);
                    if (!poNumber.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!poNumber.Equals(dbPoNumber))
                            continue;
                    }
                    else if (poNumber.IsNullOrEmptyOrWhiteSpace() && !dbPoNumber.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbTrademarkStyle = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FTrademarkStyle);
                    if (!trademarkStyle.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!trademarkStyle.Equals(dbTrademarkStyle))
                            continue;
                    }
                    else if (trademarkStyle.IsNullOrEmptyOrWhiteSpace() && !dbTrademarkStyle.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbPackType = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FPackType);
                    if (!packType.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!packType.Equals(dbPackType))
                            continue;
                    }
                    else if (packType.IsNullOrEmptyOrWhiteSpace() && !dbPackType.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbTransportType = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FTransportType);
                    if (!transportType.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!transportType.Equals(dbTransportType))
                            continue;
                    }
                    else if (transportType.IsNullOrEmptyOrWhiteSpace() && !dbTransportType.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    string dbStockDate = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FStockDate);
                    string currStockDate =
                        GetStockDate(barCode, plan);
                    if (!currStockDate.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!currStockDate.Equals(dbStockDate))
                            continue;
                    }
                    else if (currStockDate.IsNullOrEmptyOrWhiteSpace() && !dbStockDate.IsNullOrEmptyOrWhiteSpace())
                    {
                        continue;
                    }
                    if (this.Context.CompanyCode == "2")
                    {
                        string dbMemo = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FMemo);
                        if (!remark.IsNullOrEmptyOrWhiteSpace())
                        {
                            if (!remark.Equals(dbMemo))
                                continue;
                        }
                        else if (remark.IsNullOrEmptyOrWhiteSpace() && !dbMemo.IsNullOrEmptyOrWhiteSpace())
                        {
                            continue;
                        }
                    }
                    stockArea =
                         plan.GetValue<string>(Const_StockAreaData.Entry_Column_FStockArea);
                    stock =
                        plan.GetValue<string>(Const_StockAreaData.Entry_Column_FStock);
                    stockAreaPlanName
                        = plan.GetValue<string>(Const_StockAreaData.Entry_Column_FName);
                    dataPlan = plan;
                    break;
                }

            }
            if (stockArea.IsNullOrEmptyOrWhiteSpace())
            {
                ErrorInfo info = new ErrorInfo();
                info.Message = "没有匹配到仓库和库区！";
                info.Stack = string.Format("条码：【{0}】,产品名称:【{1}】,分配库区失败，没有匹配到仓库和库区，请检查是否存在有效的库区数据计划！", barCode, material, stockAreaPlanName);
                info.Source = "自动扫描入库，分配库区操作";
                Logger.Log(info, Enums_ErrorLevel.Normal);
                string tip = "没有匹配到库区";
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FMessage,
                    string.Format("{0}|{1}|{2}", DateTime.Now.ToString("HH:mm:ss"), barCode, tip));
                string msg =
                    this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FResult);
                this.View.SetValue(Const_StockInAutoScan.Head_Field_FResult,
                    string.Format("{0}，{1}", msg, tip));
            }
            string stockDate =
                      GetStockDate(barCode, dataPlan);
            //先尝试更新
            UpdateObject updateObj = new UpdateObject();
            updateObj.TableName = Const_Distribution.Entry_TableName;
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FStockAreaPlan,
                "@FSTOCKAREAPLAN", stockAreaPlanName, Enums_FieldType.String));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FStock,
                "@FSTOCK", stock, Enums_FieldType.String));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FStockArea,
                 "@FSTOCKAREA", stockArea, Enums_FieldType.String));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FAmount,
                "@FAMOUNT", 1, Enums_FieldType.Int32));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FWeight,
                "@FWEIGHT", weight, Enums_FieldType.Decimal));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FDate,
                "@FDATE", DateTime.Now, Enums_FieldType.DateTime));
            updateObj.AddWhereItem(new WhereItem(Const_Distribution.Entry_Column_FBarCode,
              "@FBARCODE", barCode, Enums_FieldType.String));
            updateObj.AddWhereItem(new WhereItem(Const_Distribution.Entry_Column_FStatus,
                "@FSTATUS", 0, Enums_FieldType.Int32));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FStockDate,
                string.Format("@{0}", Const_Distribution.Entry_Column_FStockDate), stockDate,
                Enums_FieldType.String));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FOperator,
                string.Format("@{0}", Const_Distribution.Entry_Column_FOperator), Operator,
                Enums_FieldType.String));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FPaperType,
                string.Format("@{0}", Const_Distribution.Entry_Column_FPaperType), paperType, Enums_FieldType.String));
            updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FProductInfo,
                 string.Format("@{0}", Const_Distribution.Entry_Column_FProductInfo), productInfo, Enums_FieldType.String));
            if (dataPlan != null)
            {
                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FSpecification,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FSpecification),
                    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecification), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FPaperGrade,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FPaperGrade),
                    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FPaperGrade), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FCertification,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FCertification),
                    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FCertification), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FTransportType,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FTransportType),
                    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FTransportType), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FCoreDiameterOrReam,
    string.Format("@{0}", Const_Distribution.Entry_Column_FCoreDiameterOrReam),
    dataPlan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FCoreDiameterOrReam), Enums_FieldType.Decimal));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FDiameterOrSlides,
    string.Format("@{0}", Const_Distribution.Entry_Column_FDiameterOrSlides),
    dataPlan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FDiameterOrSlides), Enums_FieldType.Decimal));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FLength,
    string.Format("@{0}", Const_Distribution.Entry_Column_FLength),
    dataPlan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FLength), Enums_FieldType.Decimal));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FTrademarkStyle,
    string.Format("@{0}", Const_Distribution.Entry_Column_FTrademarkStyle),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FTrademarkStyle), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FSpecCustName,
    string.Format("@{0}", Const_Distribution.Entry_Column_FSpecCustName),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecCustName), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FSpecProdName,
    string.Format("@{0}", Const_Distribution.Entry_Column_FSpecProdName),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecProdName), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FPackType,
    string.Format("@{0}", Const_Distribution.Entry_Column_FPackType),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FPackType), Enums_FieldType.String));

                DateTime deliverDate = dataPlan.GetValue<DateTime>(Const_StockAreaData.Entry_Column_FDeliverDate);
                if (deliverDate != DateTime.MinValue)
                {
                    updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FDeliverDate,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FDeliverDate),
                    deliverDate, Enums_FieldType.DateTime));
                }

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FWeightMode,
    string.Format("@{0}", Const_Distribution.Entry_Column_FWeightMode),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FWeightMode), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FColor,
    string.Format("@{0}", Const_Distribution.Entry_Column_FColor),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FColor), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FPoNumber,
    string.Format("@{0}", Const_Distribution.Entry_Column_FPoNumber),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FPoNumber), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FMemo,
    string.Format("@{0}", Const_Distribution.Entry_Column_FMemo),
    dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FMemo), Enums_FieldType.String));

                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FCustomer,
    string.Format("@{0}", Const_Distribution.Entry_Column_FCustomer),
    dataPlan.GetValue<int>(Const_StockAreaData.Entry_Column_FCustomer), Enums_FieldType.Int32));
                updateObj.AddUpdateItem(new UpdateItem(Const_Distribution.Entry_Column_FSpCustomer,
string.Format("@{0}", Const_Distribution.Entry_Column_FSpCustomer),
dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpCustomer), Enums_FieldType.String));
            }
            string updateSql = updateObj.ToSqlString();
            OperateResult updateResult =
                DBAccessServiceHelper.ExcuteNonQuery(this.Context, updateSql, updateObj.QueryParameters.ToArray());
            if (!updateResult.IsSuccess)
            {
                ChaintMessageBox.Show("发生错误，请查看日志!");
                return stockArea;
            }
            if (updateResult.AffectRow <= 0)//如果没有更新到数据，则执行插入操作
            {
                //插入数据库
                InsertObject InsertObj = new InsertObject();
                InsertObj.TableName = Const_Distribution.Entry_TableName;
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FID,
                    "@FID", SequenceGuid.NewGuid().ToString(), Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FBarCode,
                  "@FBARCODE", barCode, Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FProduct,
                    "@FPRODUCT", materialNumber, Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FStockAreaPlan,
                    "@FSTOCKAREAPLAN", stockAreaPlanName, Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FStock,
                    "@FSTOCK", stock, Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FStockArea,
                     "@FSTOCKAREA", stockArea, Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FStatus,
                    "@FSTATUS", 0, Enums_FieldType.Int32));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FAmount,
                    "@FAMOUNT", 1, Enums_FieldType.Int32));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FWeight,
                    "@FWEIGHT", weight, Enums_FieldType.Decimal));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FDate,
                    "@FDATE", DateTime.Now, Enums_FieldType.DateTime));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FStockDate,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FStockDate), stockDate,
                    Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FOperator,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FOperator), Operator,
                    Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FPaperType,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FPaperType), paperType, Enums_FieldType.String));
                InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FProductInfo,
                    string.Format("@{0}", Const_Distribution.Entry_Column_FProductInfo), productInfo, Enums_FieldType.String));
                if (dataPlan != null)
                {
                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FSpecification,
                        string.Format("@{0}", Const_Distribution.Entry_Column_FSpecification),
                        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecification), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FPaperGrade,
                        string.Format("@{0}", Const_Distribution.Entry_Column_FPaperGrade),
                        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FPaperGrade), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FCertification,
                        string.Format("@{0}", Const_Distribution.Entry_Column_FCertification),
                        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FCertification), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FTransportType,
                        string.Format("@{0}", Const_Distribution.Entry_Column_FTransportType),
                        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FTransportType), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FCoreDiameterOrReam,
        string.Format("@{0}", Const_Distribution.Entry_Column_FCoreDiameterOrReam),
        dataPlan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FCoreDiameterOrReam), Enums_FieldType.Decimal));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FDiameterOrSlides,
        string.Format("@{0}", Const_Distribution.Entry_Column_FDiameterOrSlides),
        dataPlan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FDiameterOrSlides), Enums_FieldType.Decimal));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FLength,
        string.Format("@{0}", Const_Distribution.Entry_Column_FLength),
        dataPlan.GetValue<decimal>(Const_StockAreaData.Entry_Column_FLength), Enums_FieldType.Decimal));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FTrademarkStyle,
        string.Format("@{0}", Const_Distribution.Entry_Column_FTrademarkStyle),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FTrademarkStyle), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FSpecCustName,
        string.Format("@{0}", Const_Distribution.Entry_Column_FSpecCustName),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecCustName), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FSpecProdName,
        string.Format("@{0}", Const_Distribution.Entry_Column_FSpecProdName),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpecProdName), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FPackType,
        string.Format("@{0}", Const_Distribution.Entry_Column_FPackType),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FPackType), Enums_FieldType.String));

                    DateTime deliverDate = dataPlan.GetValue<DateTime>(Const_StockAreaData.Entry_Column_FDeliverDate);
                    if (deliverDate != DateTime.MinValue)
                    {
                        InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FDeliverDate,
                        string.Format("@{0}", Const_Distribution.Entry_Column_FDeliverDate),
                        deliverDate, Enums_FieldType.DateTime));
                    }

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FWeightMode,
        string.Format("@{0}", Const_Distribution.Entry_Column_FWeightMode),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FWeightMode), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FColor,
        string.Format("@{0}", Const_Distribution.Entry_Column_FColor),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FColor), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FPoNumber,
        string.Format("@{0}", Const_Distribution.Entry_Column_FPoNumber),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FPoNumber), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FMemo,
        string.Format("@{0}", Const_Distribution.Entry_Column_FMemo),
        dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FMemo), Enums_FieldType.String));

                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FCustomer,
        string.Format("@{0}", Const_Distribution.Entry_Column_FCustomer),
        dataPlan.GetValue<int>(Const_StockAreaData.Entry_Column_FCustomer), Enums_FieldType.Int32));
                    InsertObj.AddInsertItem(new InsertItem(Const_Distribution.Entry_Column_FSpCustomer,
string.Format("@{0}", Const_Distribution.Entry_Column_FSpCustomer),
dataPlan.GetValue<string>(Const_StockAreaData.Entry_Column_FSpCustomer), Enums_FieldType.String));
                }
                string insertSql = InsertObj.ToSqlString();
                OperateResult excuteResult =
                    DBAccessServiceHelper.ExcuteNonQuery(this.Context, insertSql, InsertObj.QueryParameters.ToArray());
                if (!excuteResult.IsSuccess)
                {
                    ErrorInfo error = new ErrorInfo();
                    error.Message = "插入入库确认数据时发生错误";
                    Logger.Log(error,Enums_ErrorLevel.Error);
                    //ChaintMessageBox.Show("发生错误，请查看日志!");
                }
            }
            return stockArea;
        }
        private string GetStockDate(string barCode, DataRow dataPlan)
        {
            string stockDate = string.Empty;
            if (barCode.Substring(2, 1) == "1")
            {
                stockDate = barCode.Substring(3, 4);
            }
            else if (barCode.Substring(2, 1) == "2")
            {
                stockDate = barCode.Substring(5, 4);
            }
            if (dataPlan == null)
                return stockDate;
            //万国入库日期获取规则
            if (this.Context.CompanyCode == "2")
            {
                string customer = this.View.GetValue<string>(Const_StockInAutoScan.Head_Field_FSpecCustName);
                //如果客户字段勾选，判断该条码是否存在客户信息，如果存在StockDate记录到天
                int isCustomer =
                    dataPlan.GetValue<int>(Const_StockAreaData.Entry_Column_FCustomer);
                if (isCustomer == 1 && !customer.IsNullOrEmptyOrWhiteSpace())
                {
                    stockDate = barCode.Substring(5, 6);
                }
            }
            return stockDate;
        }
        private string GetMaterialNumber(string materialName)
        {
            string sql = @" SELECT WLBH FROM CT_WLZD WHERE WLMC=@WLMC; ";
            List<QueryParameter> parameters
                = new List<QueryParameter>();
            parameters.Add(new QueryParameter("@WLMC", typeof(string), materialName));
            OperateResult result =
                DBAccessServiceHelper.ExcuteQuery(this.Context, sql, parameters.ToArray());
            if (!result.IsSuccess || result.ResultTable.Count() <= 0) return string.Empty;
            return result.ResultTable[0].GetValue<string>("WLBH");
        }
        /// <summary>
        /// 加载库区计划
        /// </summary>
        /// <returns></returns>
        private bool LoadStockAreaPlan()
        {
            string sql = @"SELECT * FROM (SELECT SADE.*,ISNULL(TEMP.FAMOUNT,0) AS FWAMOUNT
                    FROM T_AUTOSCAN_STOCKAREADATAENTRY SADE
                    LEFT JOIN
                    (SELECT SADIS.FSTOCK, SADIS.FSTOCKAREA, SADIS.FPRODUCT, SADIS.FSTOCKAREAPLAN, 
                    SUM(SADIS.FAMOUNT) AS  FAMOUNT
                    FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SADIS WHERE SADIS.FSTATUS = 0 GROUP BY
                    SADIS.FSTOCK, SADIS.FSTOCKAREA, SADIS.FPRODUCT, SADIS.FSTOCKAREAPLAN) TEMP
                       ON SADE.FSTOCK = TEMP.FSTOCK AND SADE.FSTOCKAREA = TEMP.FSTOCKAREA 
                    AND SADE.FPRODUCT = TEMP.FPRODUCT
                    AND SADE.FNAME = TEMP.FSTOCKAREAPLAN 
                    WHERE  SADE.FSTATUS = 1 AND SADE.FDATE BETWEEN @FBEGINDATE AND @FENDDATE) TEMP0
					WHERE TEMP0.FPLANAMOUNT-FAMOUNT-FWAMOUNT>0 ";
            List<SqlParameter> parameters
               = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FBEGINDATE", SqlDbType.DateTime));
            parameters.Add(new SqlParameter("@FENDDATE", SqlDbType.DateTime));
            parameters[0].Value = DateTime.Now.Date.AddDays(-2);
            parameters[1].Value = DateTime.Now.Date.AddDays(2).AddSeconds(-1);
            string[] tableName = new string[] { "T1" };
            OperateResults result =
                DBAccessServiceHelper.ExcuteQuery(this.Context, sql, tableName, parameters.ToArray());
            if (result.IsSuccess && result.ResultData.Tables.Count > 0)
            {
                stockAreaPlan = result.ResultData.Tables["T1"];
            }
            return result.IsSuccess;
        }
    }
}
