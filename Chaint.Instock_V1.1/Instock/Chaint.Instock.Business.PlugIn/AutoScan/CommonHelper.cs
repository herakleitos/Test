using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chaint.Common.Core;
using Chaint.Common.Core.Utils;
using Chaint.Common.ServiceHelper;
using Chaint.Common.Core.Sql;
using Chaint.Common.Core.Enums;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Sql.DataApi;

namespace Chaint.Instock.Business.PlugIns
{
    public static class CommonHelper
    {
        public static List<Tuple<string, int>> GetProductInfoFromBD(Context ctx, List<int> groupIds, List<string> positions, string paperType)
        {
            OperateResult result = new OperateResult();
            result.IsSuccess = false;
            var mapResult = CommonHelper.GetProductIdByGroupId(ctx, groupIds,Convert.ToInt32(paperType));
            if (mapResult != null)
            {
                if (!mapResult.IsEmpty())
                {
                    List<string> productIds = new List<string>();
                    foreach (int groupId in mapResult.Keys)
                    {
                        productIds.AddRange(mapResult[groupId]);
                    }
                    int i = 0;
                    List<QueryParameter> parameters = new List<QueryParameter>();
                    productIds = productIds.Distinct().ToList();
                    StringBuilder sbFilter = new StringBuilder();
                    foreach (string item in productIds)
                    {
                        i++;
                        string paraName = string.Format("@FPRODUCTID{0}", i);
                        sbFilter.Append(paraName);
                        sbFilter.Append(',');
                        parameters.Add(new QueryParameter(paraName, typeof(string), item));
                    }
                    StringBuilder sb = new StringBuilder();
                    if (paperType == "1")
                    {
                        sb.AppendFormat(@"SELECT SAD.FBARCODE,TEMP.FSTOCK,
                                        TEMP.FSTOCKAREANAME,
                                        RP.WIDTHLABEL,
                                        RP.DIAMETERLABEL,
                                        RP.LENGTHLABEL,
                                        CO.COLORDESC AS COLOR,
                                        RPSC.SPECCUSTNAME AS SPECCUSTNAME,
                                        RPSP.SPECPRODNAME AS SPECPRODNAME,
                                        RP.ORDERNO AS ORDERNO,
                                        RP.CUSTTRADEMARK,
                                        CASE  ISNULL(RP.WEIGHTMODE ,'1') 
                                        WHEN 1 THEN '计米' WHEN 2 THEN  '称重' ELSE '注米' END AS WEIGHTMODE
                                        FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SAD
                                        LEFT JOIN [ROLL_PRODUCT] RP ON SAD.FBARCODE = RP.ROLLID
                                        LEFT JOIN PAPER_SPECCUSTNAME RPSC ON RPSC.ONLYID=RP.SPECCUSTNAME
                                        LEFT JOIN PAPER_SPECPRODNAME RPSP ON RPSP.ONLYID=RP.SPECPRODNAME
                                        LEFT JOIN PAPER_COLOR CO ON CO.COLOR = RP.COLOR
                                        LEFT JOIN (
                                        SELECT SA.FSTOCK, SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,SAE.FLOCATION
                                        FROM T_AUTOSCAN_STOCKAREA SA INNER JOIN T_AUTOSCAN_STOCKAREAENTRY SAE
                                        ON SA.FID = SAE.FHEADID
                                        ) TEMP ON SAD.FSTOCK = TEMP.FSTOCK AND SAD.FSTOCKAREA = TEMP.FSTOCKAREANUMBER
                                        WHERE SAD.FSTATUS = 0 AND SAD.FBARCODE IN ({0}) ORDER BY SAD.FDATE DESC;",
                                sbFilter.ToString().TrimEnd(','));
                    }
                    else
                    {
                        sb.AppendFormat(@"SELECT MTRL.WLMC AS FPRODUCTNAME,SAD.FBARCODE,TEMP.FSTOCK,
                                        TEMP.FSTOCKAREANAME,
					                    SP.WIDTHLABEL AS WIDTHLABEL,
					                    SP.LENGHTLABEL AS LENGHTLABEL ,
					                    PPS.PACKTYPE AS PACKTYPE,
					                    CO.COLORDESC AS COLOR,
					                    SPTT.PLYWOOKDPACK AS ISPLYWOOKDPACK,
					                    SPSC.SPECCUSTNAME AS SPECCUSTNAME,
					                    SPSP.SPECPRODNAME AS SPECPRODNAME,
					                    SP.ORDERNO AS ORDERNO,
					                    SPPCF.PAPERCERT AS PAPERCERT,
					                    SP.PALLETREAMS,SP.FIBERDIRECT AS WEIGHTMODE
                                        FROM T_AUTOSCAN_STOCKAREADISTRIBUTION SAD
                                        LEFT JOIN[CT_WLZD] MTRL ON SAD.FPRODUCT = MTRL.WLBH
					                    LEFT JOIN [SHEET_PRODUCT] SP ON SAD.FBARCODE = SP.SHEETID
					                    LEFT JOIN PAPER_PACKTYPE PPS ON SP.REAMPACKTYPE=PPS.ONLYID
					                    LEFT JOIN PAPER_PLYWOOKDPACK SPTT ON SPTT.ONLYID = SP.ISPLYWOOKDPACK
					                    LEFT JOIN PAPER_SPECCUSTNAME SPSC ON SPSC.ONLYID=SP.SPECCUSTNAME
					                    LEFT JOIN PAPER_SPECPRODNAME SPSP ON SPSP.ONLYID=SP.SPECPRODNAME
					                    LEFT JOIN PAPER_CERTIFICATION SPPCF ON SPPCF.ONLYID=SP.PAPERCERT
				                        LEFT JOIN PAPER_COLOR CO ON CO.COLOR = SP.COLOR
                                        LEFT JOIN (
                                        SELECT SA.FSTOCK, SAE.FSTOCKAREANUMBER,SAE.FSTOCKAREANAME,SAE.FLOCATION
                                        FROM T_AUTOSCAN_STOCKAREA SA INNER JOIN T_AUTOSCAN_STOCKAREAENTRY SAE
                                        ON SA.FID = SAE.FHEADID
                                        ) TEMP ON SAD.FSTOCK = TEMP.FSTOCK AND SAD.FSTOCKAREA = TEMP.FSTOCKAREANUMBER
                                        WHERE SAD.FSTATUS = 0  AND SAD.FBARCODE IN ({0}) ORDER BY SAD.FDATE DESC;",
                                            sbFilter.ToString().TrimEnd(','));
                    }
                    result =
                        DBAccessServiceHelper.ExcuteQuery(ctx, sb.ToString(), parameters.ToArray());
                }

            }
            if (mapResult == null)
            {
                result.IsSuccess = true;
                return CombineMessage(result, null, positions, paperType);
            }
            return CombineMessage(result, mapResult, positions, paperType);
        }
        public static int GetMaxGroupId(Context ctx,int paperType)
        {
            string sql = @"SELECT ISNULL(MAX(FID), 100) AS FID FROM T_AUTOSCAN_GROUPIDMAPPING WHERE FPAPERTYPE = @FPAPERTYPE;";
            List<QueryParameter> parameters = new List<QueryParameter>();
            parameters.Add(new QueryParameter("@FPAPERTYPE", typeof(int), paperType));
            OperateResult result =
              DBAccessServiceHelper.ExcuteQuery(ctx, sql, parameters.ToArray());
            if (result.IsSuccess && result.ResultTable.Count() > 0)
            {
                int fid = result.ResultTable[0].GetValue<int>("FID");
                return fid;
            }
            else { return 0; }
        }
        public static bool InsertGroupIdMapping(Context ctx, int nextGroupId, List<string> barcodes,int paperType)
        {
            List<ExcuteObject> exeObjs = new List<ExcuteObject>();
            foreach (string barcode in barcodes)
            {
                InsertObject insObj = new InsertObject();
                insObj.TableName = "T_AUTOSCAN_GROUPIDMAPPING";
                insObj.InsertItems.Add(new InsertItem("FID", "@FID", nextGroupId, Enums_FieldType.Int32));
                insObj.InsertItems.Add(new InsertItem("FBARCODE", "@FBARCODE", barcode, Enums_FieldType.String));
                insObj.InsertItems.Add(new InsertItem("FPAPERTYPE", "@FPAPERTYPE", paperType, Enums_FieldType.Int32));
                ExcuteObject exeObj = new ExcuteObject();
                exeObj.Sql = insObj.ToSqlString();
                exeObj.Parameters = insObj.SqlParameters;
                exeObjs.Add(exeObj);
            }
            OperationResult result =
                DBAccessServiceHelper.ExcuteWithTransaction(ctx, exeObjs);
            return result.IsSuccess;
        }
        public static bool InsertScanResult(Context ctx, int paperCount, IList<string> barcodes)
        {
            List<ExcuteObject> exeObjs = new List<ExcuteObject>();
            string groupId = SequenceGuid.NewGuid().ToString();
            DateTime now = DateTime.Now;
            int i = 0;
            foreach (string barcode in barcodes)
            {
                InsertObject insObj = new InsertObject();
                insObj.TableName = "T_AUTOSCAN_SCANLOG";
                insObj.InsertItems.Add(new InsertItem("FGROUPID", string.Format("@FGROUPID{0}", i),
                    groupId
                    , Enums_FieldType.String));
                insObj.InsertItems.Add(new InsertItem("FBARCODE",
                    string.Format("@FBARCODE{0}", i), barcode, Enums_FieldType.String));
                insObj.InsertItems.Add(new InsertItem("FDATE",
                    string.Format("@FDATE{0}", i), now, Enums_FieldType.DateTime));
                insObj.InsertItems.Add(new InsertItem("FCOUNT",
                    string.Format("@FCOUNT{0}", i), paperCount, Enums_FieldType.Int32));
                ExcuteObject exeObj = new ExcuteObject();
                exeObj.Sql = insObj.ToSqlString();
                exeObj.Parameters = insObj.SqlParameters;
                exeObjs.Add(exeObj);
                i++;
            }
            OperationResult result =
                DBAccessServiceHelper.ExcuteWithTransaction(ctx, exeObjs);
            return result.IsSuccess;
        }
        private static Dictionary<int, List<string>>
            GetProductIdByGroupId(Context ctx, List<int> groupIds,int paperType)
        {
            if (groupIds.IsEmpty()) return null;
            List<string> barcodes = new List<string>();
            Dictionary<int, List<string>> groupIdMap = new Dictionary<int, List<string>>();
            string sql = @"SELECT DISTINCT GOUP.FID,GOUP.FBARCODE FROM T_AUTOSCAN_GROUPIDMAPPING GOUP
                           WHERE GOUP.FID IN ({0}) AND FPAPERTYPE=@FPAPERTYPE; ";
            StringBuilder sbFilter = new StringBuilder();
            int i = 0;
            List<QueryParameter> parameters = new List<QueryParameter>();
            foreach (int item in groupIds)
            {
                if (item <= 0) continue;
                i++;
                string paraName = string.Format("@FID{0}", i);
                sbFilter.Append(paraName);
                sbFilter.Append(',');
                parameters.Add(new QueryParameter(paraName, typeof(int), item));
            }
            parameters.Add(new QueryParameter("@FPAPERTYPE", typeof(int), paperType));
            if (parameters.IsEmpty()) return null;
            sql = string.Format(sql, sbFilter.ToString().TrimEnd(','));
            OperateResult opResut =
              DBAccessServiceHelper.ExcuteQuery(ctx, sql, parameters.ToArray());
            if (opResut.IsSuccess && opResut.ResultTable.Count() > 0)
            {
                foreach (var item in opResut.ResultTable)
                {
                    int fid = item.GetValue<int>("FID");
                    string productId = item.GetValue<string>("FBARCODE");
                    if (groupIdMap.Keys.Contains(fid))
                    {
                        groupIdMap[fid].Add(productId);
                    }
                    else
                    {
                        groupIdMap.Add(fid, new List<string>() { productId });
                    }
                }
                return groupIdMap;
            }
            else { return null; }
        }
        public static void AddEmptyMessage(List<Tuple<string, int>> message, string paperType)
        {
            Tuple<string, int> itemEmpty = new Tuple<string, int>("", 0);
            message.Add(itemEmpty);
            message.Add(itemEmpty);
            message.Add(itemEmpty);
            message.Add(itemEmpty);
            message.Add(itemEmpty);
            message.Add(itemEmpty);
            message.Add(itemEmpty);
            if (paperType == "1")
            {
                message.Add(itemEmpty);
            }
        }
        public static List<Tuple<string, int>> CombineMessage(OperateResult result,
            Dictionary<int, List<string>> groupIdMap, List<string> positions, string paperType)
        {
            List<Tuple<string, int>> message = new List<Tuple<string, int>>();
            foreach (string position in positions)
            {
                if (position.IsNullOrEmptyOrWhiteSpace())
                {
                    Tuple<string, int> item = new Tuple<string, int>("", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                if (position == "1")
                {
                    Tuple<string, int> item = new Tuple<string, int>("条码扫描失败", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                if (position == "2")
                {
                    Tuple<string, int> item = new Tuple<string, int>("产品入库失败", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                if (position == "3")
                {
                    Tuple<string, int> item = new Tuple<string, int>("产品重复入库", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                if (position == "4")
                {
                    Tuple<string, int> item = new Tuple<string, int>("1个条码未扫到", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                if (position == "5")
                {
                    Tuple<string, int> item = new Tuple<string, int>("2个条码未扫到", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                if (!result.IsSuccess)
                {
                    Tuple<string, int> item = new Tuple<string, int>("产品信息显示失败", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                int id = Convert.ToInt32(position);
                List<string> barcodes = new List<string>();
                if (groupIdMap == null || !groupIdMap.TryGetValue(id, out barcodes) || barcodes.IsEmpty())
                {
                    Tuple<string, int> item = new Tuple<string, int>("", 0);
                    message.Add(item);
                    AddEmptyMessage(message, paperType);
                    continue;
                }
                foreach (string barcode in barcodes)
                {
                    bool isMatch = false;
                    if (result.ResultTable.Count() == 0)
                    {
                        isMatch = false;
                    }
                    else
                    {
                        foreach (var item in result.ResultTable)
                        {
                            string idDb = item.GetValue<string>("FBARCODE");
                            if (!idDb.EqualIgnorCase(barcode))
                            {
                                continue;
                            }
                            isMatch = true;
                            if (paperType == "1")
                            {
                                AddRollProductInfo(item, message);
                            }
                            else if (paperType == "2")
                            {
                                AddSheetProductInfo(item, message);                    
                            }
                            break;
                        }
                    }
                    if (!isMatch)
                    {
                        string msg = "产品信息显示失败";
                        Tuple<string, int> tupleItem1 = new Tuple<string, int>(msg, 1);
                        Tuple<string, int> tupleItem2 = new Tuple<string, int>(string.Empty, 1);
                        message.Add(tupleItem1);
                        if (paperType == "2")
                        {
                            message.Add(tupleItem2);
                            message.Add(tupleItem2);
                            message.Add(tupleItem2);
                            message.Add(tupleItem2);
                            message.Add(tupleItem2);
                            message.Add(tupleItem2);
                            message.Add(tupleItem2);
                        }
                        else
                        {
                            message.Add(tupleItem2);
                        }
                    }
                }
            }
            return message;
        }

        private static void AddRollProductInfo(IRow item, List<Tuple<string, int>> message)
        {
            string stock = item.GetValue<string>("FSTOCK");
            string stockArea = item.GetValue<string>("FSTOCKAREANAME");
            string specification = string.Empty;
            string color = string.Empty;
            string[] planLength;
            string[] planDiameter;
            string specCustName = string.Empty;
            string spCustomer = string.Empty;
            string orderNo = string.Empty;
            string weightMode = string.Empty;
            if (item["WIDTHLABEL"] == null)
            {
                specification = string.Empty;
            }
            else
            {
                specification = Convert.ToString(item["WIDTHLABEL"]);
            }
            if (item["COLOR"] == null)
            {
                color = string.Empty;
            }
            else
            {
                color = Convert.ToString(item["COLOR"]);
            }
            if (item["LENGTHLABEL"] == null)
            {
                planLength = new string[] {"","" };
            }
            else
            {
                planLength = Convert.ToString(item["LENGTHLABEL"]).Split('.');
            }
            if (item["DIAMETERLABEL"] == null)
            {
                planDiameter = new string[] { "", "" };
            }
            else
            {
                planDiameter = Convert.ToString(item["DIAMETERLABEL"]).Split('.');
            }
            if (item["SPECCUSTNAME"] == null)
            {
                specCustName = string.Empty;
            }
            else
            {
                specCustName = Convert.ToString(item["SPECCUSTNAME"]);
            }
            if (item["CUSTTRADEMARK"] == null)
            {
                spCustomer = string.Empty;
            }
            else
            {
                spCustomer = Convert.ToString(item["CUSTTRADEMARK"]);
            }
            if (item["ORDERNO"] == null)
            {
                orderNo = string.Empty;
            }
            else
            {
                orderNo = Convert.ToString(item["ORDERNO"]);
            }
            if (item["WEIGHTMODE"] == null)
            {
                weightMode = string.Empty;
            }
            else
            {
                weightMode = Convert.ToString(item["WEIGHTMODE"]);
            }
            string line1 = string.Format("{0} {1}{2}",
               stockArea.IsNullOrEmptyOrWhiteSpace() ? "分配失败" : stockArea, specification,color);
            string line2 = string.Format("{0}{1}{2}",
               planLength[0].IsNullOrEmptyOrWhiteSpace()? planDiameter[0]: planLength[0],
               spCustomer.IsNullOrEmptyOrWhiteSpace()?"":string.Format("{0} ",spCustomer), specCustName);
            string line3 = string.Format("{0}{1}",weightMode,orderNo);
            message.Add(new Tuple<string, int>(line1, 1));
            message.Add(new Tuple<string, int>(line2, 1));
            message.Add(new Tuple<string, int>(line3, 1));
        }

        private static void AddSheetProductInfo(IRow item, List<Tuple<string, int>> message)
        {
            string[] width;
            string[] height;
            string stock = item.GetValue<string>("FSTOCK");
            string productName = item.GetValue<string>("FPRODUCTNAME");
            string stockArea = item.GetValue<string>("FSTOCKAREANAME");
            string packType = string.Empty;
            string specification = string.Empty;
            string color = string.Empty;
            string isPlyWookdPack = string.Empty;
            string specCustName = string.Empty;
            string specProdName = string.Empty;
            string orderNo = string.Empty;
            string paperCert = string.Empty;
            string[] ream;
            string weightMode = string.Empty;
            if (item["WIDTHLABEL"] == null)
            {
                width = new string[] { "" };
            }
            else
            {
                width = Convert.ToString(item["WIDTHLABEL"]).Split('.');
            }
            if (item["LENGHTLABEL"] == null)
            {
                height = new string[] { "" };
            }
            else
            {
                height = Convert.ToString(item["LENGHTLABEL"]).Split('.');
            }
            specification = string.Format("{0}-{1}", width[0], height[0]);
            if (item["LENGHTLABEL"] == null)
            {
                packType = string.Empty;
            }
            else
            {
                packType = Convert.ToString(item["PACKTYPE"]);
            }
            if (item["COLOR"] == null)
            {
                color = string.Empty;
            }
            else
            {
                color = Convert.ToString(item["COLOR"]);
            }
            if (item["ISPLYWOOKDPACK"] == null)
            {
                isPlyWookdPack = string.Empty;
            }
            else
            {
                isPlyWookdPack = Convert.ToString(item["ISPLYWOOKDPACK"]);
            }
            if (item["SPECCUSTNAME"] == null)
            {
                specCustName = string.Empty;
            }
            else
            {
                specCustName = Convert.ToString(item["SPECCUSTNAME"]);
            }
            if (item["SPECPRODNAME"] == null)
            {
                specProdName = string.Empty;
            }
            else
            {
                specProdName = Convert.ToString(item["SPECPRODNAME"]);
            }
            if (item["ORDERNO"] == null)
            {
                orderNo = string.Empty;
            }
            else
            {
                orderNo = Convert.ToString(item["ORDERNO"]);
            }
            if (item["PAPERCERT"] == null)
            {
                paperCert = string.Empty;
            }
            else
            {
                paperCert = Convert.ToString(item["PAPERCERT"]);
            }
            if (item["PALLETREAMS"] == null)
            {
                ream = new string[] { "" };
            }
            else
            {
                ream = Convert.ToString(item["PALLETREAMS"]).Split('.');
            }
            if (item["WEIGHTMODE"] == null)
            {
                weightMode = string.Empty;
            }
            else
            {
                weightMode = Convert.ToString(item["WEIGHTMODE"]);
            }
            string line1 = string.Format("{0} {1}",
                stockArea.IsNullOrEmptyOrWhiteSpace() ? "分配失败" : stockArea, specification);
            string line2 = productName;
            string line3 = packType;
            string line4 = string.Format("{0} {1}", color, isPlyWookdPack);
            string line5 = specCustName;
            string line6 = specProdName;
            string line7 = paperCert;
            string line8 = string.Format("{0}{1}{2}", ream[0], weightMode, orderNo);
            List<Tuple<string, int>> temp = new List<Tuple<string, int>>();
            if (!line1.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line1, 1));
            }
            if (!line2.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line2, 1));
            }
            if (!line3.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line3, 1));
            }
            if (!line4.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line4, 1));
            }
            if (!line5.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line5, 1));
            }
            if (!line6.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line6, 1));
            }
            if (!line7.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line7, 1));
            }
            if (!line8.IsNullOrEmptyOrWhiteSpace())
            {
                temp.Add(new Tuple<string, int>(line8, 1));
            }
            message.AddRange(temp);
            int emptyCount = 8 - temp.Count;
            if (emptyCount > 0)
            {
                for (int i = 0; i < emptyCount; i++)
                {
                    message.Add(new Tuple<string, int>(string.Empty, 1));
                }
            }
        }
    }
}
