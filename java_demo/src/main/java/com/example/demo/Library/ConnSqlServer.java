package com.example.demo.Library;
import java.math.BigDecimal;
import java.sql.*;

public class ConnSqlServer {
    private final static String URL = "jdbc:sqlserver://localhost:1433;DatabaseName=test";
    private static final String USER="sa";
    private static final String PASSWORD="Aa000000";
    public Connection getConnection()
    {
        Connection conn=null;
        try {
            Class.forName("com.microsoft.sqlserver.jdbc.SQLServerDriver");  //加载数据库驱动
            //2.获得数据库的连接
            conn= DriverManager.getConnection(URL,USER,PASSWORD);
        }
        catch(Exception e){
            e.printStackTrace();
        }
        return conn;
    }
    public ResultSet Excute(String sql)
    {
        try
        {
            Connection conn=getConnection();
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
            Connection conn=getConnection();
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
