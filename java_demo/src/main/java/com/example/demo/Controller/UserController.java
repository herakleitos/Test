package com.example.demo.Controller;

import com.alibaba.fastjson.JSONObject;
import com.example.demo.Base.User;
import com.example.demo.Base.ApiResult;
import com.example.demo.Library.ConnMySql;
import org.springframework.web.bind.annotation.*;

import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping("/User")
public class UserController {
    @RequestMapping(value = "/login", method = RequestMethod.POST)
    public String login(@RequestBody JSONObject jsonParam)
    {
        ConnMySql mysql = new ConnMySql();
        String sql =" select UserName,UserId,Phone from tb_user_userinfo where (userName= ? or phone= ?) and passWord= ? ";
        List<Object> params=new ArrayList();
        params.add(jsonParam.getString("userName"));
        params.add(jsonParam.getString("userName"));
        params.add(jsonParam.getString("passWord"));
        ResultSet data = mysql.Excute(sql,params.toArray());
        ApiResult result =new ApiResult();
        User user =new User();
        try
        {
            while (data.next())
            {
                String UserName  = data.getString("UserName");
                int UserId = data.getInt("UserId");
                String Phone  =data.getString("Phone");
                user.setUserName(UserName);
                user.setUserId(UserId);
                user.setPhone(Phone);
                break;
            }
        }
        catch (Exception ex)
        {
            result.setSuccess(false);
            result.setMessage(ex.getMessage());
            return JSONObject.toJSONString(result);
        }
        if(user.getUserId()<=0)
        {
            result.setSuccess(false);
            result.setMessage("登录失败，用户不存在！");
            result.setData(user);
            return JSONObject.toJSONString(result);
        }
        result.setSuccess(true);
        result.setMessage("登录成功！");
        result.setData(user);
        return JSONObject.toJSONString(result);
    }
}