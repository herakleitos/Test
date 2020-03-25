package com.example.demo.Base;

public class User {
    private int UserId;
    private String UserName;
    private String Phone;

    public int getUserId()
    {
        return UserId;
    }
    public void setUserId(int _UserId)
    {
        UserId = _UserId;
    }
    public String getPhone() { return Phone; }
    public void setPhone(String _Phone)
    {
        Phone = _Phone;
    }
    public String getUserName()
    {
        return UserName;
    }
    public void setUserName(String _UserName)
    {
        UserName = _UserName;
    }
}
