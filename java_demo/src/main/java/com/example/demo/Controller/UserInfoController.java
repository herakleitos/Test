package com.example.demo.Controller;

import com.alibaba.fastjson.JSONObject;
import com.example.demo.Base.ApiResult;
import com.example.demo.Base.User;
import com.example.demo.Library.ConnMySql;
import com.example.demo.Library.ConnSqlServer;
import org.apache.ibatis.annotations.Param;
import org.apache.ibatis.io.Resources;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.session.SqlSessionFactoryBuilder;
import org.springframework.web.bind.annotation.*;
import com.example.demo.Mapper.UserMapper;
import java.io.InputStream;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

@RestController
@RequestMapping("/UserInfo")
public class UserInfoController {
    @RequestMapping(value = "/User",method = RequestMethod.GET)
    public String UserInfo(int userId) {
        ApiResult result =new ApiResult();
        try
        {
            // 指定全局配置文件
            String resource = "mybatis-config.xml";
            // 读取配置文件
            InputStream inputStream = Resources.getResourceAsStream(resource);
            // 构建sqlSessionFactory
            SqlSessionFactory sqlSessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
            // 获取sqlSession
            SqlSession sqlSession = sqlSessionFactory.openSession();
            // 操作CRUD，第一个参数：指定statement，规则：命名空间+“.”+statementId
            // 第二个参数：指定传入sql的参数：这里是用户id
             //User user1 = sqlSession.selectOne("com.example.demo.Base.User.selectUser", userId);

            UserMapper userMapper = sqlSession.getMapper(UserMapper.class);
            User user =  userMapper.selectUser(userId);

            result.setSuccess(true);
            result.setMessage("读取成功！");
            result.setData(user);
            return JSONObject.toJSONString(result);
        }
        catch (Exception ex)
        {
            result.setSuccess(false);
            result.setMessage(ex.getMessage());
            return JSONObject.toJSONString(result);
        }
    }
    @RequestMapping(value = "/User",method = RequestMethod.DELETE)
    public String DeleteUser(int userId) {
        ApiResult result =new ApiResult();
        try
        {
            // 指定全局配置文件
            String resource = "mybatis-config.xml";
            // 读取配置文件
            InputStream inputStream = Resources.getResourceAsStream(resource);
            // 构建sqlSessionFactory
            SqlSessionFactory sqlSessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
            // 获取sqlSession
            SqlSession sqlSession = sqlSessionFactory.openSession();
            // 操作CRUD，第一个参数：指定statement，规则：命名空间+“.”+statementId
            // 第二个参数：指定传入sql的参数：这里是用户id

           // int temp = sqlSession.delete("com.example.demo.Base.User.deleteUser", userId);

            UserMapper userMapper = sqlSession.getMapper(UserMapper.class);
            Integer temp = userMapper.deleteUser(userId);

            sqlSession.commit();
            result.setSuccess(true);
            result.setMessage("删除成功！");
            result.setData(temp);
            return JSONObject.toJSONString(result);
        }
        catch (Exception ex)
        {
            result.setSuccess(false);
            result.setMessage(ex.getMessage());
            return JSONObject.toJSONString(result);
        }
    }
    @RequestMapping(value = "/Users",method = RequestMethod.GET)
    public String UserList(String userName) {
        ApiResult result =new ApiResult();
        try
        {
            // 指定全局配置文件
            String resource = "mybatis-config.xml";
            // 读取配置文件
            InputStream inputStream = Resources.getResourceAsStream(resource);
            // 构建sqlSessionFactory
            SqlSessionFactory sqlSessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
            // 获取sqlSession
            SqlSession sqlSession = sqlSessionFactory.openSession();
            // 操作CRUD，第一个参数：指定statement，规则：命名空间+“.”+statementId
            // 第二个参数：指定传入sql的参数：这里是用户id
            StringBuilder sb = new StringBuilder();
//            if(userName!=null && !userName.isEmpty())
//            {
//                sb.append("%");
//                sb.append(userName);
//                sb.append("%");
//            }
           // List<User> users = sqlSession.selectList("com.example.demo.Base.User.selectUsers");

            UserMapper userMapper = sqlSession.getMapper(UserMapper.class);
            List<User> users = userMapper.selectUsers(userName);
            result.setSuccess(true);
            result.setMessage("读取成功！");
            result.setData(users);
            return JSONObject.toJSONString(result);
        }
        catch (Exception ex)
        {
            result.setSuccess(false);
            result.setMessage(ex.getMessage());
            return JSONObject.toJSONString(result);
        }
    }
    @RequestMapping(value = "/User",method = RequestMethod.PUT)
    public String UpdateUser(@RequestBody User user) {
        ApiResult result =new ApiResult();
        try
        {
            // 指定全局配置文件
            String resource = "mybatis-config.xml";
            // 读取配置文件
            InputStream inputStream = Resources.getResourceAsStream(resource);
            // 构建sqlSessionFactory
            SqlSessionFactory sqlSessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
            // 获取sqlSession
            SqlSession sqlSession = sqlSessionFactory.openSession();
            // 操作CRUD，第一个参数：指定statement，规则：命名空间+“.”+statementId
            // 第二个参数：指定传入sql的参数：这里是用户id
            int temp = sqlSession.update("com.example.demo.Base.User.updateUser", user);
            result.setSuccess(true);
            result.setMessage("更新成功！");
            result.setData(temp);
            sqlSession.commit();
            return JSONObject.toJSONString(result);
        }
        catch (Exception ex)
        {
            result.setSuccess(false);
            result.setMessage(ex.getMessage());
            return JSONObject.toJSONString(result);
        }
    }
    @RequestMapping(value = "/User",method = RequestMethod.POST)
    public String AddUser(@RequestBody User user) {
        ApiResult result =new ApiResult();
        try
        {
            // 指定全局配置文件
            String resource = "mybatis-config.xml";
            // 读取配置文件
            InputStream inputStream = Resources.getResourceAsStream(resource);
            // 构建sqlSessionFactory
            SqlSessionFactory sqlSessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
            // 获取sqlSession
            SqlSession sqlSession = sqlSessionFactory.openSession();
            // 操作CRUD，第一个参数：指定statement，规则：命名空间+“.”+statementId
            // 第二个参数：指定传入sql的参数：这里是用户id
            sqlSession.insert("com.example.demo.Base.User.insertUser", user);
            sqlSession.commit();
            int userId = user.getUserId();
            User userFromDB = sqlSession.selectOne("com.example.demo.Base.User.selectUser",userId);
            result.setSuccess(true);
            result.setMessage("更新成功！");
            result.setData(userFromDB);
            return JSONObject.toJSONString(result);
        }
        catch (Exception ex)
        {
            result.setSuccess(false);
            result.setMessage(ex.getMessage());
            return JSONObject.toJSONString(result);
        }
    }
}