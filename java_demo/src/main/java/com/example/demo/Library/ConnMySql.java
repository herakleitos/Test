package com.example.demo.Library;
import java.math.BigDecimal;
import java.sql.*;

public class ConnMySql {
    /**
     * java与MySQL的连接
     * @param args
     */
    Connection conn=null;
    public Connection getConnection(){
        try{
            Class.forName("com.mysql.jdbc.Driver");  //加载数据库驱动
            System.out.println("数据库驱动加载成功");
            String url="jdbc:mysql://localhost:3306/test?serverTimezone=GMT&useUnicode=true&characterEncoding=utf-8&allowMultiQueries=true";
            //如果不加useSSL=false就会有警告，由于jdbc和mysql版本不同，有一个连接安全问题
            String user="root";
            String passWord="123456";
            //Connection对象引的是java.sql.Connection包
            conn = DriverManager.getConnection(url,user,passWord); //创建连接
            System.out.println("已成功的与数据库MySQL建立连接！！");
        }
        catch(Exception e){
            e.printStackTrace();
        }
        return conn;
    }
    public ResultSet Excute(String sql){
        try
        {
            conn=getConnection();
            Statement stmt = conn.createStatement(ResultSet.TYPE_SCROLL_INSENSITIVE,
                    ResultSet.CONCUR_READ_ONLY);
            ResultSet rs=stmt.executeQuery(sql);
            return rs;
        }
        catch(SQLException ex)
        {
            System.out.println(ex.getMessage());
            return null;
        }
    }
    public ResultSet Excute(String sql,Object[] parameters)
    {
        try {
            conn=getConnection();
            PreparedStatement statement = conn.prepareStatement(sql);
            int index =1;
            for(Object item :parameters)
            {
                String type = item.getClass().getName();
                switch(type)
                {
                    case "java.lang.Integer":
                        statement.setInt(index, (Integer) item);
                        break;
                    case "java.lang.String":
                        statement.setString(index, (String) item);
                        break;
                    case "java.lang.Double":
                        statement.setDouble(index, (Double) item);
                        break;
                    case "java.lang.Float":
                        statement.setFloat(index, (Float) item);
                        break;
                    case "java.lang.Boolean":
                        statement.setBoolean(index, (Boolean) item);
                        break;
                    case "java.math.BigDecimal":
                        statement.setBigDecimal(index, (BigDecimal) item);
                        break;
                }
                index++;
            }
            ResultSet rs= statement.executeQuery();
            return rs;
        }
        catch (Exception ex)
        {
            System.out.println(ex.getMessage());
            return null;
        }
    }
}
