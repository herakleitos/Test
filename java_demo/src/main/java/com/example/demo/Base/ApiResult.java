package com.example.demo.Base;

public class ApiResult {
    private boolean Success;
    private String Message;
    private Object Data;

    public void setSuccess(boolean success)
    {
        Success = success;
    }
    public void setMessage(String message)
    {
        Message = message;
    }
    public void setData(Object data)
    {
        Data = data;
    }
    public String getMessage() {
        return Message;
    }
    public boolean isSuccess() {
        return Success;
    }
    public Object getData() {
        return Data;
    }
}
