package com.example.demo.Controller;

import java.io.Console;
import java.sql.Date;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.List;
import java.util.ArrayList;
import com.alibaba.fastjson.JSON;
import com.example.demo.Base.User;
import com.alibaba.fastjson.JSONObject;
import com.example.demo.Library.ConnMySql;
import com.example.demo.Library.ConnSqlServer;
import org.springframework.web.bind.annotation.*;

import javax.jws.soap.SOAPBinding;
import javax.validation.constraints.Null;
import java.sql.ResultSet;

@RestController
@RequestMapping("/hello")
public class HelloController {

    @RequestMapping(value = "/say",method = RequestMethod.GET)
    public String sayHello() {
        ConnMySql mysql = new ConnMySql();
        String sql =" select * from spgl_xmjbxxb where lsh>= ? and lsh<= ? and xmmc like ? ";
        List<Object> params=new ArrayList();
        params.add(1300);
        params.add(1302);
        params.add("%YZH%");
        ResultSet result = mysql.Excute(sql,params.toArray());
        StringBuffer  printStr = new StringBuffer();
        try
        {
            while (result.next())
            {
                String name  = result.getString("XMMC");
                printStr.append(name);
            }
        }
        catch (Exception ex)
        {
            printStr.append(ex.getMessage());
        }
        return printStr.toString();
    }
    @RequestMapping(value = "/format/{test}")
    public String Formatter(@PathVariable("test") User user)
    {
        return JSONObject.toJSONString(user);
    }
    @RequestMapping("/sqlservertest")
    public String SqlserverTest() {
        ConnSqlServer mysql = new ConnSqlServer();
        String sql =" select * From [dbo].[tbGTProject] where  projectid > ? and projectid<?  ";
        List<Object> params=new ArrayList();
        params.add(10);
        params.add(15);
        ResultSet result = mysql.Excute(sql, params.toArray());
        List<String>  printStr = new ArrayList<>();
        try
        {
            while (result.next())
            {
                String name  = result.getString("ProjectName");
                printStr.add(name);
            }
        }
        catch (Exception ex)
        {
            printStr.add(ex.getMessage());
        }
        return JSONObject.toJSONString(printStr);
    }
}